using Medical.Data_Source_Layer.Module_3.P1_1.BoneHealthComponent;
using Medical.Data_Source_Layer;
using Medical.Domain_Layer.Module_3.P1_1.Interfaces;
using Medical.Domain_Layer.Module_3.P1_2.Communication;
using Medical.Models.Module_3.P1_1.BoneHealthComponent;
using Medical.ViewModel.Module_3.P1_1.BoneHealthComponent;
using System.Text.Json;
using Medical.Domain_Layer.Module_3.P1_1.BoneHealthComponent.Interfaces;
using Medical.ViewModel.Module_3.P1_1;
using Mediqu.Domain.Services;

namespace Medical.Domain_Layer.Module_3.P1_1.BoneHealthComponent.Control
{
	public class BoneHealthAssessmentControl : IBoneHealthAnalysis
	{
		private readonly SmartHealthPlatformContext _context;
		private readonly TransformPatientListViewModel _transformer;
		private readonly ILogger<BoneHealthAssessmentControl> _logger;
		private readonly List<PatientListViewModel> _allPatients;

		public BoneHealthAssessmentControl(
			ILogger<BoneHealthAssessmentControl> logger,
			TransformPatientListViewModel transformer,
			PatientListControl patientListService,
			SmartHealthPlatformContext context
			)
		{
			_transformer = transformer;
			_logger = logger;
			_context = context;
			_allPatients = patientListService.GetAllPatients(); // get all patient data
		}



		public List<BoneHealthAnalysisViewModel> AssessRiskForPatient(int patientId)
		{
			// List of Bone Health readings
			var analysisData = _transformer.TransformToBoneHealthAnalysis(patientId);

			// Directly populate RiskMessages within each BoneHealthAnalysisViewModel
			Assessment(analysisData);

			// Initialize TDG
			var assessmentTDG = new BoneHealthAssessment_TDG(_context);

			foreach (var viewModel in analysisData)
			{
				// Convert ViewModel to Database Model (SDM)
				// Converting readings to assessment data
				var assessmentSDM = new BoneHealthAssessment_SDM
				{
					PatientID = viewModel.PatientID,
					PatientName = viewModel.PatientName,
					Age = viewModel.Age,
					Gender = viewModel.Gender,
					Timestamp = viewModel.Timestamp,
					Weight = viewModel.Weight,
					BoneMass = viewModel.BoneMass,
					BodyFatPercentage = viewModel.BodyFatPercentage,
					LeanMass = viewModel.LeanMass,
					Protein = viewModel.Protein,
					VisceralFatRating = viewModel.VisceralFatRating,
					RiskMessagesJson = JsonSerializer.Serialize(viewModel.RiskMessages)
				};

				//_messageSender.Send(viewModel.PatientID, "Assessment Completion", "Your assessment is completed. Please check your email or Telegram for details.", MessageType.Email, MessageType.Telegram);

				// Save to database using TDG
				assessmentTDG.Upsert(assessmentSDM);
			}



			return analysisData;
		}



		private void Assessment(List<BoneHealthAnalysisViewModel> models)
		{
			foreach (var model in models)
			{
				// Defining LOW and HIGH thresholds
				double lowBoneMassThreshold = 2.0; // Below this is considered LOW
				double highBodyFatPercentageThreshold = 30.0; // Above this is considered HIGH
				double lowProteinThreshold = 14.0; // Below this is considered LOW
				int highVisceralFatRatingThreshold = 45; // Above this is considered HIGH

				// Checking correlations and adding risks
				if (model.BoneMass < lowBoneMassThreshold && model.BodyFatPercentage > highBodyFatPercentageThreshold)
				{
					model.RiskMessages.Add("Risk of osteoporosis & fractures due to LOW bone mass and HIGH body fat percentage.");
				}

				if (model.Protein < lowProteinThreshold && model.BoneMass < lowBoneMassThreshold)
				{
					model.RiskMessages.Add("Risk of poor bone health due to LOW protein and LOW bone mass.");
				}

				if (model.VisceralFatRating > highVisceralFatRatingThreshold && model.BoneMass < lowBoneMassThreshold)
				{
					model.RiskMessages.Add("Risk of metabolic syndrome due to HIGH visceral fat rating and LOW bone mass.");
				}
			}
		}






		// method to populate PopulateLatestBoneHealthData
		public void PopulateLatestBoneHealthData(int patientId, ref HealthPractitionerDashboardViewModel dashboardViewModel)
		{
			foreach (var patientListViewModel in _allPatients)
			{
				// Find the patient matching the given userId
				var patient = patientListViewModel.SearchResults.FirstOrDefault(p => p.ID == patientId);
				if (patient != null)
				{


					// Attempt to find the latest reading for "Air Pulse Oximeter"
					var latestReading = patient.DeviceReadings
						.Where(r => r.DeviceName == "Air Pulse Oximeter")
						.OrderByDescending(r => r.Timestamp)
						.FirstOrDefault();


					// Populate latest reading in the dashboard view model for airpulse
					if (latestReading != null)
					{
						dashboardViewModel.LatestPerfusionIndex = latestReading.ReadingValues.FirstOrDefault(r => r.Key == "Perfusion Index")?.Value ?? 0;
						dashboardViewModel.LatestPulseRate = (int)(latestReading.ReadingValues.FirstOrDefault(r => r.Key == "Pulse rate")?.Value ?? 0);
						dashboardViewModel.LatestSpO2 = latestReading.ReadingValues.FirstOrDefault(r => r.Key == "SP02")?.Value ?? 0;
					}
				}
			}
		}



	}
}
