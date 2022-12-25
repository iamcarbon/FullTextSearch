namespace Protsyk.PMS.FullText.Core;

/// <summary>
/// Query that returns no results
/// </summary>
public sealed class NullQuery : ISearchQuery
{
    public static readonly NullQuery Instance = new();

    private NullQuery() { }

    public IMatch NextMatch() => null;

    public void Dispose() { }
}