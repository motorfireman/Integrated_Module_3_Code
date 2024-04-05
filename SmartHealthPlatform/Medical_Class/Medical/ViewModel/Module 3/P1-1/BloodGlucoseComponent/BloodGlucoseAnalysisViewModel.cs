namespace Medical.ViewModel.Module_3.P1_1.BloodGlucoseComponent
{
	public class BloodGlucoseAnalysisViewModel
	{
		public int PatientID { get; set; }
		public string PatientName { get; set; }
		public int Age { get; set; }
		public string Gender { get; set; }
		public DateTime Timestamp { get; set; }


		// below are specific readings for blood glucose
		
		public double BloodGlucoseLevels { get; set; }
		public List<string> RiskMessages { get; set; } = new List<string>();
	}
}
