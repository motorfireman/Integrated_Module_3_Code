using Medical.ViewModel.Module_3.P1_1.BloodGlucoseComponent;
using System.Collections.Generic;


namespace Medical.Domain_Layer.Module_3.P1_1.Interfaces
{
	
	public interface IBGAnalysis
	{
		List<BloodGlucoseAnalysisViewModel> AssessRiskForPatient(int patientId);
	}
}
