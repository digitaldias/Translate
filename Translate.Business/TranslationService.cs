using System;
using System.Collections.Generic;
using Translate.Domain.Contracts;
using Translate.Domain.Entities;
using System.Linq;
using System.Xml.Linq;

namespace Translate.Business
{
    public class TranslationService : ITranslationService
    {
        private readonly IExceptionHandler _exceptionHandler;
        private readonly ITranslationClient _translationClient;
        private IEnumerable<Language> _supportedLanguages;
        private readonly ILogger _logger;


        public TranslationService(IExceptionHandler exceptionHandler, ITranslationClient translationClient, ILogger logger)
        {
            _exceptionHandler  = exceptionHandler;
            _translationClient = translationClient;
            _logger            = logger;
        }


        public IEnumerable<Language> SupportedLanguages
        {
            get
            {
                if (_supportedLanguages == null)
                    _supportedLanguages = new List<Language>(GetLanguageNames());

                return _supportedLanguages;
            }
        }


        private IEnumerable<Language> GetLanguageNames()
        {
            return _exceptionHandler.Run(() => _translationClient.GetLanguageNames());
        }


        public string TranslateSingle(string from, string to, string text)
        {
            if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to) || string.IsNullOrEmpty(text))
            {
                _logger.LogError("TranslateSingle(from, to, text) has one or more empty parameters. All 3 are required");
                return string.Empty;
            }

            if (!SupportedLanguages.Any(l => l.Code == from))
            {
                _logger.LogError($"Paramameter 'from' used invalid language code '{from}'");
                return string.Empty;
            }

            if (!SupportedLanguages.Any(l => l.Code == to))
            {
                _logger.LogError($"Paramameter 'to' used invalid language code '{to}'");
                return string.Empty;
            }
            return _exceptionHandler.Run(() =>
            {
                var translatedResult = _translationClient.TranslateSingle(from, to, text);
                var xmlElement = XElement.Parse(translatedResult);
                return xmlElement.Value;
            });        
        }

        
    }
}
