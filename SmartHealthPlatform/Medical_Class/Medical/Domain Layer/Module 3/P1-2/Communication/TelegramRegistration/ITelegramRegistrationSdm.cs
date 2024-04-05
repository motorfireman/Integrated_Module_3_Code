using System.Text.Json.Nodes;

namespace Medical.Domain_Layer.Module_3.P1_2.Communication.TelegramRegistration;

public interface ITelegramRegistrationSdm
{
    public Dictionary<string, string> GetRegistrationStatus();
    public Dictionary<string, string> GenerateToken();
    public Dictionary<string, string> DeRegisterAccount();
    public void ReceiveMessage(JsonNode update);
}