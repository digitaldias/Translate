using Oversett.Domain.Contracts;
using RestSharp;
using RestSharp.Extensions.MonoHttp;
using System.Threading;

namespace Oversett.Business
{
    public class TranslationClient : ITranslationClient
    {
        private static string TRANSLATION_URL = "https://api.cognitive.microsoft.com/sts/v1.0/";
        private static int NINE_MINUTES       = 9 * 60 * 1000;
        private Timer _tokenRefreshTimer;
        private RestClient _restClient;
        private string _authorizationToken;
        private readonly ISettingsReader _settingsReader;


        public TranslationClient(ISettingsReader settingsReader)
        {
            _settingsReader    = settingsReader;
            _restClient        = new RestClient(TRANSLATION_URL);
            _tokenRefreshTimer = new Timer(RefreshToken, null, 0, NINE_MINUTES);
            Thread.Sleep(250);
        }


        private void RefreshToken(object _)
        {
            _authorizationToken = string.Empty;
            var request         = new RestRequest("issueToken", Method.POST);
            var subscriptionKey = _settingsReader["Translator:AccountKey"];

            request.AddHeader("Ocp-Apim-Subscription-Key", subscriptionKey);

            var response = _restClient.Execute(request);

            if (response.ResponseStatus == ResponseStatus.Completed)
                _authorizationToken = response.Content;
        }


        public string TranslateSingle(string untranslated, string from, string to)
        {
            var bearer = "Bearer " + _authorizationToken;
            var request = new RestRequest("Translate", Method.GET);

            request.AddParameter("appid", "");
            request.AddParameter("to", to);
            request.AddParameter("text", HttpUtility.UrlEncode(untranslated));

            request.AddHeader("Authorization", bearer);
            request.AddHeader("Accept", "application/json");

            var result = _restClient.Execute(request);

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
                return result.Content;

            return string.Empty;
        }
    }
}
