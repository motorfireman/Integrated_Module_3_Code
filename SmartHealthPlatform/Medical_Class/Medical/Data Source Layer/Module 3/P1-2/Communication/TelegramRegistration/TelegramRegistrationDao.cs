using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medical.Data_Source_Layer.Module_3.P1_2.Communication;

public class TelegramRegistrationDao
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Token { get; set; }
    public DateTime TokenGenerated { get; set; }
    public int? TelegramId { get; set; }
}