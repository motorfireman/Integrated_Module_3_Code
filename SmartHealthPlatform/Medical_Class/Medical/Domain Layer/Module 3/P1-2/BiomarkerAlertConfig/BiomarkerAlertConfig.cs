using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medical.Domain_Layer.Module_3.P1_2.BiomarkerAlertConfig
{   
    public class BiomarkerAlertConfig
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserDeviceId { get; set; }
        public int MpId { get; set; }
        public int BiomarkerId { get; set; }
        public bool TelegramAlertEnabled { get; set; }
        public bool EmailAlertEnabled { get; set; }
        public string Severity { get; set; }
        public float MaxThreshold { get; set; }
        public float MinThreshold { get; set; }
        public int TemplateId { get; set; } 
    }
}