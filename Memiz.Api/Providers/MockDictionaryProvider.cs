using Memiz.Api.Interfaces;
using Memiz.Api.Models;

namespace Memiz.Api.Providers;

public class MockDictionaryProvider : IDictionaryProvider
{
    public string Name => "default";

    public DictionaryEntry GetEntry(string input, string sourceLanguage, string targetLanguage)
    {
        return new DictionaryEntry
        {
            Input = input,
            Meaning = $"Meaning of {input}",
            Phonetic = $"/{input}/",
            PartOfSpeech = "noun"
        };
    }
}