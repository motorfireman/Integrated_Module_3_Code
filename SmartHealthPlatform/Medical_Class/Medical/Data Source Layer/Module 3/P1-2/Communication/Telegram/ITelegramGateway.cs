namespace Medical.Data_Source_Layer.Module_3.P1_2.Communication;

public interface ITelegramGateway
{
    public Task SendTelegram(int telegramId, string message);
}