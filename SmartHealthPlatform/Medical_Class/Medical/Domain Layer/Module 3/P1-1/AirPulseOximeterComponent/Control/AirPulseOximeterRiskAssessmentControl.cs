using Medical.Data_Source_Layer.Module_3.P1_1.AirPulseOximeterComponent;
using Medical.Domain_Layer.Module_3.P1_1.Interfaces;
using Medical.Models.Module_3.P1_1.AirPulseOximeterComponent;
using Medical.ViewModel.Module_3.P1_1.AirPulseOximeterComponent;
using System.Text.Json;
using Medical.Data_Source_Layer;
using Medical.Domain_Layer.Module_3.Mock;
using Medical.Domain_Layer.Module_3.P1_2.Communication;
using Mediqu.Domain.Services;
using Medical.ViewModel.Module_3.P1_1;


namespace Medical.Domain_Layer.Module_3.P1_1
{
	public class AirPulseOximeterRiskAssessmentControl : IAPOAnalysis
	{
		private readonly SmartHealthPlatformContext _context;
		private readonly IPulseRate _pulseRateService;
		private readonly ISpO2 _spO2Service;
		private readonly IPerfusionIndex _perfusionIndexService;
		private readonly IMessageSender _messageSender;
		private readonly IUserData _userData;
		private readonly TransformPatientListViewModel _transformer;
		private readonly ILogger<AirPulseOximeterRiskAssessmentControl> _logger;
		private readonly List<PatientListViewModel> _allPatients;

		public AirPulseOximeterRiskAssessmentControl(
			IMessageSender messageSender,
			IUserData userData,
			IPulseRate pulseRateService,
			ISpO2 spO2Service,
			IPerfusionIndex perfusionIndexService,
			ILogger<AirPulseOximeterRiskAssessmentControl> logger,
			TransformPatientListViewModel transformer,
			SmartHealthPlatformContext context, 
			PatientListControl patientListService)
		{
			_pulseRateService = pulseRateService;
			_spO2Service = spO2Service;
			_perfusionIndexService = perfusionIndexService;
			_transformer = transformer;
			_logger = logger;
			_context = context;
			_messageSender = messageSender;
			_userData = userData;
			_allPatients = patientListService.GetAllPatients(); // get all patient data
		}



		public List<AirPulseOximeterAnalysisViewModel> AssessRiskForPatient(int patientId)
		{
			var analysisData = _transformer.TransformToAirPulseOximeterAnalysis(patientId);

			// Directly populate RiskMessages within each AirPulseOximeterAnalysisViewModel
			Assessment(analysisData);

			// Initialize TDG
			var assessmentTDG = new AirPulseOximeterAssessment_TDG(_context);

			foreach (var viewModel in analysisData)
			{
				// Convert ViewModel to Database Model
				var assessmentSDM = new AirPulseOximeterAssessment_SDM
				{
					PatientID = viewModel.PatientID,
					PatientName = viewModel.PatientName,
					Age = viewModel.Age,
					Gender = viewModel.Gender,
					Timestamp = viewModel.Timestamp,
					PerfusionIndex = viewModel.PerfusionIndex,
					PulseRate = viewModel.PulseRate,
					SpO2 = viewModel.SpO2,
					RiskMessagesJson = JsonSerializer.Serialize(viewModel.RiskMessages)
				};

				// Save to database using TDG
				assessmentTDG.Upsert(assessmentSDM);
			}


			try
			{
				//_messageSender.Send(_userData.GetCurrentUser().Id, "Assessment Completion", "Your assessment is completed. Please check your email or Telegram for details.", MessageType.Email, MessageType.Telegram);
			}
			catch (Exception ex)
			{
				//Console.WriteLine($"Failed to send message for patient {patientId}: {ex.Message}");
			}


			return analysisData;
		}



		private void Assessment(List<AirPulseOximeterAnalysisViewModel> models)
		{
			foreach (var model in models)
			{
				// Assess pulse rate risk for each model (patient)
				int idealPulseRate = _pulseRateService.GenerateIdealPulseRate(model.Age);
				if (model.PulseRate < idealPulseRate)
				{
					model.RiskMessages.Add($"Pulse rate is below ideal resting rate.");
				}
				else if (model.PulseRate > idealPulseRate)
				{
					model.RiskMessages.Add($"Pulse rate is above ideal resting rate.");
				}
				else
				{
					model.RiskMessages.Add($"Pulse rate is within the ideal range.");
				}

				// Assess SpO2 risk for each model (patient)
				double idealSpO2 = _spO2Service.GenerateIdealSpO2();
				if (model.SpO2 < idealSpO2)
				{
					model.RiskMessages.Add($"SpO2 levels are below the ideal range, indicating potential hypoxemia.");
				}

				// Assess perfusion index risk for each model (patient)
				bool isPerfusionNormal = _perfusionIndexService.IsPerfusionIndexNormal(model.PerfusionIndex);
				if (!isPerfusionNormal)
				{
					model.RiskMessages.Add($"Perfusion index is outside the normal range, indicating potential issues with blood circulation.");
				}
			}
		}


		// method to populate Air Pulse Oximeter data
		public void PopulateLatestAirPulseOximeterData(int patientId, ref HealthPractitionerDashboardViewModel dashboardViewModel)
		{
			foreach (var patientListViewModel in _allPatients)
			{
				// Find the patient matching the given userId
				var patient = patientListViewModel.SearchResults.FirstOrDefault(p => p.ID == patientId);
				if (patient != null)
				{

					// Attempt to find the latest reading for "Bone Health"
					var latestBHReading = patient.DeviceReadings
						.Where(r => r.DeviceName == "Body Composition")
						.OrderByDescending(r => r.Timestamp)
						.FirstOrDefault();

					// Populate latest reading in the dashboard view model for bone health
					if (latestBHReading != null)
					{
						dashboardViewModel.LatestBoneMass = latestBHReading.ReadingValues.FirstOrDefault(r => r.Key == "Bone Mass")?.Value ?? 0;
						dashboardViewModel.LatestLeanMass = latestBHReading.ReadingValues.FirstOrDefault(r => r.Key == "Lean Mass")?.Value ?? 0;
						dashboardViewModel.LatestProtein = latestBHReading.ReadingValues.FirstOrDefault(r => r.Key == "Protein")?.Value ?? 0;
					}

				}
			}
		}






	}
}
