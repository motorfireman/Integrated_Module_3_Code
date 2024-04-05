using Medical.Domain_Layer.Module_3.P1_1.Interfaces;
using Medical.Models.Module_3.P1_1.MetabolicHealthComponent;
using Medical.ViewModel.Module_3.P1_1.MetabolicHealthComponent;

namespace Medical.Domain_Layer.Module_3.P1_1.MetabolicHealthComponent.Control
{
	public class MetabolicHealthAssessmentControl : IMetabolicAssessment
	{
		// Uses MetabolicHealthRiskControl
		private readonly IMetabolicRisk _metabolicHealthRiskControl;

		// Uses MetabolicHealthRecommendationControl
		private readonly IMetabolicRecommendation _metabolicHealthRecommendationControl;

		// Constructor injection
		public MetabolicHealthAssessmentControl(IMetabolicRisk metabolicHealthRiskControl, IMetabolicRecommendation metabolicHealthRecommendationControl)
		{
			_metabolicHealthRiskControl = metabolicHealthRiskControl;
			_metabolicHealthRecommendationControl = metabolicHealthRecommendationControl;
		}


		// Implement GenerateMetabolicAssessment()
		public MetabolicAssessment_SDM GenerateMetabolicAssessment(MetabolicHealthAnalysisViewModel reading)
		{
	
			// Initialize Metabolic Assessment
			MetabolicAssessment_SDM metabolicAssessment = new MetabolicAssessment_SDM();

			// Initialize List of Risks
			List<MetabolicRisk_SDM> listOfRisks = new List<MetabolicRisk_SDM>();

			// Initialize List of Recommendations
			List<MetabolicRecommendation_SDM> listOfRecommendations = new List<MetabolicRecommendation_SDM>();


			// Generate Risks
			listOfRisks = _metabolicHealthRiskControl.GenerateMetabolicRisks(reading);

			// Generate Recommendations using Risks
			listOfRecommendations = _metabolicHealthRecommendationControl.GenerateMetabolicRecommendations(listOfRisks, reading);

			// Generate Assessment
			metabolicAssessment.reading = reading;
			metabolicAssessment.MetabolicRisks = listOfRisks;
			metabolicAssessment.MetabolicRecommndations = listOfRecommendations;

			return metabolicAssessment;

		}
	}
}
