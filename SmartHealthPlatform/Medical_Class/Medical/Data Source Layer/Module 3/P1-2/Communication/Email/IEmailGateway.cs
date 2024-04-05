namespace Medical.Data_Source_Layer.Module_3.P1_2.Communication;

public interface IEmailGateway
{
    public void SendEmail(string address, string subject, string message);
}