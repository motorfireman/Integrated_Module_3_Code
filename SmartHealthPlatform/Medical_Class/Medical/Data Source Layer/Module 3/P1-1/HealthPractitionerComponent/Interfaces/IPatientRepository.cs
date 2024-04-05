using Medical.Models;
using Medical.ViewModel.Module_3.P1_1;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Medical.Data_Source_Layer.Module_3.P1_1.DataAccessLayerInterface
{
	// Interface for accessing patient data
	public interface IPatientRepository
	{
		// Asynchronously retrieves all patients with details
		Task<List<PatientListViewModel>> GetAllPatientsWithDetailsAsync();
	}
}
