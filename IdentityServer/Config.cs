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
        private const string ROLES_RESOURCE = "roles";
        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                /* disabled when we added hybrid grant type because this is no longer needed*/
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
                    AllowedGrantTypes = GrantTypes.Hybrid, //provides info about the flow - using the token when logging in with user credentials
                    RequirePkce = false, //this was set to false when changing from Code Grant Type to Hybrid Grand Type
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
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        IdentityServerConstants.StandardScopes.Email,
                        MOVIES_API,
                        ROLES_RESOURCE //custom resource we created
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
                new IdentityResources.Profile(),
                new IdentityResources.Address(),
                new IdentityResources.Email(),
                new IdentityResource( //because roles are not a built in part, we have to create it here
                    ROLES_RESOURCE,
                    "Your role(s)",
                    new List<string>() {"role"}
                    )
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
