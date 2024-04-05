using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medical.Models
{
	//create a table to store notes for respective patient
	public class PatientNote_SDM
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public int PatientID { get; set; }
		public string PatientName { get; set; }
		public string UserName { get; set; }
		public string Note { get; set; }
		public DateTime Timestamp { get; set; } = DateTime.UtcNow;
	}
}
