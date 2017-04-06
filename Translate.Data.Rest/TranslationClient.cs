using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;
using Translate.Domain.Contracts;
using Translate.Domain.Entities;

namespace Translate.Business
{
    public class TranslationClient : ITranslationClient
    {
        private static string            TRANSLATION_API_URL = "https://api.microsofttranslator.com/v2/http.svc";
        private RestClient               _restClientTranslation;
        private readonly ITokenRefresher _tokenRefresher;
        private ILogger                  _logger;


        public TranslationClient(ILogger logger, ITokenRefresher tokenRefresher)
        {
            _logger         = logger;
            _tokenRefresher = tokenRefresher;
            _restClientTranslation = new RestClient(TRANSLATION_API_URL);
        }


        public string TranslateSingle(string from, string to, string untranslated)
        {            
            var request = CreateAuthorizedRequest("Translate", Method.GET);

            request.AddParameter("from", from);
            request.AddParameter("to", to);
            request.AddParameter("text", untranslated);
            
            request.AddHeader("accept", "application/xml");

            var result = _restClientTranslation.Execute(request);

            if (result.StatusCode == HttpStatusCode.OK)
                return result.Content;

            return string.Empty;
        }


        public async Task<string> TranslateSingleAsync(string from, string to, string untranslated)
        {
            return await Task.Run(() => TranslateSingle(from, to, untranslated));
        }


        public IEnumerable<Language> GetLanguageCodes()
        {
            var request = CreateAuthorizedRequest("GetLanguagesForTranslate", Method.GET);            
            var result  = _restClientTranslation.Execute<List<string>>(request);
            var languages = new List<Language>();
            if (result.StatusCode == HttpStatusCode.OK)
            {
                languages = new List<Language>(result.Data.Select(s => new Language { Code = s }));                
            }
            return languages;
        }


        public async Task<IEnumerable<Language>> GetLanguageCodesAsync()
        {
            return await Task.Run(() => GetLanguageCodes());
        }


        public Language DetectLanguage(string text)
        {
            var request = CreateAuthorizedRequest("Detect", Method.GET);
            request.AddParameter("text", text);

            var result = _restClientTranslation.Execute(request);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                var xml = XElement.Parse(result.Content);
                return new Language { Code = xml.Value};
            }
            return null;
        }


        public async Task<Language> DetectLanguageAsync(string text)
        {
            return await Task.Run(() => DetectLanguage(text));
        }


        private RestRequest CreateAuthorizedRequest(string resource, Method method)
        {
            var request = new RestRequest(resource, method);
            request.AddParameter("appid", _tokenRefresher.BearerToken);
            
            return request;
        }


        public IEnumerable<int> BreakSentences(Language language, string text)
        {
            var request = CreateAuthorizedRequest("/BreakSentences", Method.GET);
            request.AddParameter("language", language.Code);
            request.AddParameter("text", text);

            var result = _restClientTranslation.Execute(request);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                var xmlParsed = XElement.Parse(result.Content);
                foreach (var element in xmlParsed.Elements())
                {
                    yield return int.Parse(element.Value);
                }
            }            
        }

        private string GetLanguageNames(string[] languageCodes)
        {
            var newClient = new RestClient("https://api.microsofttranslator.com/v2/http.svc");
            var request = CreateAuthorizedRequest("/GetLanguageNames", Method.POST);
            request.AddParameter("locale", "no");
            request.AddParameter("languageCodes", "en,no,fr");
            

            var response = newClient.Execute(request);
            if(response.StatusCode == HttpStatusCode.OK)
            {
                return response.Content;
            }
            return string.Empty;
        }
    }
}
