using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Translate.Domain.Contracts;
using Translate.Domain.Entities;

namespace Translate.Business
{
    public class TranslationService : ITranslationService
    {
        private readonly IExceptionHandler _exceptionHandler;
        private readonly ITranslationClient _translationClient;
        private Dictionary<string, Language> _supportedLanguages;
        private readonly ILogger _logger;


        public TranslationService(IExceptionHandler exceptionHandler, ITranslationClient translationClient, ILogger logger)
        {
            _exceptionHandler  = exceptionHandler;
            _translationClient = translationClient;
            _logger            = logger;
        }

        public Language DetectLanguage(string text)
        {
            if(string.IsNullOrEmpty(text))
                return null;

            // API doesen't support detection of over 100 000 characters
            if (text.Length > 100000)
                text = text.Substring(0, 100000);

            return  _exceptionHandler.Run(() => _translationClient.DetectLanguage(text));
        }


        public string TranslateSingle(string from, string to, string text)
        {
            if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to) || string.IsNullOrEmpty(text))
            {
                _logger.LogError("TranslateSingle(from, to, text) has one or more empty parameters. All 3 are required");
                return string.Empty;
            }

            if (!SupportedLanguages.Values.Any(l => l.Code == from))
            {
                _logger.LogError($"Paramameter 'from' used invalid language code '{from}'");
                return string.Empty;
            }

            if (!SupportedLanguages.Values.Any(l => l.Code == to))
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


        public string TranslateSingle(Language from, Language to, string text)
        {
            return TranslateSingle(from.Code, to.Code, text);
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


        public bool AddTranslation(Language from, Language to, string originalText, string translatedText, string userName)
        {
            if (from == null || to == null)
            {
                _logger.LogError("FROM and TO languages cannot be null. They're both required");
                return false;
            }

            if (string.IsNullOrEmpty(originalText) || string.IsNullOrEmpty(translatedText))
            {
                _logger.LogError("Input texts cannot be empty");
                return false;
            }

            if(string.IsNullOrEmpty(userName))
            {
                _logger.LogError("The userName cannot be null");
                return false;
            }

            if (originalText.Length > 1000)
            {
                _logger.LogError("Original text cannot exceed 1000 characters");
                return false;
            }

            if (translatedText.Length > 2000)
            {
                _logger.LogError("Translated text cannot exceed 2000 characters");
                return false;
            }
            return _exceptionHandler.Run(() => _translationClient.AddTranslation(from, to, originalText, translatedText, userName));
        }


        private IEnumerable<Language> GetLanguageCodes()
        {
            return _exceptionHandler.Run(() => _translationClient.GetLanguageCodes());
        }
    }
}
