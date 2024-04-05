using Medical.Data_Source_Layer.Module_3.P1_2.Communication;

namespace Medical.Domain_Layer.Module_3.P1_2.Communication.Telegram;

public class TelegramSdm: ITelegramSdm
{
    private readonly ITelegramRegistrationTdg _telegramRegistrationTdg;
    private readonly ITelegramGateway _telegramGateway;

    public TelegramSdm(ITelegramRegistrationTdg telegramRegistrationTdg, ITelegramGateway telegramGateway)
    {
        _telegramRegistrationTdg = telegramRegistrationTdg;
        _telegramGateway = telegramGateway;
    }
    
    public void SendTelegram(int userId, string subject, string message)
    {
        // Get user telegram id
        var telegramId = _telegramRegistrationTdg.GetByUserId(userId)?.TelegramId;

        // Telegram id not found
        if (telegramId == null)
        {
            Console.Error.WriteLine($"User {userId} does not have a registered Telegram account.");
            return;
        }
        
        // Build and send message
        message = message
            .Replace("<p>", "")
            .Replace("</p>", "\n");
        
        var telegramMessage = (subject != "" ? $"<u><b>{subject}</b></u>\n\n" : "") + message;
        _telegramGateway.SendTelegram((int)telegramId, telegramMessage);
    }
    
    // For direct reply
    public void ReplyTelegram(int telegramId, string message)
    {
        _telegramGateway.SendTelegram((int)telegramId, message);
    }
}