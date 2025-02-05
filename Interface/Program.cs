using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

class Program
{
    static async Task Main(string[] args)
    {
        if (args.Length < 4 || args[0] != "--file" || args[2] != "--url")
        {
            Console.WriteLine("Usage: vulnerability-cli --file <JSON_FILE> --url <API_URL>");
            return;
        }

        string filePath = args[1];
        string apiUrl = args[3];

        if (!File.Exists(filePath))
        {
            Console.WriteLine("Error: File not found.");
            return;
        }

        string jsonContent = await File.ReadAllTextAsync(filePath);
        var vulnerabilities = JsonSerializer.Deserialize<DataModel>(jsonContent);

        if (vulnerabilities == null || vulnerabilities.Vulnerabilities.Count == 0)
        {
            Console.WriteLine("Error: No vulnerabilities found in the file.");
            return;
        }

        using HttpClient client = new HttpClient();
        foreach (var vuln in vulnerabilities.Vulnerabilities)
        {
            try
            {
                string json = JsonSerializer.Serialize(vuln);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl + "/vulnerability", content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Uploaded: {vuln.Cve}");
                }
                else
                {
                    Console.WriteLine($"Failed to upload {vuln.Cve}: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading {vuln.Cve}: {ex.Message}");
            }
        }
    }
}

public class DataModel
{
    [JsonPropertyName("vulnerabilities")]
    public List<Vulnerability> Vulnerabilities { get; set; } = new();
}

public class Vulnerability
{
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("cve")]
    public string Cve { get; set; }

    [JsonPropertyName("criticality")]
    public int Criticality { get; set; }
}

