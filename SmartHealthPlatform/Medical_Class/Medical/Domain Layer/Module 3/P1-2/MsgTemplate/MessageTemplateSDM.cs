using System.Collections;
using Medical.Data_Source_Layer.Module_3.P1_2.MsgTemplate;
using System.Text.RegularExpressions;
using Medical.Domain_Layer.DummyDeviceInterface;
using Microsoft.IdentityModel.Tokens;

namespace Medical.Domain_Layer.Module_3.P1_2.MsgTemplate;

public class MessageTemplateSDM : IEditMessageTemplate, IRetrieveMessageTemplate
{
    private readonly IMessageTemplateTDG _msgTdg;
    
    // Obtain device information from another module
    private readonly IDevice _device;

    public MessageTemplateSDM(IMessageTemplateTDG msgTdg, IDevice device)
    {
        _msgTdg = msgTdg;
        _device = device;
    }

    private String RegexReplace(String input)
    {
        input = input.Replace("\n","")
            //.Replace("<p>", "")
            //.Replace("</p>", "\n")
            .Replace("&nbsp;", " ");

        input = Regex.Replace(input, @"<input(.*?)device-data=""(.*?)""(.*?)>", "<$2>");
        input = Regex.Replace(input, @"<input(.*?)device-data='(.*?)'(.*?)>", "<$2>");

        return input;
    }

    private Boolean AreTemplateFieldsEmpty(MessageTemplate messageTemplate)
    {
        return (messageTemplate.Subject.IsNullOrEmpty() ||
                messageTemplate.Message.IsNullOrEmpty() ||
                messageTemplate.Name.IsNullOrEmpty());
    }
    public List<MessageTemplate> GetMessageTemplates(int userId)
    {
        var templates = _msgTdg.GetTemplatesByUserId(userId);
        foreach (var template in templates)
        {
            template.Message = RegexReplace(template.Message);
        }

        return templates;
    }
    
    // For Message Template Text Editor use only
    public List<MessageTemplate> GetMessageTemplatesForEditor(int userId)
    {

        return _msgTdg.GetTemplatesByUserId(userId);
    }

    public MessageTemplate GetMessageTemplate(int userId, int templateId)
    {
        var template = _msgTdg.GetTemplateByTemplateId(userId, templateId);
        template.Message = RegexReplace(template.Message);
        return template;
    }
    
    // For Message Template Text Editor use only
    public MessageTemplate GetMessageTemplateForEditor(int userId, int templateId)
    {
        return _msgTdg.GetTemplateByTemplateId(userId, templateId);
    }

    
    public void AddMessageTemplate(MessageTemplate messageTemplate)
    {
        // Code Contract has been deprecated :(
        //Contract.Requires<InvalidDataException>(!AreTemplateFieldsEmpty(messageTemplate),"Input fields cannot be empty");
        if (AreTemplateFieldsEmpty(messageTemplate))
        {
            throw new InvalidDataException("Input fields cannot be empty");
        }
        _msgTdg.Insert(messageTemplate);
    }

    public void EditMessageTemplate(MessageTemplate messageTemplate)
    {
        // Code Contract has been deprecated :( Using if -> throw instead
        if (AreTemplateFieldsEmpty(messageTemplate))
        {
            throw new InvalidDataException("Input fields cannot be empty");
        }
        _msgTdg.Update(messageTemplate);
    }

    public void DeleteMessageTemplate(int userId, int templateId)
    {
        _msgTdg.DeleteTemplate(userId, templateId);
        
    }

    public List<Device> GetAttributes(int userId)
    {
        return _device.getAllDevices(userId);
    }
    
    
}