using System.Collections.Generic;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace IdentityServer
{
    public class Config
    {
        private const string MOVIES_API = "moviesAPI";
        private const string MOVIES_DESCRIPTION = "Movies API";
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
                    AllowedScopes = {MOVIES_API} //what api are we protecting?
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

            };

        public static List<TestUser> TestUsers =>
            new List<TestUser>
            {

            };


    }
}
