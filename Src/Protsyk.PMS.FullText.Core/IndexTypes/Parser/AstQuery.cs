using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Protsyk.PMS.FullText.Core;

public abstract class AstQuery
{
    protected AstQuery(string name)
    {
        Name = name;
    }

    public string Name { get; }

    protected internal abstract StringBuilder ToString(StringBuilder builder);

    public override string ToString()
    {
        return ToString(new StringBuilder()).ToString();
    }
}

public sealed class FunctionAstQuery : AstQuery
{
    public readonly List<AstQuery> Args = new();

    public FunctionAstQuery(string name)
        : base(name)
    {
    }

    protected internal override StringBuilder ToString(StringBuilder builder)
    {
        builder.Append(Name);
        if (Args.Count > 0)
        {
            builder.Append('(');
            var first = true;
            foreach (var child in Args)
            {
                if (!first)
                {
                    builder.Append(',');
                }
                builder = child.ToString(builder);
                first = false;
            }
            builder.Append(')');
        }
        return builder;
    }
}

public abstract class TermAstQuery : AstQuery
{
    public TermAstQuery(string name, string value, string escapedValue)
        : base(name)
    {
        Value = value;
        EscapedValue = escapedValue;
    }

    public string Value { get; }

    public string EscapedValue { get; }

    protected internal override StringBuilder ToString(StringBuilder builder)
    {
        builder.Append(Name);
        builder.Append('(');
        builder.Append(EscapedValue);
        builder.Append(')');
        return builder;
    }
}

public sealed class WordAstQuery : TermAstQuery
{
    public WordAstQuery(string name, string value, string escapedValue)
        : base(name, value, escapedValue)
    {
    }
}

public sealed class WildcardAstQuery : TermAstQuery
{
    public WildcardAstQuery(string name, string value, string escapedValue)
        : base(name, value, escapedValue)
    {
    }
}

public sealed class EditAstQuery : TermAstQuery
{
    public EditAstQuery(string name, string value, string escapedValue, int distance)
        : base(name, value, escapedValue)
    {
        Distance = distance;
    }

    public int Distance { get; }

    protected internal override StringBuilder ToString(StringBuilder builder)
    {
        builder.Append(CultureInfo.InvariantCulture, $"{Name}({EscapedValue},{Distance})");

        return builder;
    }
}
