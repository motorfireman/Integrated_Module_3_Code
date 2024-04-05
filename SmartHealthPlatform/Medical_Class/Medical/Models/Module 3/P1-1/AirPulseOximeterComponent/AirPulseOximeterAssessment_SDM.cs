using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Medical.Models.Module_3.P1_1.AirPulseOximeterComponent
{
	//Create a table to store patient health data for airpulse and risk message.
	public class AirPulseOximeterAssessment_SDM
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; } // Primary key

		public int PatientID { get; set; }
		public string PatientName { get; set; }
		public int Age { get; set; }
		public string Gender { get; set; }
		public DateTime Timestamp { get; set; }
		public double PerfusionIndex { get; set; }
		public double PulseRate { get; set; }
		public double SpO2 { get; set; }

		// RiskMessages will be stored as a JSON string to simplify database schema
		public string RiskMessagesJson { get; set; }
	}
}
