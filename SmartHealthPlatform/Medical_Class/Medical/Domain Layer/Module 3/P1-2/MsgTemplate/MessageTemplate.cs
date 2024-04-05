using System.ComponentModel.DataAnnotations;

namespace Medical.Domain_Layer.Module_3.P1_2.MsgTemplate;

//TODO: DeletedTime to support undo operation
public class MessageTemplate
{
    public MessageTemplate()
    {
        
    }
    public MessageTemplate(int userId, string name, string subject, string message)
    {
        Name = name;
        UserId = userId;
        Subject = subject;
        Message = message;
    }
    // Id of Message Template
    [Key]
    public int Id { get; set; }
    
    // Id of User that this template is used by
    public int UserId { get; set; }
    
    public string Name { get; set; }
    
    public string Subject { get; set; }
    
    public string Message { get; set; }

}