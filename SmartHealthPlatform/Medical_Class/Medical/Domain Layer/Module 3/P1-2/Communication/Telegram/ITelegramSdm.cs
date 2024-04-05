namespace Medical.Domain_Layer.Module_3.P1_2.Communication.Telegram;

public interface ITelegramSdm
{
    public void SendTelegram(int userId, string subject, string message);
    public void ReplyTelegram(int telegramId, string message);
}