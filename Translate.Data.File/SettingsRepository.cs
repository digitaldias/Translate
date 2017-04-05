using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Reflection;
using Translate.Domain.Contracts;

namespace Translate.Data.File
{
    public class SettingsRepository : ISettingsRepository
    {
        private JObject _jsonContents;
        private string _settingsFileName;


        public SettingsRepository()
        {
            var assemblyName = Assembly.GetEntryAssembly().GetName().Name;

            _settingsFileName = $"{assemblyName}-Secrets.json";
        }


        public string GetFromUserDocuments(string settingName)
        {
            if (_jsonContents == null)
                LoadContentsFromSecretsFile();

            return _jsonContents[settingName].Value<string>();
        }


        private void LoadContentsFromSecretsFile()
        {
            var fullPathToSettingsFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), _settingsFileName);

            if (!System.IO.File.Exists(fullPathToSettingsFile))
                return;

            var fileContents = System.IO.File.ReadAllText(fullPathToSettingsFile);

            _jsonContents = JsonConvert.DeserializeObject<JObject>(fileContents);
        }
    }
}
