using System.Collections.Generic;
using Translate.Domain.Entities;

namespace Translate.Domain.Contracts
{
    public interface ITranslationClient
    {
        string TranslateSingle(string from, string to, string untranslated);

        IEnumerable<Language> GetLanguageCodes();

        Language DetectLanguage(string text);
    }
}
