using Medical.Domain_Layer.Module_3.P1_2.MsgTemplate;

namespace Medical.Data_Source_Layer.Module_3.P1_2.MsgTemplate;

public interface IMessageTemplateTDG
{
    public void Insert(MessageTemplate messageTemplate);
    
    public List<MessageTemplate> GetTemplatesByUserId(int userId);
    
    public MessageTemplate GetTemplateByTemplateId(int userId, int templateId);

    public void DeleteTemplate(int userId, int templateId);

    public void Update(MessageTemplate messageTemplate);

}