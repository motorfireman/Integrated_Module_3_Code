using Medical.Domain_Layer.Module_3.P1_1.Interfaces;
using Medical.Models.Module_3.P1_1.MetabolicHealthComponent;
using Medical.ViewModel.Module_3.P1_1;
using Medical.ViewModel.Module_3.P1_1.MetabolicHealthComponent;
using Mediqu.Domain.Services;

namespace Medical.Domain_Layer.Module_3.P1_1.MetabolicHealthComponent.Control
{
	public class MetabolicHealthAssessmentControl : IMetabolicAssessment
	{
		// Uses MetabolicHealthRiskControl
		private readonly IMetabolicRisk _metabolicHealthRiskControl;

		// Uses MetabolicHealthRecommendationControl
		private readonly IMetabolicRecommendation _metabolicHealthRecommendationControl;


		private readonly List<PatientListViewModel> _allPatients;


		// Constructor injection
		public MetabolicHealthAssessmentControl(IMetabolicRisk metabolicHealthRiskControl, IMetabolicRecommendation metabolicHealthRecommendationControl, PatientListControl patientListService)
		{
			_metabolicHealthRiskControl = metabolicHealthRiskControl;
			_metabolicHealthRecommendationControl = metabolicHealthRecommendationControl;
			_allPatients = patientListService.GetAllPatients(); // get all patient data
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


		// method to populate data for viewmodel
		public void PopulateLatestMetabolicData(int patientId, ref HealthPractitionerDashboardViewModel dashboardViewModel)
		{
			foreach (var patientListViewModel in _allPatients)
			{
				// Find the patient matching the given userId
				var patient = patientListViewModel.SearchResults.FirstOrDefault(p => p.ID == patientId);
				if (patient != null)
				{
					// Attempt to find the latest reading for "Body Composition"
					var latestMetabolicReadings = patient.DeviceReadings
						.Where(r => r.DeviceName == "Body Composition")
						.OrderByDescending(r => r.Timestamp)
						.FirstOrDefault();


					// Populate latest reading in the dashboard view model for metabolic

					if (latestMetabolicReadings != null)
					{
						dashboardViewModel.LatestWeight = (float)(latestMetabolicReadings.ReadingValues.FirstOrDefault(rv => rv.Key == "Weight")?.Value ?? 0.0f);
						dashboardViewModel.LatestBodyFatPercentage = (float)(latestMetabolicReadings.ReadingValues.FirstOrDefault(r => r.Key == "Body Fat Percentage")?.Value ?? 0.0f);
						dashboardViewModel.CurrentRisk = "Low Metabolic Risk";
					}


				}
			}
		}



	}
}
