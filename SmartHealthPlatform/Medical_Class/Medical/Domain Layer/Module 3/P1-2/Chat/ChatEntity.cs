using Medical.Data_Source_Layer.Module_3.P1_2.Chat;
using System.ComponentModel.DataAnnotations;

namespace Medical.Domain_Layer.Module_3.P1_2.Chat
{
    public class ChatEntity
    {
        [Key]
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
