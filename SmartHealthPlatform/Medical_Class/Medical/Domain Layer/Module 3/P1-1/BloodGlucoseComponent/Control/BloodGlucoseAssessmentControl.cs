using Medical.Data_Source_Layer.Module_3.P1_1.BloodGlucoseComponent;
using Medical.Domain_Layer.Module_3.P1_1.Interfaces;
using Medical.Models;
using Medical.Models.Module_3.P1_1.BloodGlucoseComponent;
using Medical.ViewModel.Module_3.P1_1.BloodGlucoseComponent;
using System.Text.Json;
using Medical.Data_Source_Layer;
using Medical.ViewModel.Module_3.P1_1;
using Mediqu.Domain.Services;


namespace Medical.Domain_Layer.Module_3.P1_1
{
	public class BloodGLucoseAssessmentControl : IBGAnalysis
	{
		private readonly SmartHealthPlatformContext _context;
		private readonly IBloodGlucose _bloodGlucoseService;	
		private readonly IBGRisk _riskService;
		private readonly TransformPatientListViewModel _transformer;
		private readonly ILogger<BloodGLucoseAssessmentControl> _logger;
		private readonly List<PatientListViewModel> _allPatients;

		public BloodGLucoseAssessmentControl(
			IBloodGlucose bloodGlucoseService,
			IBGRisk riskService,
			ILogger<BloodGLucoseAssessmentControl> logger,
			TransformPatientListViewModel transformer,
			SmartHealthPlatformContext context,
			PatientListControl patientListService)
		{
			_bloodGlucoseService = bloodGlucoseService;
			_riskService = riskService;
			_transformer = transformer;
			_logger = logger;
			_context = context;
			_allPatients = patientListService.GetAllPatients(); // get all patient data
		}



		public List<BloodGlucoseAnalysisViewModel> AssessRiskForPatient(int patientId)
		{
			Console.WriteLine($"Transforming data for PatientID: {patientId}");

			var analysisData = _transformer.TransformToBloodGluccoseAnalysis(patientId);
			if (!analysisData.Any())
			{
				Console.WriteLine($"No data found for PatientID: {patientId} after transformation.");
				return analysisData; // Or handle the lack of data as needed
			}


			Console.WriteLine($"Data transformed for PatientID: {patientId}, entries: {analysisData.Count}");



			// Directly populate RiskMessages within each AirPulseOximeterAnalysisViewModel
			Assessment(analysisData);

			// Initialize TDG
			var assessmentTDG = new BloodGlucoseAssessment_TDG(_context);

			foreach (var viewModel in analysisData)
			{
				// Convert ViewModel to Database Model
				var assessmentSDM = new BloodGlucoseAssessment_SDM
				{
					PatientID = viewModel.PatientID,
					PatientName = viewModel.PatientName,
					Age = viewModel.Age,
					Gender = viewModel.Gender,
					Timestamp = viewModel.Timestamp,
					BloodGlucoseLevels = viewModel.BloodGlucoseLevels,
					RiskMessagesJson = JsonSerializer.Serialize(viewModel.RiskMessages)
				};

				// Save to database using TDG
				assessmentTDG.Upsert(assessmentSDM);
			}



			return analysisData;
		}



		private void Assessment(List<BloodGlucoseAnalysisViewModel> models)
		{
			foreach (var model in models)
			{
				int idealBGL = _bloodGlucoseService.GenerateIdealBloodGlucoseLevel(model.Age);
				// Assess pulse rate risk for each model (patient)
				
				if (model.BloodGlucoseLevels < idealBGL)
				{
					model.RiskMessages.Add($"Blood Glucose Level is below ideal rate.");
				}
				else if (model.BloodGlucoseLevels > idealBGL)
				{
					model.RiskMessages.Add($"Blood Glucose Level is above ideal rate.");
				}
				else
				{
					model.RiskMessages.Add($"Blood Glucose Level is within ideal range.");
				}

				String risk = _riskService.GenerateBloodGlucoseRisks(model.BloodGlucoseLevels);

				model.RiskMessages.Add(risk);


			}
		}




		// method to populate BloodGlucose data for viewmodel
		public void PopulateLatestBloodGlucoseData(int patientId, ref HealthPractitionerDashboardViewModel dashboardViewModel)
		{
			foreach (var patientListViewModel in _allPatients)
			{
				// Find the patient matching the given userId
				var patient = patientListViewModel.SearchResults.FirstOrDefault(p => p.ID == patientId);
				if (patient != null)
				{


					// Fetch the latest 24 blood glucose readings from the "Blood Glucose" device
					var bloodGlucoseReadings = patient.DeviceReadings
						.Where(r => r.DeviceName == "Blood Glucose")
						.OrderByDescending(r => r.Timestamp)
						.Take(24)
						.SelectMany(r => r.ReadingValues.Where(rv => rv.Key == "blood glucose levels"))
						.Select(rv => rv.Value)
						.ToList();

					// Populate latest reading in the dashboard view model for blood glucose

					if (bloodGlucoseReadings.Any())
					{
						dashboardViewModel.LatestMinBloodGlucose = bloodGlucoseReadings.Min();
						dashboardViewModel.LatestMaxBloodGlucose = bloodGlucoseReadings.Max();
						dashboardViewModel.LatestAvgBloodGlucose = bloodGlucoseReadings.Average();
					}

				}
			}
		}


	}
}
