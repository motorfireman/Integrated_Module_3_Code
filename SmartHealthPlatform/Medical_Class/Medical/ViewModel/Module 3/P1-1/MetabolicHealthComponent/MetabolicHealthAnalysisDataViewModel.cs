using Medical.Models.Module_3.P1_1.MetabolicHealthComponent;

namespace Medical.ViewModel.Module_3.P1_1.MetabolicHealthComponent
{
	public class MetabolicHealthAnalysisDataViewModel
	{
		public MetabolicHealthAnalysisSummary Summary { get; set; }
		public List<MetabolicHealthAnalysisViewModel> WeeklyMeasurements { get; set; }
	}

}
