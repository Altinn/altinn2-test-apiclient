
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading.Tasks;
using Altinn.TestClient.Models;
using Flurl;
using Flurl.Http;
using Microsoft.IdentityModel.Tokens;

namespace Altinn.TestClient.Handlers
{
    public class AuthenticationHandler
    {
        private readonly string _username;
        private readonly string _password;
        private readonly string _certificateThumbprint;

        public AuthenticationHandler(string username, string password, string certificateThumbprint)
        {
            _username = username;
            _password = password;
            _certificateThumbprint = certificateThumbprint;
        }

        public async Task<TokenResponse> PostMaskinportenToken()
        {
            try
            {
                X509Certificate2 certificate = GetCertificateFromX509Store();
                string jwtAssertion = GetJwtAssertion(certificate);

                string response = await "https://ver2.maskinporten.no/"
                    .AppendPathSegment("token")
                    .PostUrlEncodedAsync(new 
                    {
                        grant_type = "urn:ietf:params:oauth:grant-type:jwt-bearer",
                        assertion = jwtAssertion
                    })
                    .ReceiveString();
                
                TokenResponse maskinportenToken = JsonSerializer.Deserialize<TokenResponse>(response);
                return maskinportenToken;
            }
            catch (FlurlHttpException ex)
            {
                var status = ex.Call.Response.StatusCode;
                var message = await ex.GetResponseStringAsync();
                // TODO Logger
            }

            return null;
        }

        public async Task<string> GetAltinnExchangeToken(string maskinportenToken)
        {
            try
            {
                var basicAuthEncodedBytes = System.Text.Encoding.UTF8.GetBytes($"{_username}:{_password}");
                string basicAuthEncodedString = System.Convert.ToBase64String(basicAuthEncodedBytes);

                string response = await "https://platform.tt02.altinn.no"
                    .AppendPathSegment("authentication/api/v1/exchange/maskinporten")
                    .WithOAuthBearerToken(maskinportenToken)
                    .WithHeader("X-Altinn-EnterpriseUser-Authentication", basicAuthEncodedString)
                    .WithHeader("Accept", "application/hal+json")
                    .GetStringAsync();
                
                return response;
            }
            catch (FlurlHttpException ex)
            {
                var status = ex.Call.Response.StatusCode;
                var message = await ex.GetResponseStringAsync();
            }

            return null;
        }

        private X509Certificate2 GetCertificateFromX509Store()
        {
            StoreName storeName = StoreName.My;
            StoreLocation storeLocation = StoreLocation.LocalMachine;
            X509Store store = new X509Store(storeName, storeLocation);

            store.Open(OpenFlags.ReadOnly);

            X509Certificate2Collection certCollection = store.Certificates.Find(X509FindType.FindByThumbprint, _certificateThumbprint, validOnly: false);
            X509Certificate2Enumerator enumerator = certCollection.GetEnumerator();
            X509Certificate2 certificate = null;
            while (enumerator.MoveNext())
            {
                certificate = enumerator.Current;
            }

            if (certificate == null)
            {
                throw new ArgumentException("Unable to find certificate in store with thumbprint: " + _certificateThumbprint + ". Check your config, and make sure the certificate is installed in the \"LocalMachine\\My\" store.");
            }

            return certificate;
        }

        private static string GetJwtAssertion(X509Certificate2 certificate)
        {
            X509SecurityKey securityKey = new X509SecurityKey(certificate);
            List<string> certificateChain = new List<string>() { Convert.ToBase64String(certificate.GetRawCertData()) };

            // The header of the JWT grant towards Maskinporten must have the following claims:
            // "alg": "RS256"
            // "x5c": "<X.509 Certificate chain>"
            JwtHeader header = new JwtHeader(new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha256))
            {
                { "x5c", certificateChain }
            };

            // Remove claims that are not needed for Enterprise Certificate grants.
            header.Remove("typ");
            header.Remove("kid");

            // 120 seconds is the maximum expiration time allowed by Maskinporten.
            // This value is chosen to make debugging more lenient.
            long issuedAt = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            long expirationTime = issuedAt + 120;
            string clientId = "d395043f-19de-48f7-9ea7-9e38c96b5472";

            JwtPayload body = new JwtPayload
            {
                { "aud", "https://ver2.maskinporten.no/" },
                { "resource", "https://tt02.altinn.no" }, 
                { "scope", "altinn:enduser" },
                { "iss", clientId },
                { "exp", expirationTime },
                { "iat", issuedAt },
                { "jti", Guid.NewGuid().ToString() },
            };

            JwtSecurityToken securityToken = new JwtSecurityToken(header, body);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.WriteToken(securityToken);

        }
    }
}