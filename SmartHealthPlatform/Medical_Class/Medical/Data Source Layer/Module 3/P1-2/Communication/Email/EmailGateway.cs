using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Medical.Data_Source_Layer.Module_3.P1_2.Communication.Email;

public class EmailGateway: IEmailGateway
{
    private readonly IConfiguration _configuration;

    public EmailGateway(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public void SendEmail(string address, string subject, string message)
    {
        var settings = _configuration.GetSection("EmailSettings").Get<EmailSettings>()!;
        
        // Build message
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("Smart Health Platform", "alert@smarthealth.spmovy.com"));
        emailMessage.To.Add(new MailboxAddress(address, address));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message };
        
        // Send message
        try
        {
            var client = new SmtpClient();
            client.Connect(settings.SmtpServer, settings.Port, SecureSocketOptions.StartTls);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            client.Authenticate(settings.UserName, settings.Password);
            client.Send(emailMessage);
        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"Email send error {e.Message}");
        }
        
        Console.WriteLine("Email sent");


    }
}