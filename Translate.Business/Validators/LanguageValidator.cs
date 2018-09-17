using System;
using Translate.Domain.Contracts;
using Translate.Domain.Contracts.Validators;
using Translate.Domain.Entities;

namespace Translate.Business.Validators
{
    public class LanguageValidator : ILanguageValidator
    {
        private readonly ILogger _logger;

        public LanguageValidator(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException("logger");
        }


        public bool IsValid(Language entity)
        {
            if(entity == null)
            {
                _logger.LogError("Language is null");
                return false;
            }

            if(string.IsNullOrEmpty(entity.Code))
            {
                _logger.LogError("Language code is null or empty");
                return false;
            }

            if(entity.Code.Length > 4 || entity.Code.Length <= 1)
            {
                _logger.LogError($"Language code '{entity.Code}' does not appear to be the right length");
                return false;
            }
            return true;
        }
    }
}
