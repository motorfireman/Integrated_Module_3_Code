using Medical.Data_Source_Layer;
using Medical.Domain_Layer.Module_3.P1_1;
using Medical.Models;
using Medical.ViewModel.Module_3.P1_1;
using System.Text.Json;

namespace Mediqu.Domain.Services
{
	public class RetrieveDataControl : IRetrieveData
	{

		private readonly SmartHealthPlatformContext _context;

		public RetrieveDataControl(SmartHealthPlatformContext context)
		{
			_context = context;
		}


		public Dictionary<string, List<string>> GetLatestRiskMessagesByPatientId(int patientId)
		{
			var riskMessages = new Dictionary<string, List<string>>();

			// Fetch the latest risk message for Air Pulse Oximeter
			var airPulseOximeterLatest = _context.AirPulseOximeterAssessment_SDM
				.Where(x => x.PatientID == patientId)
				.OrderByDescending(x => x.Timestamp)
				.FirstOrDefault();

			if (airPulseOximeterLatest != null)
			{
				var messages = JsonSerializer.Deserialize<List<string>>(airPulseOximeterLatest.RiskMessagesJson) ?? new List<string>();
				riskMessages["Air Pulse Oximeter"] = messages;
			}

			// Fetch the latest risk message for Blood Glucose
			var bloodGlucoseLatest = _context.BloodGlucoseAssessment_SDM
				.Where(x => x.PatientID == patientId)
				.OrderByDescending(x => x.Timestamp)
				.FirstOrDefault();

			if (bloodGlucoseLatest != null)
			{
				var messages = JsonSerializer.Deserialize<List<string>>(bloodGlucoseLatest.RiskMessagesJson) ?? new List<string>();
				riskMessages["Blood Glucose"] = messages;
			}

			return riskMessages;

		}


	}
}
