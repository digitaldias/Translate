using Translate.Domain.Entities;

namespace Translate.Domain.Contracts
{
    public interface IValidator
    {
        bool IsValidTranslateArrayRequest(TranslateArrayRequest translateArrayRequest);
    }
}