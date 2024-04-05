using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Medical.Models.Module_3.P1_1.BloodGlucoseComponent
{
	public class BloodGlucoseAssessment_SDM
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; } // Primary key

		public int PatientID { get; set; }
		public string PatientName { get; set; }
		public int Age { get; set; }
		public string Gender { get; set; }
		public DateTime Timestamp { get; set; }
		public double BloodGlucoseLevels { get; set; }


		// Recommendations will be stored as a JSON string to simplify database schema
		public string RiskMessagesJson { get; set; }
	}
}





