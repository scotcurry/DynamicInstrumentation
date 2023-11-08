using System.Text.Json;
using Microsoft.AspNetCore.Http.Features;

namespace DynamicInstrumentation;

public class HttpHandler
{
    public HttpHandler() { }

    public string GetJSONData() {

        var datadogAPIKey = Environment.GetEnvironmentVariable("DD_API_KEY");
        var datadogAPPKey = Environment.GetEnvironmentVariable("DD_APP_KEY");

        var responseBody = string.Empty;
        if (datadogAPIKey != null && datadogAPPKey != null) {
            var getOrganizationURI = new UriBuilder("https", "api.datadoghq.com", 443, "api/v1/org");
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("DD-API-KEY", datadogAPIKey);
            httpClient.DefaultRequestHeaders.Add("DD-APPLICATION-KEY", datadogAPPKey);

            
            var response = httpClient.GetAsync(getOrganizationURI.Uri).Result;
            if (response.IsSuccessStatusCode) {
                responseBody = response.Content.ReadAsStringAsync().Result; 
            }
        } else {
            var errorDict = new Dictionary<string,string>
            {
                { "error", "Datadog API or APP key missing" }
            };
            responseBody = JsonSerializer.Serialize(errorDict);
        }

        return responseBody;
    }
}
