using System.Text.Json;
using System.Threading.Tasks;
using Altinn.TestClient.Models;
using Flurl;
using Flurl.Http;

namespace Altinn.TestClient.Handlers
{
    public class ReporteeHandler 
    {
        private readonly string _apiKey;

        public ReporteeHandler(string apiKey)
        {
            _apiKey = apiKey;
        }

        public async Task<Reportee> GetReportee(string altinnToken, string ssn, string  lastName)
        {
            try
            {
                var response = await "https://tt02.altinn.no"
                    .AppendPathSegment("api/reportees/ReporteeConversion")
                    .WithOAuthBearerToken(altinnToken)
                    .WithHeader("ApiKey", _apiKey)
                    .WithHeader("Accept", "application/json")
                    .PostJsonAsync(new {
                        SocialSecurityNumber = ssn,
                        LastName = lastName 
                    })
                    .ReceiveString();

                Reportee reportee = JsonSerializer.Deserialize<Reportee>(response);
                return reportee;
            }
            catch (FlurlHttpException ex)
            {
                var status = ex.Call.Response.StatusCode;
                var message = await ex.GetResponseStringAsync();
            }
            return null;
        }
    }
}