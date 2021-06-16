using System;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace IdentityServer
{
    public class Config
    {
        private const string MOVIES_API = "moviesAPI";
        private const string MOVIES_MVC_CLIENT = "movies_mvc_client";
        private const string MOVIES_DESCRIPTION = "Movies API";
        private const string MOVIES_MVC_DESCRIPTION = "Movies MVC Web App";
        private const string MOVIES_CLIENT = "moviesClient";
        private const string MOVIES_SECRET = "smeagolCries";
        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = MOVIES_CLIENT,
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret(MOVIES_SECRET.Sha256())
                    },
                    AllowedScopes = {MOVIES_API} //what api are we protecting?  this is what area we can access
                },
                new Client
                {
                    ClientId = MOVIES_MVC_CLIENT,
                    ClientName = MOVIES_MVC_DESCRIPTION,
                    AllowedGrantTypes = GrantTypes.Code, //provides info about the flow - using the token when logging in with user credentials
                    AllowRememberConsent = false,
                    RedirectUris = new List<string>()
                    {
                        "https://localhost:5002/signin-oidc" //this is hard coded badness but is what the mvc app is configured to use
                    },
                    PostLogoutRedirectUris = new List<string>()
                    {
                        "https://localhost:5002/signout-callback-oidc"
                    },
                    ClientSecrets = new List<Secret>
                    {
                        new Secret(MOVIES_SECRET.Sha256())
                    },
                    AllowedScopes = new List<string> //which areas can we access?
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }
                }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope(MOVIES_API, MOVIES_DESCRIPTION)
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {

            };

        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile() 
            };

        //Test users are only for dev - not for prod code!
        public static List<TestUser> TestUsers =>
            new List<TestUser>
            {
                new TestUser()
                {
                    SubjectId = Guid.NewGuid().ToString(),
                    Username = "Sammy",
                    Password = "Screams",
                    Claims = new List<Claim>
                    {
                        new Claim(JwtClaimTypes.GivenName, "Sam"),
                        new Claim(JwtClaimTypes.FamilyName, "Kinnison")
                    }
                }
            };
    }
}
