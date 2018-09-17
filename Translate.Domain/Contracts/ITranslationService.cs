using System.Collections.Generic;
using Translate.Domain.Entities;

namespace Translate.Domain.Contracts
{
    public interface ITranslationService
    {
        /// <summary>
        /// Detect what language the given text is written in
        /// </summary>
        /// <param name="text">The text to do language detection on</param>
        /// <returns>The corresponding Language object</returns>
        Language DetectLanguage(string text);


        /// <summary>
        /// Translate a single piece of text
        /// </summary>
        /// <returns>Translated string</returns>
        string TranslateSingle(TranslationRequest translationRequest);


        /// <summary>
        /// Provides a list of supported language codes to be used in the Translator. 
        /// The list is obtained immediately when the TranslationService is initiated
        /// </summary>
        Dictionary<string,Language> SupportedLanguages { get; }


        /// <summary>
        /// Break down a text into sentences and give back the length of each.
        /// </summary>
        /// <param name="textLanguage">The language of the provided text</param>
        /// <param name="text">The text to analyze</param>
        /// <returns>Enumerable of ints where each represents the character length of each sentence</returns>
        IEnumerable<int> BreakSentences(Language textLanguage, string text);
    }
}
