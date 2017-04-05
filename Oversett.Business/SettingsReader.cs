using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Oversett.Domain.Contracts;
using System;
using System.IO;

namespace Oversett.Business
{
    public class SettingsReader : ISettingsReader
    {
        private JObject _json;


        public SettingsReader()
        {
                var specialFolder      = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var pathToSettingsFile = Path.Combine(specialFolder, "Oversett secrets.json");

                if (!File.Exists(pathToSettingsFile))
                    throw new InvalidProgramException($"Did not find '{pathToSettingsFile}'. Please make sure it's there");

            var fileContents = File.ReadAllText(pathToSettingsFile);
            _json = JsonConvert.DeserializeObject<JObject>(fileContents);
        }


        public string this[string name]
        {
            get
            {
                return _json[name].Value<string>();
            }
        }
    }
}
