namespace Oversett.Domain.Contracts
{
    public interface ITranslationService
    {
        string TranslateSingle(string from, string to, string text);
    }
}
