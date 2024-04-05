using System.ComponentModel.DataAnnotations;

namespace Medical.Domain_Layer.Module_3.P1_2.AlertTrigger
{
    public class AlertTriggerEntity
    {
        [Key]
        public int id { get; set; }
        public int userId { get; set; }
        public int recipientId { get; set; }
        public string type { get; set; }
        public int caregiverId { get; set; }
        public int alertConfigId { get; set; }
        public DateTime triggeredOn { get; set; }
        public string message { get; set; }
    }
}