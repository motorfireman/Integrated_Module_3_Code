using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Medical.Data_Source_Layer.Module_3.P1_2.Communication;

public class TelegramRequestBuilder: ITelegramRequestBuilder
{
    private string? ApiKey { get; set; }
    private string? EndpointMethod { get; set; }
    private Dictionary<string, string> Parameters { get; set; } = new();

    public ITelegramRequestBuilder AddApiKey(string apiKey)
    {
        ApiKey = apiKey;
        return this;
    }
    
    public ITelegramRequestBuilder AddEndpointMethod(string methodName)
    {
        EndpointMethod = methodName;
        return this;
    }

    public ITelegramRequestBuilder AddParameter(string key, string value)
    {
        Parameters.Add(key, value);
        return this;
    }

    public async Task<JsonNode> Build()
    {
        try
        {
            var url = $"https://api.telegram.org/bot{ApiKey}/{EndpointMethod}";
            var content = new StringContent(JsonSerializer.Serialize(Parameters), Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await new HttpClient().PostAsync(url, content);
            return JsonNode.Parse(await response.Content.ReadAsStringAsync())!;
        }
        catch (HttpRequestException exception)
        {
            Console.WriteLine($"An error has occurred while sending a http request: {exception.Message}");
            return new JsonObject {{"error", exception.Message}};
        }
    }


}