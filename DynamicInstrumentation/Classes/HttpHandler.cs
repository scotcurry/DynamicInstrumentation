namespace DynamicInstrumentation;

public class HttpHandler
{
    public HttpHandler() { }

    public string GetJSONData() {

        var datadogAPIKey = Environment.GetEnvironmentVariable("DD_API_KEY");
        var datadogAPPKey = Environment.GetEnvironmentVariable("DD_APP_KEY");

        var getOrganizationURI = new UriBuilder("https", "api.datadoghq.com", 443, "api/v1/org");
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        httpClient.DefaultRequestHeaders.Add("DD-API-KEY", datadogAPIKey);
        httpClient.DefaultRequestHeaders.Add("DD-APPLICATION-KEY", datadogAPPKey);

        var responseBody = string.Empty;
        var response = httpClient.GetAsync(getOrganizationURI.Uri).Result;
        if (response.IsSuccessStatusCode) {
            responseBody = response.Content.ReadAsStringAsync().Result; 
        }

        return responseBody;
    }
}
