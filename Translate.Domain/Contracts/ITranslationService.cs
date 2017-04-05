using Translate.Domain.Entities;
using System.Collections.Generic;

namespace Translate.Domain.Contracts
{
    public interface ITranslationService
    {
        string TranslateSingle(string from, string to, string text);

        IEnumerable<Language> GetLanguageNames();

        IEnumerable<Language> SupportedLanguages { get; }
    }
}
