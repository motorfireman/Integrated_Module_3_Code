using Medical.Models.Module_3.P1_1.MetabolicHealthComponent;
using Medical.ViewModel.Module_3.P1_1.MetabolicHealthComponent;

namespace Medical.Domain_Layer.Module_3.P1_1.Interfaces
{
	public interface IMetabolicAnalysis
	{
		// Return String for now. Need to create a new entity called MetabolicAnalysisSummary
		MetabolicHealthAnalysisSummary GenerateMetabolicAnalysisSummary(int patientId);

		List<MetabolicHealthAnalysisViewModel> RetrieveWeeklyMeasurements(int patientId);

	}
}
