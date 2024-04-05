using System.Net.Mime;
using System.Text;
using System.Text.Json.Nodes;

namespace Medical.Data_Source_Layer.Module_3.P1_2.Communication.Telegram;

public class TelegramReceiver: BackgroundService
{
    private readonly IConfiguration _configuration;

    public TelegramReceiver(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var offset = 0;
        while (!stoppingToken.IsCancellationRequested)
        {
            // Get list of messages from Telegram
            var result = await new TelegramRequestBuilder()
                .AddApiKey(_configuration.GetValue<string>("TelegramBotApiKey") ?? "")
                .AddEndpointMethod("getUpdates")
                .AddParameter("offset", offset.ToString())
                .Build();
            
            // Error occured, skip iteration
            if (result["error"] != null || (!result["ok"]?.GetValue<bool>() ?? true))
            {
                Console.WriteLine(result.ToJsonString());
                await Task.Delay(1000, stoppingToken);
                continue;
            }
            
            // Process updates
            var updates = result["result"]?.AsArray() ?? new JsonArray();
            foreach (var update in updates)
            {
                // Forward update to controller
                var url = _configuration.GetValue<string>("ApplicationUrl") + "/Telegram/ReceiveMessage";
                var content = new StringContent(update!.ToJsonString(), Encoding.UTF8, MediaTypeNames.Application.Json);
                new HttpClient().PostAsync(url, content); // Not awaited to process multiple updates concurrently
                
                // Update offset to prevent fetching repeated updates
                offset = update["update_id"]!.GetValue<int>() + 1;
            }
            
            //Console.WriteLine(result.ToJsonString());
            await Task.Delay(1000, stoppingToken);
        }
    }
}