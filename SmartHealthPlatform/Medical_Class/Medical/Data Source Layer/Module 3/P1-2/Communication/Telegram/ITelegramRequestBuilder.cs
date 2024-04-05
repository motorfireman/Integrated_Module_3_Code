using System.Text.Json.Nodes;

namespace Medical.Data_Source_Layer.Module_3.P1_2.Communication;

public interface ITelegramRequestBuilder
{
    public ITelegramRequestBuilder AddApiKey(string apiKey);
    public ITelegramRequestBuilder AddEndpointMethod(string methodName);
    public ITelegramRequestBuilder AddParameter(string key, string value);
    public Task<JsonNode> Build();
}