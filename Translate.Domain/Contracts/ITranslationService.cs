using Translate.Domain.Entities;
using System.Collections.Generic;

namespace Translate.Domain.Contracts
{
    public interface ITranslationService
    {
        Language DetectLanguage(string text);

        string TranslateSingle(string from, string to, string text);

        string TranslateSingle(Language from, Language to, string text);

        Dictionary<string,Language> SupportedLanguages { get; }

        IEnumerable<int> BreakSentences(Language textLanguage, string text);
    }
}
