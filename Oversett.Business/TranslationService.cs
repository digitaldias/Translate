using Oversett.Domain.Contracts;

namespace Oversett.Business
{
    public class TranslationService : ITranslationService
    {
        private readonly IExceptionHandler _exceptionHandler;
        private readonly ITranslationClient _translationClient;

        public TranslationService(IExceptionHandler exceptionHandler, ITranslationClient translationClient)
        {
            _exceptionHandler  = exceptionHandler;
            _translationClient = translationClient;
        }

        public string TranslateSingle(string from, string to, string text)
        {
            if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to) || string.IsNullOrEmpty(text))
                return string.Empty;
               
            return _exceptionHandler.Run(() => _translationClient.TranslateSingle(text, from, to));
        }
    }
}
