namespace Translate.Domain.Entities
{
    /// <summary>
    /// Translation request object, defines the languate to translate from and to, 
    /// as well as the text to transate
    /// </summary>
    public class TranslationRequest
    {
        public Language From { get; set; }

        public Language To { get; set; }

        public string Text { get; set; }
    }
}
