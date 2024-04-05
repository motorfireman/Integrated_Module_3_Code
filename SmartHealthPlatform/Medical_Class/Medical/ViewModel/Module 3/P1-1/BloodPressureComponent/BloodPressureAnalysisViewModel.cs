namespace Medical.ViewModel.Module_3.P1_1.BloodPressureComponent
{
	public class BloodPressureAnalysisViewModel
	{
		public int PatientID { get; set; }
		public string PatientName { get; set; }
		public int Age { get; set; }
		public string Gender { get; set; }
		public DateTime Timestamp { get; set; }

		public float SystolicPressure { get; set; }
		public float DiastolicPressure { get; set; }
        public int HeartHealthRiskSeverity { get; set; }
        public string HeartHealthRiskMessages { get; set; }
        public float MAPRiskSeverity { get; set; }
        public string MAPRiskMessages { get; set; }

    }

}
