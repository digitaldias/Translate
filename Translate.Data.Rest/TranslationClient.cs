using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Translate.Domain.Contracts;
using Translate.Domain.Entities;
using System;
using System.Xml.Linq;

namespace Translate.Business
{
    public class TranslationClient : ITranslationClient
    {
        private static string TRANSLATION_API_URL = "https://api.microsofttranslator.com/v2/http.svc";
        private RestClient                  _restClientTranslation;

        private readonly ITokenRefresher    _tokenRefresher;

        private ILogger                     _logger;

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


        public IEnumerable<Language> GetLanguageCodes()
        {
            var request = CreateAuthorizedRequest("GetLanguagesForTranslate", Method.GET);            
            var result  = _restClientTranslation.Execute<List<string>>(request);

            if (result.StatusCode == HttpStatusCode.OK)
            {
                return result.Data.Select(s => new Language { Code = s });                
            }
            return new List<Language>();
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


        private RestRequest CreateAuthorizedRequest(string resource, Method method)
        {
            var request = new RestRequest(resource, method);
            request.AddParameter("appid", _tokenRefresher.BearerToken);
            
            return request;
        }
    }
}
