using Medical.Models;

namespace Medical.Data_Source_Layer.Module_3.P1_1.HealthPractitionerComponent.Repository
{
	// Repository for handling patient notes data
	public class PatientNote_TDG
	{
		private readonly SmartHealthPlatformContext _context;

		// Constructor to initialize the repository with database context
		public PatientNote_TDG(SmartHealthPlatformContext context)
		{
			_context = context;
		}

		// Retrieves patient notes by patient ID
		public IEnumerable<PatientNote_SDM> GetNotesByPatient(int patientId)
		{
			return _context.PatientNotes
						   .Where(note => note.PatientID == patientId)
						   .ToList();
		}



	}
}
