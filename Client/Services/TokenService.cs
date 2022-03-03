using IdentityModel;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client.Services
{
    public class TokenService : ITokenService
    {
        private readonly IOptions<IdentityServerSettings> _IdentityServerSettings;
        private readonly DiscoveryDocumentResponse _discoveryDocument;
        private readonly HttpClient httpClient;

        public TokenService(IOptions<IdentityServerSettings> identityServerSettings)
        {
            _IdentityServerSettings = identityServerSettings;
            httpClient = new HttpClient();
            _discoveryDocument = httpClient.GetDiscoveryDocumentAsync(this._IdentityServerSettings.Value.DiscoveryUrl).Result;

            if (_discoveryDocument.IsError)
            {
                throw new Exception("Unable to get discovery document", _discoveryDocument.Exception);
            }
        }

        public async Task<TokenResponse> GetToken(string scope)
        {
            var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address=_discoveryDocument.TokenEndpoint,
                ClientId=_IdentityServerSettings.Value.ClientName,
                ClientSecret=_IdentityServerSettings.Value.ClientPassword,
                Scope =scope
            });

            if(tokenResponse.IsError)
            {
                throw new Exception("unable to get token", tokenResponse.Exception);
            }

            return tokenResponse;
        }
    }
}
