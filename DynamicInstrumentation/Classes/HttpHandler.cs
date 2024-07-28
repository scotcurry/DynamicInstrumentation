//using System.Text.Json;
using Newtonsoft.Json;

namespace DynamicInstrumentation;

public class HttpHandler
{
    public HttpHandler() { }

    public string GetServiceDefinitions() {

        var datadogApiKey = Environment.GetEnvironmentVariable("DD_API_KEY");
        var datadogAppKey = Environment.GetEnvironmentVariable("DD_APP_KEY");

        var responseBody = string.Empty;
        if (datadogApiKey != null && datadogAppKey != null) {
            var organizationUri = new UriBuilder("https", "api.datadoghq.com", 443, "api/v2/services/definitions");
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("DD-API-KEY", datadogApiKey);
            httpClient.DefaultRequestHeaders.Add("DD-APPLICATION-KEY", datadogAppKey);

            
            var response = httpClient.GetAsync(organizationUri.Uri).Result;
            if (response.IsSuccessStatusCode) {
                responseBody = response.Content.ReadAsStringAsync().Result; 
            }
        } else {
            var errorDict = new Dictionary<string,string>
            {
                { "error", "Datadog API or APP key missing" }
            };
            responseBody = JsonConvert.SerializeObject(errorDict);
        }

        responseBody = ParseServiceDefinitionJson(responseBody);
        return responseBody;
    }

    private static string ParseServiceDefinitionJson(string jsonString) {

        var serviceDictionary = new Dictionary<string, string>();
        var jsonContent = JsonConvert.DeserializeObject<ServiceDefinitionModel>(jsonString);
        var jsonData = jsonContent?.data;
        if (jsonData != null) {
            for (var counter = 0; counter < jsonData.Count; counter++) {
                if (jsonData[counter] != null) {
                    var serviceAttributes = jsonData[counter].attributes;
                    if (serviceAttributes != null) {
                        var attributeSchema = serviceAttributes.schema;
                        if (attributeSchema != null) {
                            var serviceName = attributeSchema.ddservice;
                            var serviceDescription = attributeSchema.description;
                            if (serviceName != null && serviceDescription != null) {
                                serviceDictionary.Add(serviceName, serviceDescription);
                            }
                        }
                    }
                }
            }
        }
        var dictionaryJson = JsonConvert.SerializeObject(serviceDictionary);
        return dictionaryJson;
    }
}
