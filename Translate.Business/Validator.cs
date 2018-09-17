using Translate.Domain.Contracts;
using Translate.Domain.Entities;

namespace Translate.Business
{
    public class Validator : IValidator
    {
        public bool IsValidTranslateArrayRequest(TranslateArrayRequest translateArrayRequest)
        {
            return true;
        }
    }
}
