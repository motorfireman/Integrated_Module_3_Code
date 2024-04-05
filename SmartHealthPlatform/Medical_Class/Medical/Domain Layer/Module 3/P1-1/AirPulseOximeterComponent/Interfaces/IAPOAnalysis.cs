using Medical.ViewModel.Module_3.P1_1.AirPulseOximeterComponent;
using System.Collections.Generic;

namespace Medical.Domain_Layer.Module_3.P1_1.Interfaces
{
	public interface IAPOAnalysis
	{
		// Method to assess the risk for a patient based on their air pulse oximeter readings with respect to their ID
		List<AirPulseOximeterAnalysisViewModel> AssessRiskForPatient(int patientId);
	}
}
