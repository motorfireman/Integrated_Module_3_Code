using Medical.ViewModel.Module_3.P1_1;
using Medical.Models;

namespace Medical.Domain_Layer.Module_3.P1_1
{
	// Provides module to p1-2
	public interface IRetrieveData
	{

		// Method to get the latest risk message by patient ID
		public Dictionary<string, List<string>> GetLatestRiskMessagesByPatientId(int patientId);

	}
}
