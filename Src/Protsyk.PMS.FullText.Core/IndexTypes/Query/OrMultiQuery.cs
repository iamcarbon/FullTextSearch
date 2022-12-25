using System;
using System.Collections.Generic;

using Protsyk.PMS.FullText.Core.Collections;

namespace Protsyk.PMS.FullText.Core;

public sealed class OrMultiQuery : ISearchQuery
{
    private readonly ISearchQuery[] queries;
    private readonly Heap<ValueTuple<IMatch, ISearchQuery>> heap;
    private States state;

    private enum States
    {
        Initial,
        MatchReturn,
        MatchAdvance,
        Consumed
    }

    public OrMultiQuery(params ISearchQuery[] queries)
    {
        this.queries = queries;
        this.state = States.Initial;
        this.heap = new Heap<ValueTuple<IMatch, ISearchQuery>>(
                Comparer<ValueTuple<IMatch, ISearchQuery>>.Create(
                    (x, y) => MatchComparer.Instance.Compare(x.Item1, y.Item1)));
    }


    public IMatch NextMatch()
    {
        while (true)
        {
            switch (state)
            {
                case States.Consumed:
                    return null;
                case States.Initial:
                    {
                        state = States.MatchReturn;
                        foreach (var searchQuery in queries)
                        {
                            if (searchQuery.NextMatch() is { } match)
                            {
                                heap.Add(new (match, searchQuery));
                            }
                        }
                    }
                    break;
                case States.MatchReturn:
                    if (heap.Count is 0)
                    {
                        state = States.Consumed;
                        return null;
                    }
                    state = States.MatchAdvance;
                    return heap.Top.Item1;
                case States.MatchAdvance:
                    {
                        var top = heap.RemoveTop();
                        var searchQuery = top.Item2;
                        if (searchQuery.NextMatch() is { } match)
                        {
                            heap.Add(new (match, searchQuery));
                        }
                        state = States.MatchReturn;
                    }
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }
    }

    public void Dispose()
    {
        foreach (var searchQuery in queries)
        {
            searchQuery.Dispose();
        }
    }
}
