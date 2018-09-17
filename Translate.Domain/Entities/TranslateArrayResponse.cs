namespace Translate.Domain.Entities
{
    public class TranslateArrayResponse
    {
        /// <summary>
        /// Language of the original text
        /// </summary>
        public Language From { get; set; }


        /// <summary>
        /// The translated text
        /// </summary>
        public string TranslatedText { get; set; }
    }
}
