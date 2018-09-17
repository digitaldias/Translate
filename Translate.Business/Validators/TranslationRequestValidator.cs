using System;
using Translate.Domain.Contracts;
using Translate.Domain.Contracts.Validators;
using Translate.Domain.Entities;

namespace Translate.Business.Validators
{
    public class TranslationRequestValidator : ITranslationRequestValidator
    {
        private readonly ILanguageValidator _languageValidator;
        private readonly ILogger _logger;


        public TranslationRequestValidator(ILogger logger, ILanguageValidator languageValidator)
        {
            _languageValidator = languageValidator ?? throw new ArgumentNullException("languageValidator");
            _logger = logger ?? throw new ArgumentNullException("logger");
        }


        public bool IsValid(TranslationRequest entity)
        {
            if (entity == null)
                return false;

            if(!_languageValidator.IsValid(entity.From)){
                _logger.LogError("Request language FROM was invalid");
                return false;
            }

            if (!_languageValidator.IsValid(entity.To)){
                _logger.LogError("Request language TO was invalid");
                return false;
            }

            if(string.IsNullOrEmpty(entity.Text)){
                _logger.LogError("No text to translate");
                return false;
            }
            return true;
        }
    }
}
