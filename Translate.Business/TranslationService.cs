using System.Collections.Generic;
using System.Xml.Linq;
using Translate.Domain.Contracts;
using Translate.Domain.Contracts.Validators;
using Translate.Domain.Entities;

namespace Translate.Business
{
    public class TranslationService : ITranslationService
    {
        private readonly IExceptionHandler            _exceptionHandler;
        private readonly ITranslationRequestValidator _translationRequestValidator;
        private readonly ITranslationClient           _translationClient;
        private Dictionary<string, Language>          _supportedLanguages;
        private readonly ILogger                      _logger;


        public TranslationService(IExceptionHandler exceptionHandler, ITranslationRequestValidator translationRequestValidator, ITranslationClient translationClient, ILogger logger)
        {
            _exceptionHandler            = exceptionHandler;
            _translationRequestValidator = translationRequestValidator;
            _translationClient           = translationClient;
            _logger                      = logger;
        }


        public Language DetectLanguage(string text)
        {
            if(string.IsNullOrEmpty(text))
                return null;

            // API doesen't support detection of over 100 000 characters
            if (text.Length > 100_000)
                text = text.Substring(0, 100_000);

            return  _exceptionHandler.Run(() => _translationClient.DetectLanguage(text));
        }


        public string TranslateSingle(TranslationRequest translationRequest)
        {
            if(!_translationRequestValidator.IsValid(translationRequest))
            {
                _logger.LogError("Received an invalid Translation request object");
                return string.Empty;
            }
            return _exceptionHandler.Run(() =>
            {
                var translatedResult = _translationClient.TranslateSingle(translationRequest);
                var xmlElement       = XElement.Parse(translatedResult);
                return xmlElement.Value;
            });        
        }


        public Dictionary<string, Language> SupportedLanguages
        {
            get
            {
                if (_supportedLanguages == null)
                {
                    var languageCodes = GetLanguageCodes();
                    _supportedLanguages = new Dictionary<string, Language>();

                    foreach (var languageCode in languageCodes)
                        _supportedLanguages[languageCode.Code] = languageCode;
                }
                return _supportedLanguages;
            }
        }


        public IEnumerable<int> BreakSentences(Language textLanguage, string text)
        {
            if(textLanguage == null || string.IsNullOrEmpty(text))
                return null;

            return _exceptionHandler.Run(() => _translationClient.BreakSentences(textLanguage, text));
        }


        private IEnumerable<Language> GetLanguageCodes()
        {
            return _exceptionHandler.Run(() => _translationClient.GetLanguageCodes());
        }
    }
}