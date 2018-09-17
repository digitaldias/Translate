using System;
using System.Collections.Generic;

namespace Translate.Domain.Entities
{
    [Serializable]
    public class TranslateArrayRequest
    {
        /// <summary>
        /// Optional. 
        /// A string representing the language code to translate the text from. 
        /// If left empty the response will include the result of language auto-detection
        /// </summary>
        public Language From { get; set; }


        /// <summary>
        /// Required. 
        /// A string representing the language code to translate the text into.
        /// </summary>
        public Language To { get; set; }


        /// <summary>
        /// A string containing the category (domain) of the translation. Defaults to general
        /// </summary>
        public string Category { get; set; }


        /// <summary>
        ///  If you want to avoid getting profanity in the translation, regardless of the presence of profanity in the source text, you can use the profanity filtering option. 
        ///  The option allows you to choose whether you want to see profanity deleted or marked with appropriate tags, or no action taken. 
        ///  The accepted values of ProfanityAction are  NoAction (default), Marked and Deleted.
        /// </summary>
        public ProfanityAction ProfanityAction { get; set; }


        /// <summary>
        /// Required. An array containing the texts for translation. 
        /// All strings must be of the same language. 
        /// The total of all texts to be translated must not exceed 10000 characters. 
        /// The maximum number of array elements is 2000.
        /// </summary>
        public string[] Texts { get; set; }
    }
}
