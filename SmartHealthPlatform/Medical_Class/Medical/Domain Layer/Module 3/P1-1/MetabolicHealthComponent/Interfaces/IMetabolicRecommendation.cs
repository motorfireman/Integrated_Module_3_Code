using Medical.Models.Module_3.P1_1.MetabolicHealthComponent;
using Medical.ViewModel.Module_3.P1_1.BloodPressureComponent;
using Medical.ViewModel.Module_3.P1_1.MetabolicHealthComponent;

namespace Medical.Domain_Layer.Module_3.P1_1.Interfaces
{
	public interface IMetabolicRecommendation
	{
		// MetabolicRecommendations generated using MetabolicRisks
		List<MetabolicRecommendation_SDM> GenerateMetabolicRecommendations(List<MetabolicRisk_SDM> metabolicRisks, MetabolicHealthAnalysisViewModel reading);
	}
}
