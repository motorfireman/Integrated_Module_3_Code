using Medical.ViewModel.Module_3.P1_1.BloodPressureComponent;

namespace Medical.Domain_Layer.Module_3.P1_1.BloodPressureComponent.Interface
{
	public interface IBPAnalysis
	{
		List<BloodPressureAnalysisViewModel> GetBPAnalysisSummary(int patientId);

	}
}
