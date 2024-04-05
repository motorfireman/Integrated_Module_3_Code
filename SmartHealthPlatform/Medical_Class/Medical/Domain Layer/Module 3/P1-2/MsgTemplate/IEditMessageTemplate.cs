using Medical.Domain_Layer.DummyDeviceInterface;

namespace Medical.Domain_Layer.Module_3.P1_2.MsgTemplate;

// If you're wondering what this interface is, it means this isn't for you
// Use IRetrieveMessageTemplate to retrieve message templates instead.

// This interface is meant for CRUD operations of MessageTemplate, and not for data retrieval.
// The Get methods here will return data formatted specifically for the frontend Text Editor,
// and is not meant for any purpose outside of Text Editor
public interface IEditMessageTemplate
{
    public MessageTemplate GetMessageTemplateForEditor(int userId, int templateId);
    public List<MessageTemplate> GetMessageTemplatesForEditor(int userId);
    public void AddMessageTemplate(MessageTemplate messageTemplate);
    public void EditMessageTemplate(MessageTemplate messageTemplate);
    public void DeleteMessageTemplate(int userId, int templateId);
    
    // Retrieves data from IDevice provided by another module
    // The IDevice interface requires userId for validation
    public List<Device> GetAttributes(int userId);
}