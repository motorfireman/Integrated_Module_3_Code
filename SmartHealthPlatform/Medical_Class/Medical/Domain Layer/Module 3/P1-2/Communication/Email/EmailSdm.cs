using Medical.Data_Source_Layer.Module_3.P1_2.Communication;
using Medical.Domain_Layer.Module_3.Mock;

namespace Medical.Domain_Layer.Module_3.P1_2.Communication.Email;

public class EmailSdm: IEmailSdm
{
    private readonly IUserData _userData;
    private readonly IEmailGateway _emailGateway;

    public EmailSdm(IUserData userData, IEmailGateway emailGateway)
    {
        _userData = userData;
        _emailGateway = emailGateway;
    }
    
    public void SendEmail(int userId, string subject, string message)
    {
        var user = _userData.GetUser(userId);

        if (user == null)
        {
            Console.Error.WriteLine($"User {userId} does not exist");
            return;
        }
        
        _emailGateway.SendEmail(user.Email, subject, message);
    }
}