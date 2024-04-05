using Medical.ViewModel.Module_3.P1_1;
using Medical.ViewModel.Module_3.P1_1.BoneHealthComponent;

namespace Medical.Domain_Layer.Module_3.P1_1.BoneHealthComponent.Interfaces
{
	public interface IBoneHealthAnalysis
	{
		// Method to assess the risk for a patient based on their bone health readings with respect to their ID
		List<BoneHealthAnalysisViewModel> AssessRiskForPatient(int patientId);

		void PopulateLatestBoneHealthData(int patientId, ref HealthPractitionerDashboardViewModel dashboardViewModel);
	}
}
