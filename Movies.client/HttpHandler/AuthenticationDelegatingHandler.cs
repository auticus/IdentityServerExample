using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace Movies.client.HttpHandler
{
    public class AuthenticationDelegatingHandler : DelegatingHandler
    {
        //intercepts HTTP requests and inject the identity server token

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ClientCredentialsTokenRequest _tokenRequest;

        public AuthenticationDelegatingHandler(IHttpClientFactory clientFactory, ClientCredentialsTokenRequest tokenRequest)
        {
            _httpClientFactory = clientFactory;
            _tokenRequest = tokenRequest;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient("IdentityServerClient");
            var response = await client.RequestClientCredentialsTokenAsync(_tokenRequest);
            if (response.IsError) throw new HttpRequestException("Error while requesting access token");

            request.SetBearerToken(response.AccessToken);
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
