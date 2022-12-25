namespace Protsyk.PMS.FullText.Core;

public sealed class PersistentIndexName : IIndexName
{
    public static string DefaultValue = "Default";

    public PersistentIndexName(string folder)
        : this(folder, DefaultValue, DefaultValue, DefaultValue, DefaultValue)
    {
    }

    public PersistentIndexName(string folder, string dictionaryType, string fieldsType, string postingType, string textEncoding)
    {
        Folder = folder;
        DictionaryType = dictionaryType;
        FieldsType = fieldsType;
        PostingType = postingType;
        TextEncoding = textEncoding;
    }

    public string Folder { get; }

    public string DictionaryType { get; set; }

    public string FieldsType { get; set; }

    public string PostingType { get; set; }

    public string TextEncoding { get; set; }
}
