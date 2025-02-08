using System.Text.Json.Serialization;

public class DataModel
{
    [JsonPropertyName("vulnerabilities")]
    public List<Vulnerability> Vulnerabilities { get; set; } = new();
}

