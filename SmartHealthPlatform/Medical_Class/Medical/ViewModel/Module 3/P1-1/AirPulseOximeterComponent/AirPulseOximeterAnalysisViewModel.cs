namespace Medical.ViewModel.Module_3.P1_1.AirPulseOximeterComponent
{
	public class AirPulseOximeterAnalysisViewModel
	{
		public int PatientID { get; set; }
		public string PatientName { get; set; }
		public int Age { get; set; }
		public string Gender { get; set; }
		public DateTime Timestamp { get; set; }


		// below are specific readings for air pulse
		public double PerfusionIndex { get; set; }
		public double PulseRate { get; set; }
		public double SpO2 { get; set; }
		public List<string> RiskMessages { get; set; } = new List<string>();

	}

}
