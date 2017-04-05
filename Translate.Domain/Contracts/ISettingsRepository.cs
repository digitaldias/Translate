namespace Translate.Domain.Contracts
{
    public interface ISettingsRepository
    {
        string GetFromUserDocuments(string settingName);
    }
}