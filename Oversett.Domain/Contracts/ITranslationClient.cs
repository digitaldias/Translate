namespace Oversett.Domain.Contracts
{
    public interface ITranslationClient
    {
        string TranslateSingle(string untranslated, string from, string to);
    }
}
