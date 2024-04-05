//using System.Security.Cryptography.X509Certificates;
using Medical.Domain_Layer.Module_3.P1_2.MsgTemplate;
using Microsoft.EntityFrameworkCore;
using SmartHealthPlatformContext = Medical.Data_Source_Layer.SmartHealthPlatformContext;

namespace Medical.Data_Source_Layer.Module_3.P1_2.MsgTemplate;

public class MessageTemplateTDG : IMessageTemplateTDG
{
    private readonly SmartHealthPlatformContext _db;

    public MessageTemplateTDG(SmartHealthPlatformContext db)
    {
        _db = db;
    }
    
    public void Insert(MessageTemplate messageTemplate)
    {
        try
        {
            _db.MessageTemplates.Add(messageTemplate);
            _db.SaveChanges();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error occurred while inserting message template: {e.Message}");
            throw;
        }

    }

    public void Update(MessageTemplate messageTemplate)
    {
        try
        {
            // For validation purposes, check for UserId first before calling Update
            _db.MessageTemplates
                .Where(template => template.Id == messageTemplate.Id 
                                   && template.UserId == messageTemplate.UserId)
                .ExecuteUpdate(property =>
                    property.SetProperty(template => template.Subject, messageTemplate.Subject)
                        .SetProperty(template => template.Message, messageTemplate.Message)
                        .SetProperty(template => template.Name, messageTemplate.Name)
                    );
            _db.SaveChanges();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error occurred while updating message template: {e.Message}");
            throw;
        }
    }

    public List<MessageTemplate> GetTemplatesByUserId(int userId)
    {
        try
        {
            List<MessageTemplate> messageTemplates = _db.MessageTemplates
                .Where(messageTemplate =>  messageTemplate.UserId == userId)
                .ToList();
            return messageTemplates;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error occurred while retrieving message template: {e.Message}");
            throw;
        }
    }
    
    public MessageTemplate GetTemplateByTemplateId(int userId, int templateId)
    {
        try
        {
            // UserID check has been commented out as module does not support login/account system
            var messageTemplate = _db.MessageTemplates
                .FirstOrDefault(messageTemplate => messageTemplate.Id == templateId
                                                   && messageTemplate.UserId == userId);
            
            return messageTemplate;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error occurred while retrieving message template: {e.Message}");
            throw;
        }
    }

    public void DeleteTemplate(int userId, int templateId)
    {
        try
        {
            _db.MessageTemplates
                .Where(messageTemplate => 
                    messageTemplate.Id == templateId
                    && messageTemplate.UserId == userId)
                .ExecuteDelete();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error occurred while deleting message template: {e.Message}");
            throw;
        }
    }
}