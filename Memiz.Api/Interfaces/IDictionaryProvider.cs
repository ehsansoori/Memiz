using Memiz.Api.Models;

namespace Memiz.Api.Interfaces
{
    public interface IDictionaryProvider
    {
        string Name { get; }
        DictionaryEntry GetEntry(string input, string sourceLanguage, string targetLanguage);

    }
}
