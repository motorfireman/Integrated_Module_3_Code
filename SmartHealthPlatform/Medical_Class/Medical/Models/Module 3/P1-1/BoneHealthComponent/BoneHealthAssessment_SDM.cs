using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Medical.Models.Module_3.P1_1.BoneHealthComponent
{
	//Create a table to store patient health data for bone health.
	public class BoneHealthAssessment_SDM
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; } // Primary key

		public int PatientID { get; set; }
		public string PatientName { get; set; }
		public int Age { get; set; }
		public string Gender { get; set; }
		public DateTime Timestamp { get; set; }
		public double Weight { get; set; }
		public double BoneMass { get; set; }
		public double BodyFatPercentage { get; set; }
		public double LeanMass { get; set; }
		public double Protein { get; set; }
		public double VisceralFatRating { get; set; }

		// RiskMessages will be stored as a JSON string to simplify database schema
		public string RiskMessagesJson { get; set; }
	}
}
