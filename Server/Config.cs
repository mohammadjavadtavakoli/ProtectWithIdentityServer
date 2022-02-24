using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityResource = IdentityServer4.Models.IdentityResource;
using ApiScope = IdentityServer4.Models.ApiScope;
using ApiResource = IdentityServer4.Models.ApiResource;
using Secret = IdentityServer4.Models.Secret;
using Client = IdentityServer4.Models.Client;
namespace Server
{
    public class Config
    {
       
        public static IEnumerable<IdentityResource> IdentityResources =>
        new[]
       {
            //claims_supported
              new IdentityResources.OpenId(),
              new IdentityResources.Profile(),
              new IdentityResource
              {
                  Name="role",
                  UserClaims= new List<string>{"role"}
              }
        };

        public static IEnumerable<ApiScope> ApiScopes =>
            new[] { new ApiScope("CofeeAPI.read"), new ApiScope("CofeeAPI.write"), };

        public static IEnumerable<ApiResource> ApiResources =>
            new[]
            {
                new ApiResource("CofeeAPI")
                {
                    Scopes= new List<string> { "CofeeAPI.read", "CofeeAPI.write" },
                    ApiSecrets= new List<Secret> {new Secret("ScopeSecret".Sha256())},
                    UserClaims= new List<string> {"role"}
                }
            };
        public static IEnumerable<Client> Clients =>
            new[]
            {
                new Client
                {
                 //m2m client credentials flow client

                    ClientId="m2m.client",
                    ClientName="Client Credentials Client",
                    ClientSecrets={new Secret("clientSecret1".Sha256())},
                    AllowedScopes={"CofeeAPI.read","CofeeAPI.write"}
                },

                //interactive client using code flow + pkce

                new Client
                {
                    ClientId = "interactive",
                    ClientSecrets = { new Secret("ClientSecret1".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = { "https://localhost:5444/signin-oidc" },
                    FrontChannelLogoutUri = "https://localhost:5444/signout-oidc",
                    PostLogoutRedirectUris = { "https://localhost:5444/signout-callback-oidc" },
                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "CoffeeAPI.read" },
                    RequirePkce = true,
                    RequireConsent = true,
                    AllowPlainTextPkce = false
                }
            };


    }
}
