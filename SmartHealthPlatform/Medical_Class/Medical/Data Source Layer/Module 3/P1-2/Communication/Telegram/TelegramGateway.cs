using System.Text.Encodings.Web;
using System.Text.Json;

namespace Medical.Data_Source_Layer.Module_3.P1_2.Communication;

public class TelegramGateway: ITelegramGateway
{
    private readonly IConfiguration _configuration;

    public TelegramGateway(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendTelegram(int telegramId, string message)
    {
        // Telegram requires . to be escaped
        // message = message.Replace(".", "\\.");
        
        var response = await new TelegramRequestBuilder()
            .AddApiKey(_configuration.GetValue<string>("TelegramBotApiKey") ?? "")
            .AddEndpointMethod("sendMessage")
            .AddParameter("chat_id", telegramId.ToString())
            .AddParameter("text", message)
            .AddParameter("parse_mode", "HTML")
            .Build();
        Console.WriteLine(response.ToJsonString(new JsonSerializerOptions {Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping}));
    }
    
}