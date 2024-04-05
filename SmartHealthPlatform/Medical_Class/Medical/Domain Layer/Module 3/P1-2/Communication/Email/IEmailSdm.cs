namespace Medical.Domain_Layer.Module_3.P1_2.Communication;

public interface IEmailSdm
{
    public void SendEmail(int userId, string subject, string message);
}