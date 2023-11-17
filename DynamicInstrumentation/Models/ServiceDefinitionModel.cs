using Newtonsoft.Json;

public class ServiceDefinitionModel
    {
        public List<DataDefinition>? data { get; set; }
    }

    public class DataDefinition
    {
        public string? type { get; set; }
        public string? id { get; set; }
        public Attributes? attributes { get; set; }
    }

    public class Attributes
    {
        public Meta? meta { get; set; }
        public Schema? schema { get; set; }
    }

    public class Schema
    {
        public string? schemaversion { get; set; }
        [JsonProperty("dd-service")]
        public string? ddservice { get; set; }
        public string? team { get; set; }
        public string? application { get; set; }
        public string? tier { get; set; }
        public string? description { get; set; }
        public string? lifecycle { get; set; }
        public List<Contact>? contacts { get; set; }
        public List<Link>? links { get; set; }
        public List<string>? tags { get; set; }
        public List<string>? cipipelinefingerprints { get; set; }
        public string? type { get; set; }
        public List<string>? languages { get; set; }
    }

    public class Contact
    {
        public string? name { get; set; }
        public string? type { get; set; }
        public string? contact { get; set; }
    }

    public class Link
    {
        public string? name { get; set; }
        public string? type { get; set; }
        public string? provider { get; set; }
        public string? url { get; set; }
    }

    public class Meta
    {
        public DateTime? lastmodifiedtime { get; set; }
        public string? githubhtmlurl { get; set; }
        public string? ingestionsource { get; set; }
        public string? origin { get; set; }
        public string? origindetail { get; set; }
        public List<object>? warnings { get; set; }
        public string? ingestedschemaversion { get; set; }
    }