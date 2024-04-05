namespace Medical.Domain_Layer.Module_3.P1_2.Communication;

public interface IMessageSender
{
    public void Send(int userId, string subject, string message, params MessageType[] messageTypes);
}