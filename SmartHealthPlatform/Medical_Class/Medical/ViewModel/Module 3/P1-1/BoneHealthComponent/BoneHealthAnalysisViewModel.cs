namespace Medical.ViewModel.Module_3.P1_1.BoneHealthComponent
{
	public class BoneHealthAnalysisViewModel
	{
		public int PatientID { get; set; }
		public string PatientName { get; set; }
		public int Age { get; set; }
		public string Gender { get; set; }
		public DateTime Timestamp { get; set; }


		// below are specific readings for bone health
		public double Weight { get; set; }
		public double BoneMass { get; set; }
		public double BodyFatPercentage { get; set; }
		public double LeanMass { get; set; }
		public double Protein { get; set; }
		public double VisceralFatRating { get; set; }
		public List<string> RiskMessages { get; set; } = new List<string>();
	}
}
