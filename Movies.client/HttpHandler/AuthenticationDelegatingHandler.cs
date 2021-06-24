using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Movies.client.HttpHandler
{
    public class AuthenticationDelegatingHandler : DelegatingHandler
    {
        //intercepts HTTP requests and inject the identity server token

        /* disabled when going to hybrid grant type
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ClientCredentialsTokenRequest _tokenRequest;

        public AuthenticationDelegatingHandler(IHttpClientFactory clientFactory, ClientCredentialsTokenRequest tokenRequest)
        {
            _httpClientFactory = clientFactory;
            _tokenRequest = tokenRequest;
        }
        */

        private readonly IHttpContextAccessor _httpContextAccessor;  //added when adding Hybrid grant type

        public AuthenticationDelegatingHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            /* Disabled with hybrid - since we are going to get token back we dont need to manually hit up identity server any longer

            var client = _httpClientFactory.CreateClient("IdentityServerClient");
            var response = await client.RequestClientCredentialsTokenAsync(_tokenRequest);
            if (response.IsError) throw new HttpRequestException("Error while requesting access token");

            request.SetBearerToken(response.AccessToken);
            return await base.SendAsync(request, cancellationToken);
            */

            //use httpcontextaccessor to get the token with hybrid grant type
            //using hybrid grant type - when we log in to identity server it sends the token across and its already a part of the httpContext.
            var accessToken =
                await _httpContextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                request.SetBearerToken(accessToken);
            }
            
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
