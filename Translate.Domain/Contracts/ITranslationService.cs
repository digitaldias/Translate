using Translate.Domain.Entities;
using System.Collections.Generic;

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
        /// <param name="from">Language to translate from</param>
        /// <param name="to">Language to translate to</param>
        /// <param name="text">the text to be translated</param>
        /// <returns>Translated string</returns>
        string TranslateSingle(Language from, Language to, string text);


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


        /// <summary>
        /// Provides a way to give Translation API a better translation for 
        /// a common text. 
        /// </summary>
        /// <param name="from">Language to translate from</param>
        /// <param name="to">Language to translate to</param>
        /// <param name="originalText">Max. 1000 characters</param>
        /// <param name="translatedText">Max. 2000 characters</param>
        /// <param name="userName">A string used to track the originator of the submission</param>
        /// <returns>True if accepted. False otherwise. See ErrorLog for details on fail</returns>
        bool AddTranslation(Language from, Language to, string originalText, string translatedText, string userName);
    }
}
