using Newtonsoft.Json.Linq;
using Translate.Domain.Contracts;

namespace Translate.Business
{
    public class SettingsReader : ISettingsReader
    {
        private JObject _json;
        private readonly ISettingsRepository _settingsRepository;
        private readonly IExceptionHandler _exceptionHandler;

        public SettingsReader(ISettingsRepository settingsRepository, IExceptionHandler exceptionHandler)
        {
            _settingsRepository = settingsRepository;
            _exceptionHandler = exceptionHandler;
        }


        public string this[string name]
        {
            get
            {
                return _exceptionHandler.Run(() => _settingsRepository.GetFromUserDocuments(name));
            }
        }
    }
}
