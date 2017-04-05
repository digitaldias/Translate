namespace Translate.Domain.Contracts
{
    public interface ISettingsReader
    {
        string this[string name] { get; }
    }
}
