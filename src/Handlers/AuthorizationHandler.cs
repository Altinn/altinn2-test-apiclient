using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Altinn.TestClient.Converters;
using Altinn.TestClient.Models;
using Altinn.TestClient.Testdata;
using Flurl;
using Flurl.Http;

namespace Altinn.TestClient.Handlers
{
    public class AuthorizationHandler
    {
        private readonly string _apiKey;

        public AuthorizationHandler(string apiKey)
        {
            _apiKey = apiKey;
        }

        public async Task<RightHolder> GetDelegations(string altinnToken, string reporteeId)
        {
            try
            {
                var response = await "https://tt02.altinn.no"
                    .AppendPathSegment($"api/my/authorization/Delegations/{reporteeId}")
                    .SetQueryParam("language", 1044)
                    .WithOAuthBearerToken(altinnToken)
                    .WithHeader("ApiKey", _apiKey)
                    .WithHeader("Accept", "application/hal+json")
                    .GetStringAsync();

                var deserializeOptions = new JsonSerializerOptions();
                deserializeOptions.Converters.Add(new EmbeddedListConverter());

                RightHolder rightHolder = JsonSerializer.Deserialize<RightHolder>(response, deserializeOptions);
                return rightHolder;
            }
            catch (FlurlHttpException ex)
            {
                var status = ex.Call.Response.StatusCode;
                var message = await ex.GetResponseStringAsync();
            }
            return null;
        }

        // TODO Pass a collection of rights to this method instead of a testdata service.
        public async Task<string> PostDelegations(string altinnToken, string ssn, string lastName, Service service)
        {
            NewDelegations rightsToDelegate = new NewDelegations { Rights = new List<Right>() };
            rightsToDelegate.Rights.Add(new Right
            {
                ServiceCode = service.ServiceCode,
                ServiceEditionCode = service.ServiceEditionCode,
                Action = "Read"
            });
            rightsToDelegate.Rights.Add(new Right
            {
                ServiceCode = service.ServiceCode,
                ServiceEditionCode = service.ServiceEditionCode,
                Action = "Write"
            });
            RightHolder delegations = new RightHolder {
                SocialSecurityNumber = ssn,
                LastName = lastName,
                NewDelegations = rightsToDelegate
            };

            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new EmbeddedListConverter());
            string rightHolderString = JsonSerializer.Serialize<RightHolder>(delegations, serializeOptions);

            try
            {
                var response = await "https://tt02.altinn.no"
                    .AppendPathSegment("api/my/authorization/Delegations/")
                    .WithOAuthBearerToken(altinnToken)
                    .WithHeader("ApiKey", _apiKey)
                    .WithHeader("Content-Type", "application/hal+json")
                    .PostStringAsync(
                        rightHolderString
                    )
                    .ReceiveString();

                return response;
            }
            catch (FlurlHttpException ex)
            {
                var status = ex.Call.Response.StatusCode;
                var message = await ex.GetResponseStringAsync();
            }
            return null;
        }

        public async Task DeleteDelegatedRights(string altinnToken, string reporteeId, int authorizationRuleId)
        {
            try
            {
                await "https://tt02.altinn.no"
                    .AppendPathSegment($"api/my/authorization/Delegations/{reporteeId}/rights/{authorizationRuleId}")
                    .WithOAuthBearerToken(altinnToken)
                    .WithHeader("ApiKey", _apiKey)
                    .DeleteAsync();
            }
            catch (FlurlHttpException ex)
            {
                var status = ex.Call.Response.StatusCode;
                var message = await ex.GetResponseStringAsync();
            }
        }
    }
}