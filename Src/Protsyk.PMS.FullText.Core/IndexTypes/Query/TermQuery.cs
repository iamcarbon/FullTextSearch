using System;
using System.Collections.Generic;

namespace Protsyk.PMS.FullText.Core;

public sealed class TermQuery : ISearchQuery
{
    private readonly IPostingList postings;
    private readonly MatchIterator matchIterator;
    private bool consumed;

    public TermQuery(IPostingList postings)
    {
        this.postings = postings;
        this.matchIterator = new MatchIterator(postings.GetEnumerator());
        this.consumed = false;
    }

    public IMatch NextMatch()
    {
        if (consumed)
        {
            return null;
        }

        if (matchIterator.NextMatch())
        {
            return matchIterator;
        }

        consumed = true;
        return null;
    }

    public void Dispose()
    {
        matchIterator?.Dispose();
    }

    #region Types
    private sealed class MatchIterator: IMatch, IDisposable
    {
        private readonly IEnumerator<Occurrence> postings;

        public MatchIterator(IEnumerator<Occurrence> postings)
        {
            this.postings = postings;
        }

        public bool NextMatch()
        {
            if (postings.MoveNext())
            {
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"{{{postings.Current}}}";
        }

        public IEnumerable<Occurrence> GetOccurrences()
        {
            yield return postings.Current;
        }

        public Occurrence Left => postings.Current;

        public Occurrence Right => postings.Current;

        public Occurrence Max => postings.Current;

        public Occurrence Min => postings.Current;

        public ulong DocumentId => postings.Current.DocumentId;

        public void Dispose()
        {
            postings?.Dispose();
        }
    }
    #endregion
}
