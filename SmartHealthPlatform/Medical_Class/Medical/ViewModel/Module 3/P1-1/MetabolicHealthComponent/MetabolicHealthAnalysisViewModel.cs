namespace Medical.ViewModel.Module_3.P1_1.MetabolicHealthComponent
{
	public class MetabolicHealthAnalysisViewModel
	{
		public int PatientID { get; set; }
		public string PatientName { get; set; }
		public int Age { get; set; }
		public string Gender { get; set; }
		public DateTime Timestamp { get; set; }
		public double Weight { get; set; }
		public double Height { get; set; }
		public double BMR { get; set; }
		public double BMI { get; set; }
		public double BodyFatPercentage { get; set; }
		public double VisceralFatRating { get; set; }
		public double Protein { get; set; }

	}

}
