using System.Collections.Generic;
using System.Threading.Tasks;
using Translate.Domain.Entities;

namespace Translate.Domain.Contracts
{
    public interface ITranslationClient
    {
        string TranslateSingle(string from, string to, string untranslated);

        Task<string> TranslateSingleAsync(string from, string to, string untranslated);

        IEnumerable<Language> GetLanguageCodes();

        Task<IEnumerable<Language>> GetLanguageCodesAsync();

        Language DetectLanguage(string text);

        Task<Language> DetectLanguageAsync(string text);

        IEnumerable<int> BreakSentences(Language englishLanguage, string englishPhrase);
    }
}
