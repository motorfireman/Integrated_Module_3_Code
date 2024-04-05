using Medical.Models;

namespace Medical.ViewModel.Module_3.P1_1
{

	// A composite ViewModel to hold multiple pieces of data
	public class HealthPractitionerDashboardViewModel
	{
		public PatientViewModel Patient { get; set; }
		public int ActiveDeviceCount { get; set; }
		public DateTime? LastVisitDate { get; set; }

		// for air pulse summary card
		public double LatestPerfusionIndex { get; set; }
		public int LatestPulseRate { get; set; }
		public double LatestSpO2 { get; set; }

		// for bone health summary card
		public double LatestBoneMass { get; set; }
		public double LatestLeanMass { get; set; }
		public double LatestProtein { get; set; }


		// for Blood glucose summary card
		public double LatestMaxBloodGlucose { get; set; }

		public double LatestMinBloodGlucose { get; set; }

		public double LatestAvgBloodGlucose { get; set; }


		// for blood pressure
		public float LatestSystolic { get; set; }
		public float LatestDiastolic { get; set; }


		// for metabolic health summary card
		public float LatestWeight { get; set; }
		public float LatestBodyFatPercentage { get; set; }
		public string CurrentRisk { get; set; }



		// You can add other data points or ViewModels here as needed



	}
}
