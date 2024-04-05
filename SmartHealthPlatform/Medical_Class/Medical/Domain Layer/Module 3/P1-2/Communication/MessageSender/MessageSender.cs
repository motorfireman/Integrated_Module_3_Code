using Medical.Domain_Layer.Module_3.P1_2.Communication.Telegram;

namespace Medical.Domain_Layer.Module_3.P1_2.Communication.MessageSender;

public class MessageSender: IMessageSender
{
    private readonly IEmailSdm _emailSdm;
    private readonly ITelegramSdm _telegramSdm;

    public MessageSender(IEmailSdm emailSdm, ITelegramSdm telegramSdm)
    {
        _emailSdm = emailSdm;
        _telegramSdm = telegramSdm;
    }
    
    public void Send(int userId, string subject, string message, params MessageType[] messageTypes)
    {
        foreach (var type in messageTypes)
        {
            switch (type)
            {
                case MessageType.Email:
                    _emailSdm.SendEmail(userId, subject, message);
                    break;
                case MessageType.Telegram:
                    _telegramSdm.SendTelegram(userId, subject, message);
                    break;
            }
        }
    }
}