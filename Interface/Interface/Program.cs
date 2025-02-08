using System.Text;
using System.Text.Json;

class Program
{
    static async Task Main(string[] args)
    {
        ValidateArgs(args);

        string filePath = args[1];
        string apiUrl = args[3];

        ValidateFile(filePath);

        string jsonContent = await File.ReadAllTextAsync(filePath);
        var vulnerabilities = JsonSerializer.Deserialize<DataModel>(jsonContent);

        if (vulnerabilities == null || vulnerabilities.Vulnerabilities.Count == 0)
        {
            Console.WriteLine("Error: No vulnerabilities found in the file.");
            return;
        }

        using HttpClient client = await ProcessVulnerabilities(apiUrl, vulnerabilities);
    }

    private static async Task<HttpClient> ProcessVulnerabilities(string apiUrl, DataModel? vulnerabilities)
    {
        HttpClient client = new HttpClient();
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

        return client;
    }

    private static void ValidateFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("Error: File not found.");
            return;
        }
    }

    private static void ValidateArgs(string[] args)
    {
        if (args.Length < 4 || args[0] != "--file" || args[2] != "--url")
        {
            Console.WriteLine("Usage: vulnerability-cli --file <JSON_FILE> --url <API_URL>");
            return;
        }
    }
}