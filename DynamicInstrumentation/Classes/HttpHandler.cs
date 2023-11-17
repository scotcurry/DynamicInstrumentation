//using System.Text.Json;
using Newtonsoft.Json;

namespace DynamicInstrumentation;

public class HttpHandler
{
    public HttpHandler() { }

    public string GetServiceDefinitions() {

        var datadogAPIKey = Environment.GetEnvironmentVariable("DD_API_KEY");
        var datadogAPPKey = Environment.GetEnvironmentVariable("DD_APP_KEY");

        var responseBody = string.Empty;
        if (datadogAPIKey != null && datadogAPPKey != null) {
            var getOrganizationURI = new UriBuilder("https", "api.datadoghq.com", 443, "api/v2/services/definitions");
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
            responseBody = JsonConvert.SerializeObject(errorDict);
        }

        responseBody = ParseServiceDefinitionJSON(responseBody);
        return responseBody;
    }

    static private string ParseServiceDefinitionJSON(string jsonString) {

        var serviceDictionary = new Dictionary<string, string>();
        var jsonContent = JsonConvert.DeserializeObject<ServiceDefinitionModel>(jsonString);
        var jsonData = jsonContent?.data;
        if (jsonData != null) {
            for (int counter = 0; counter < jsonData.Count; counter++) {
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
        var dictionaryJSON = JsonConvert.SerializeObject(serviceDictionary);
        return dictionaryJSON;
    }
}
