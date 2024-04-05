namespace Medical.Domain_Layer.Module_3.P1_2.MsgTemplate;

// Use this interface to retrieve message templates
public interface IRetrieveMessageTemplate
{
    public List<MessageTemplate> GetMessageTemplates(int userId);

    public MessageTemplate GetMessageTemplate(int userId, int templateId);
}