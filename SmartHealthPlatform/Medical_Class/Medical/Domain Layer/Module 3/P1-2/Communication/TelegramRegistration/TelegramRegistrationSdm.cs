using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using Medical.Data_Source_Layer.Module_3.P1_2.Communication;
using Medical.Domain_Layer.Module_3.Mock;
using Medical.Domain_Layer.Module_3.P1_2.Communication.Telegram;

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

namespace Medical.Domain_Layer.Module_3.P1_2.Communication.TelegramRegistration;

public class TelegramRegistrationSdm: ITelegramRegistrationSdm
{
    private readonly IUserData _userData;
    private readonly ITelegramSdm _telegramSdm;
    private readonly ITelegramRegistrationTdg _telegramRegistrationTdg;

    public TelegramRegistrationSdm(IUserData userData, ITelegramSdm telegramSdm, ITelegramRegistrationTdg telegramRegistrationTdg)
    {
        _userData = userData;
        _telegramSdm = telegramSdm;
        _telegramRegistrationTdg = telegramRegistrationTdg;
    }
    
    public Dictionary<string, string> GetRegistrationStatus()
    {
        var user = _userData.GetCurrentUser();

        // User not logged in
        if (user == null)
        {
            return new Dictionary<string, string>
            {
                { "status", "unauthorised" }
            };
        }
        
        // Get registration details
        var registration = _telegramRegistrationTdg.GetByUserId(user.Id);
        var registrationStatus = "Registered";

        // Check if telegram id exists
        if (registration?.TelegramId == null)
            registrationStatus = "Not registered";
        
        return new Dictionary<string, string>
        {
            { "status", "success" },
            { "registrationStatus", registrationStatus }
        };
    }

    public Dictionary<string, string> GenerateToken()
    {
        var user = _userData.GetCurrentUser();

        // User is not logged in
        if (user == null)
        {
            return new Dictionary<string, string>
            {
                { "status", "unauthorised" }
            };
        }
        
        // Get existing registration
        var registration = _telegramRegistrationTdg.GetByUserId(user.Id);

        // Telegram account already registered
        if (registration != null && registration.TelegramId != null)
        {
            return new Dictionary<string, string>
            {
                { "status", "error" },
                { "message", "An existing Telegram account is already registered." }
            };
        }

        // Generate new token
        var token = Guid.NewGuid().ToString();
        
        // No registration, create new
        if (registration == null)
        {
            _telegramRegistrationTdg.Insert(user.Id, token);
        }
        else // Update token
        {
            _telegramRegistrationTdg.UpdateTokenById(registration.Id, token);
        }
        
        return new Dictionary<string, string>
        {
            { "status", "success" },
            { "token", token }
        };
    }

    public Dictionary<string, string> DeRegisterAccount()
    {
        var user = _userData.GetCurrentUser();

        // User is not logged in
        if (user == null)
        {
            return new Dictionary<string, string>
            {
                { "status", "unauthorised" }
            };
        }
        
        // Delete registration
        _telegramRegistrationTdg.DeleteByUserId(user.Id);

        return new Dictionary<string, string>
        {
            { "status", "success" }
        };
    }
    
    public void ReceiveMessage(JsonNode update)
    {
        var telegramId = update["message"]?["from"]?["id"]?.GetValue<int>() ?? 0;
        var message = update["message"]?["text"]?.GetValue<string>() ?? "";

        // Invalid update format, probably not sent by Telegram
        if (telegramId == 0 || message == "")
            return;
        
        // Token registration
        if (Regex.Match(message, @"^[\da-f]{8}\-[\da-f]{4}\-4[\da-f]{3}\-[89ab][\da-f]{3}\-[\da-f]{12}$").Success)
        {
            var replayMessage = RegisterAccount(message, telegramId);
            _telegramSdm.ReplyTelegram(telegramId, replayMessage);
            return;
        }
        
        _telegramSdm.ReplyTelegram(telegramId, "Invalid action. For account registration, please visit the Smart Health Platform web portal.");
    }
    
    private string RegisterAccount(string token, int telegramId)
    {
        // Get registration by token
        var registration = _telegramRegistrationTdg.GetByToken(token);

        // No registration or expired
        if (registration == null || registration.TokenGenerated < DateTime.Now.AddMinutes(-15))
            return "Token is invalid or may have expired. Please generate a new token from the Smart Health Platform web portal.";
        
        // Another Telegram account already registered
        if (registration.TelegramId != null && registration.TelegramId != telegramId)
            return "Another Telegram account has already been registered. If you wish to change the account, please de-register the existing account from the Smart Health Platform web portal.";
        
        // Update token
        _telegramRegistrationTdg.UpdateTelegramIdByToken(token, telegramId);
        return "Your account has been linked successfully.";
    }
}