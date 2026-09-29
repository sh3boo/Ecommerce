using Duende.IdentityServer.Models;

namespace eShop.Identity;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("Catalogapi"),
            new ApiScope("basketapi"),
            new ApiScope("Catalogapi.read"),
            new ApiScope("Catalogapi.write"),
            new ApiScope("eshoppinggateway")
        };
    public static IEnumerable<ApiResource> ApiResource =>
        new ApiResource[]
        {
            new ApiResource("Catalog","Catalogapi.Api")
            {
                Scopes = { "Catalogapi.read", "Catalogapi.write" }
            },
            new ApiResource("Basket","Basketapi.Api")
            {
                Scopes = { "basketapi" }
            },
            new ApiResource("EshoppingGateway","Eshopping Gateway")
            {
                Scopes = { "eshoppinggateway", "basketapi" }
            },
            //new ApiScope("scope2"),
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            // m2m client credentials flow client
            new Client
            {
                ClientId = "m2m.client",
                ClientName = "Client Credentials Client",

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

                AllowedScopes = { "scope1" }
            },

            // interactive client using code flow + pkce
            new Client
            {
                ClientId = "interactive",
                ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },
                    
                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { "https://localhost:44300/signin-oidc" },
                FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
                PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },

                AllowOfflineAccess = true,
                AllowedScopes = { "openid", "profile", "scope2" }
            },

            // interactive client using code flow + pkce
            new Client
            {
                ClientName = "Catalog API Client",
                ClientId = "CatalogAPIClient",
                ClientSecrets = { new Secret("49C1A7E1-0C99-4V89-A5D6-A37998FB86B0".Sha256()) },
                    
                AllowedGrantTypes = GrantTypes.ClientCredentials,

                //RedirectUris = { "https://localhost:44300/signin-oidc" },
                //FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
                //PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },

                //AllowOfflineAccess = true,
                AllowedScopes = { "Catalogapi.read", "Catalogapi.write" }
            },
            new Client
            {
                ClientName = "Basket API Client",
                ClientId = "BasketAPIClient",
                ClientSecrets = { new Secret("49C1A7E1-0C99-4V89-A5S9-A37998FB86B0".Sha256()) },
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = {  "basketapi" }
            },
            new Client
            {
                ClientName = "Eshopping Gateway Client",
                ClientId = "EshoppingGatewayClient",
                ClientSecrets = { new Secret("49C1A7E1-0C99-4V89-A5S9-A34998FV86B0".Sha256()) },
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = { "eshoppinggateway", "basketapi" }
            },
        };
}
