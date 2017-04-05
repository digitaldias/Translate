using System.Collections.Generic;
using Oversett.Domain.Entities;

namespace Oversett.Domain.Contracts
{
    public interface ITranslationClient
    {
        string TranslateSingle(string from, string to, string untranslated);
        IEnumerable<Language> GetLanguageNames();
    }
}
