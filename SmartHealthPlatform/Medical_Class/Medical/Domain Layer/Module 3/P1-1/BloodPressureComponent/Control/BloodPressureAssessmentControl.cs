using Medical.Data_Source_Layer;
using Medical.Data_Source_Layer.Module_3.P1_1.BloodPressureComponent;
using Medical.Domain_Layer.Module_3.P1_1.BloodPressureComponent.Interface;
using Medical.Models;
using Medical.Models.Module_3.P1_1.BloodPressureComponent;
using Medical.ViewModel.Module_3.P1_1;
using Medical.ViewModel.Module_3.P1_1.BloodPressureComponent;
using Mediqu.Domain.Services;


namespace Medical.Domain_Layer.Module_3.P1_1
{
	public class BloodPressureRiskAssessmentControl : IBPAnalysis
	{
		private readonly SmartHealthPlatformContext _context;
		private readonly IHeartHealthRiskSeverity _HeartHealthSeverity;
		private readonly IMAPRiskSeverity _MAPSeverity;
		private readonly TransformPatientListViewModel _transformer;
		private readonly ILogger<BloodPressureRiskAssessmentControl> _logger;
		private readonly List<PatientListViewModel> _allPatients;


		public BloodPressureRiskAssessmentControl(
			IHeartHealthRiskSeverity HeartHealthRiskSeverity,
			IMAPRiskSeverity MAPRiskSeverity,
			ILogger<BloodPressureRiskAssessmentControl> logger,
			TransformPatientListViewModel transformer,
			PatientListControl patientListService,
			SmartHealthPlatformContext context)
		{
			_HeartHealthSeverity = HeartHealthRiskSeverity;
			_MAPSeverity = MAPRiskSeverity;
			_transformer = transformer;
			_logger = logger;
			_context = context;
			_allPatients = patientListService.GetAllPatients(); // get all patient data
		}

		public List<BloodPressureAnalysisViewModel> GetBPAnalysisSummary(int patientId)
		{

			var analysisData = _transformer.TransformToBloodPressureAnalysis(patientId);

			GetHeartHealthAssessment(analysisData);
			GetMAPAssessment(analysisData);
			InsertIntoDB(analysisData);
			
			return analysisData;
			
		}

		private void InsertIntoDB(List<BloodPressureAnalysisViewModel> models)
		{

			// Initialize TDG
			var assessmentTDG = new BloodPressureAssessment_TDG(_context);

			foreach (var viewModel in models)
			{

				// Convert ViewModel to Database Model
				var assessmentSDM = new BloodPressureAssessment_SDM
				{
					PatientID = viewModel.PatientID,
					PatientName = viewModel.PatientName,
					Age = viewModel.Age,
					Gender = viewModel.Gender,
					Timestamp = viewModel.Timestamp,
					SystolicPressure = viewModel.SystolicPressure,
					DiastolicPressure = viewModel.DiastolicPressure,
					HeartHealthRiskSeverity = viewModel.HeartHealthRiskSeverity,
					MAPRiskSeverity = viewModel.MAPRiskSeverity,
					HeartHealthRiskMessage = viewModel.HeartHealthRiskMessages,
					MAPRiskMessage = viewModel.MAPRiskMessages
				};

				// Save to database using TDG
				assessmentTDG.Upsert(assessmentSDM);
			}

		}

		private void GetHeartHealthAssessment(List<BloodPressureAnalysisViewModel> models)
		{

			foreach (var viewModel in models)
			{

				int HeartHealthRiskSeverity = _HeartHealthSeverity.generateHHRiskSeverity(viewModel.Age, viewModel.Gender, viewModel.SystolicPressure, viewModel.DiastolicPressure);

				viewModel.HeartHealthRiskSeverity = HeartHealthRiskSeverity;

				if (HeartHealthRiskSeverity <= 5 && HeartHealthRiskSeverity >= 0)
				{
					viewModel.HeartHealthRiskMessages = "Low Risk. Maintain a healthy lifestyle with regular exercise and balanced diet.";
				}
				else if (HeartHealthRiskSeverity <= 10 && HeartHealthRiskSeverity >= 6)
				{
					viewModel.HeartHealthRiskMessages = "Moderate Risk. Monitor blood pressure regularly and consider lifestyle modifications like dietary changes and exercise.";
				}
				else if (HeartHealthRiskSeverity <= 15 && HeartHealthRiskSeverity >= 11)
				{
					viewModel.HeartHealthRiskMessages = "High Risk. Consult a healthcare professional for personalized advice and possibly medication.";
				}
				else
				{
					viewModel.HeartHealthRiskMessages = "Very High Risk. Seek immediate medical attention and follow medical advice strictly.";
				}
			}
		}

		private void GetMAPAssessment(List<BloodPressureAnalysisViewModel> models)
		{
			
			foreach (var viewModel in models)
			{

				float MAPRiskSeverity = _MAPSeverity.generateMAPRiskSeverity(viewModel.SystolicPressure, viewModel.DiastolicPressure);

				viewModel.MAPRiskSeverity = MAPRiskSeverity;

				if (MAPRiskSeverity < 70)
				{
					viewModel.MAPRiskMessages = "Low. Monitor blood pressure regularly and maintain a healthy lifestyle.";
				}
				else if (105 >= MAPRiskSeverity && MAPRiskSeverity >= 70)
				{
					viewModel.MAPRiskMessages = "Normal. Continue monitoring blood pressure and ensure regular check-ups.";
				}
				else
				{
					viewModel.MAPRiskMessages = "High. Consult a healthcare professional for further evaluation and potential intervention.";
				}
			}

		}


		// method to populate Air Pulse Oximeter data
		public void PopulateLatestBloodPressureData(int patientId, ref HealthPractitionerDashboardViewModel dashboardViewModel)
		{
			foreach (var patientListViewModel in _allPatients)
			{
				// Find the patient matching the given userId
				var patient = patientListViewModel.SearchResults.FirstOrDefault(p => p.ID == patientId);
				if (patient != null)
				{


					// Attempt to find the latest reading for "Blood Pressure Oximeter"
					var BPReadings = patient.DeviceReadings
						.Where(r => r.DeviceName == "Blood Pressure")
						.OrderByDescending(r => r.Timestamp)
						.FirstOrDefault();


					// Populate latest reading in the dashboard view model for Blood Pressure

					if (BPReadings != null)
					{
						dashboardViewModel.LatestSystolic = (float)(BPReadings.ReadingValues.FirstOrDefault(rv => rv.Key == "systolic")?.Value ?? 0.0f);
						dashboardViewModel.LatestDiastolic = (float)(BPReadings.ReadingValues.FirstOrDefault(r => r.Key == "diastolic")?.Value ?? 0.0f);
					}
				}
			}
		}


	}
}