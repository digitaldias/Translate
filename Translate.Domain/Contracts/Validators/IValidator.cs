namespace Translate.Domain.Contracts.Validators
{
    public interface IValidator<TEntity> where TEntity : class
    {
        bool IsValid(TEntity entity);
    }
}
