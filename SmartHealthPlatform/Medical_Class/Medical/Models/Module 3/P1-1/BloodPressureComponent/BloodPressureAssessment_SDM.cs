using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medical.Models.Module_3.P1_1.BloodPressureComponent
{
	//Create a table to store patient health data for blood pressure and risk severity + messages.
	public class BloodPressureAssessment_SDM
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; } // Primary key

		public int PatientID { get; set; }
		public string PatientName { get; set; }
		public int Age { get; set; }
		public string Gender { get; set; }
		public DateTime Timestamp { get; set; }
		public float SystolicPressure { get; set; }
		public float DiastolicPressure { get; set; }

        public int HeartHealthRiskSeverity{ get; set; }
        public string HeartHealthRiskMessage { get; set; }
        public float MAPRiskSeverity { get; set; }
        public string MAPRiskMessage { get; set; }
    }
}
