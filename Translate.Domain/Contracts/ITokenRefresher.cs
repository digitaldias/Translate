namespace Translate.Domain.Contracts
{
    public interface ITokenRefresher
    {
        string BearerToken { get; }
    }
}
