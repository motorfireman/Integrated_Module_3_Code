using Medical.ViewModel.Module_3.P1_1.MetabolicHealthComponent;

namespace Medical.Models.Module_3.P1_1.MetabolicHealthComponent
{
	public class MetabolicAssessment_SDM
	{
		public MetabolicHealthAnalysisViewModel? reading { get; set; }
		public List<MetabolicRisk_SDM>? MetabolicRisks { get; set; }
		public List<MetabolicRecommendation_SDM>? MetabolicRecommndations { get; set; }
	}
}



