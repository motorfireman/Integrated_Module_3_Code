using Medical.Data_Source_Layer.Module_3.P1_1.BloodGlucoseComponent;
using Medical.Domain_Layer.Module_3.P1_1.Interfaces;
using Medical.Models;
using Medical.Models.Module_3.P1_1.BloodGlucoseComponent;
using Medical.ViewModel.Module_3.P1_1.BloodGlucoseComponent;
using System.Text.Json;
using Medical.Data_Source_Layer;


namespace Medical.Domain_Layer.Module_3.P1_1
{
	public class BloodGLucoseAssessmentControl : IBGAnalysis
	{
		private readonly SmartHealthPlatformContext _context;
		private readonly IBloodGlucose _bloodGlucoseService;	
		private readonly IBGRisk _riskService;
		private readonly TransformPatientListViewModel _transformer;
		private readonly ILogger<BloodGLucoseAssessmentControl> _logger;
		

		public BloodGLucoseAssessmentControl(
			IBloodGlucose bloodGlucoseService,
			IBGRisk riskService,
			ILogger<BloodGLucoseAssessmentControl> logger,
			TransformPatientListViewModel transformer,
			SmartHealthPlatformContext context)
		{
			_bloodGlucoseService = bloodGlucoseService;
			_riskService = riskService;
			
			_transformer = transformer;
			_logger = logger;
			_context = context;

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





	}
}
