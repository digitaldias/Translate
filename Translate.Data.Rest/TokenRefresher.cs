using RestSharp;
using System.Net;
using System.Threading;
using Translate.Domain.Contracts;

namespace Translate.Data.Rest
{
    public class TokenRefresher : ITokenRefresher
    {
        private static string COGNITIVE_API_URL   = "https://api.cognitive.microsoft.com/sts/v1.0/";
        private static int NINE_MINUTES           = 9 * 60 * 1000; // Update token every 9 minutes. The token expires every 10 minutes

        private readonly ISettingsReader    _settingsReader;
        private readonly ILogger            _logger;
        private RestClient                  _restClientCognitive;
        private Timer                       _tokenRefreshTimer;

        private string                      _authorizationToken;
        private string                      _bearerToken;


        public TokenRefresher(ILogger logger, ISettingsReader settingsReader)
        {
            _logger              = logger;
            _settingsReader      = settingsReader;
            _restClientCognitive = new RestClient(COGNITIVE_API_URL);
            _tokenRefreshTimer   = new Timer(RefreshToken, null, 0, NINE_MINUTES);

            while (string.IsNullOrEmpty(_authorizationToken))
                Thread.Sleep(50);
        }


        private void RefreshToken(object _)
        {
            _logger.LogInfo("Acquire token");

            _authorizationToken = string.Empty;
            var request         = new RestRequest("issueToken", Method.POST);

            request.AddHeader("Ocp-Apim-Subscription-Key", _settingsReader["Translator:AccountKey"]);

            var response = _restClientCognitive.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                _authorizationToken = response.Content;
                _bearerToken        = "Bearer " + _authorizationToken;
            }
        }


        public string BearerToken
        {
            get { return _bearerToken; }
        }
    }
}
