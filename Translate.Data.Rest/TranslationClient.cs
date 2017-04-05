using Translate.Domain.Contracts;
using RestSharp;
using RestSharp.Extensions.MonoHttp;
using System.Threading;
using Translate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;

namespace Translate.Business
{
    public class TranslationClient : ITranslationClient
    {
        private static string COGNITIVE_API_URL   = "https://api.cognitive.microsoft.com/sts/v1.0/";
        private static string TRANSLATION_API_URL = "https://api.microsofttranslator.com/v2/http.svc";
        private static int NINE_MINUTES           = 9*60*1000; // Update token every 9 minutes. The token expires every 10 minutes
        private Timer _tokenRefreshTimer;
        private RestClient _restClientCognitive;
        private RestClient _restClientTranslation;
        private string _authorizationToken;
        private readonly ISettingsReader _settingsReader;
        private ILogger _logger;


        public TranslationClient(ISettingsReader settingsReader, ILogger logger)
        {
            _logger = logger;
            _settingsReader    = settingsReader;
            _restClientCognitive        = new RestClient(COGNITIVE_API_URL);
            _restClientTranslation = new RestClient(TRANSLATION_API_URL);
            _tokenRefreshTimer = new Timer(RefreshToken, null, 0, NINE_MINUTES);

            while(string.IsNullOrEmpty(_authorizationToken))
            {
                Thread.Sleep(250);
            }
        }


        private void RefreshToken(object _)
        {
            _logger.LogInfo("Acquire token");
            _authorizationToken = string.Empty;
            var request         = new RestRequest("issueToken", Method.POST);
            var subscriptionKey = _settingsReader["Translator:AccountKey"];

            request.AddHeader("Ocp-Apim-Subscription-Key", subscriptionKey);

            var response = _restClientCognitive.Execute(request);

            if (response.ResponseStatus == ResponseStatus.Completed)
                _authorizationToken = response.Content;
        }


        public string TranslateSingle(string from, string to, string untranslated)
        {            
            var request = new RestRequest("Translate", Method.GET);

            request.AddParameter("appid", BearerToken);
            request.AddParameter("from", from);
            request.AddParameter("to", to);
            request.AddParameter("text", untranslated);
            
            request.AddHeader("accept", "application/xml");

            var result = _restClientTranslation.Execute(request);

            if (result.StatusCode == HttpStatusCode.OK)
                return result.Content;

            return string.Empty;
        }


        public IEnumerable<Language> GetLanguageNames()
        {
            var request = new RestRequest("GetLanguagesForTranslate", Method.GET);
            request.AddParameter("appId", BearerToken);
            request.AddHeader("accept", "application/xml");

            var result = _restClientTranslation.Execute<List<string>>(request);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                return result.Data.Select(s => new Language { Code = s });
            }

            return new List<Language>();
        }


        private string BearerToken
        {
            get
            {
                return "Bearer " + _authorizationToken;
            }
        }
    }
}
