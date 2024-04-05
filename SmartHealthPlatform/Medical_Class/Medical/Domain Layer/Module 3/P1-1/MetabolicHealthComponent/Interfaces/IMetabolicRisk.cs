using Medical.Models.Module_3.P1_1.MetabolicHealthComponent;
using Medical.ViewModel.Module_3.P1_1.MetabolicHealthComponent;

namespace Medical.Domain_Layer.Module_3.P1_1.Interfaces
{
	public interface IMetabolicRisk
	{
		// Patient ID used to retrieve list of readings to generate list of Metabolic Risks
		List<MetabolicRisk_SDM> GenerateMetabolicRisks(MetabolicHealthAnalysisViewModel reading);
	}
}
