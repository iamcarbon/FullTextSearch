using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Protsyk.PMS.FullText.Core.Automata;

/// <summary>
/// Deterministic finite automaton
/// </summary>
public class DFA
{
    public static readonly int NoState = -1;

    private readonly List<List<ValueTuple<CharRange, int>>> transitions = new();
    private readonly HashSet<int> final = new();

    public void AddState(int state, bool isFinal)
    {
        if (state != transitions.Count)
        {
            throw new ArgumentException();
        }

        transitions.Add(new List<ValueTuple<CharRange, int>>());
        if (isFinal)
        {
            final.Add(state);
        }
    }

    public void AddTransition(int from, int to, CharRange c)
    {
        transitions[from].Add(new ValueTuple<CharRange, int>(c, to));
    }

    public int Next(int s, char c)
    {
        if (s == NoState)
        {
            return NoState;
        }
        foreach (var t in transitions[s])
        {
            if (t.Item1.Contains(c))
            {
                return t.Item2;
            }
        }
        return NoState;
    }

    public bool IsFinal(int s)
    {
        return final.Contains(s);
    }

    public string ToDotNotation()
    {
        var result = new StringBuilder();
        result.AppendLine("digraph DFA {");
        result.AppendLine("rankdir = LR;");
        result.AppendLine("orientation = Portrait;");

        for (int i = 0; i < transitions.Count; ++i)
        {
            if (i == 0)
            {
                result.AppendLine(CultureInfo.InvariantCulture, $"{i}[label = \"{i}\", shape = circle, style = bold, fontsize = 14]");
            }
            else if (final.Contains(i))
            {
                result.AppendLine(CultureInfo.InvariantCulture, $"{i}[label = \"{i}\", shape = doublecircle, style = bold, fontsize = 14]");
            }
            else
            {
                result.AppendLine(CultureInfo.InvariantCulture, $"{i}[label = \"{i}\", shape = circle, style = solid, fontsize = 14]");
            }

            foreach (var t in transitions[i])
            {
                result.AppendLine(CultureInfo.InvariantCulture, $"{i}->{t.Item2} [label = \"{t.Item1}\", fontsize = 14];");
            }
        }

        result.AppendLine("}");
        return result.ToString();
    }
}
