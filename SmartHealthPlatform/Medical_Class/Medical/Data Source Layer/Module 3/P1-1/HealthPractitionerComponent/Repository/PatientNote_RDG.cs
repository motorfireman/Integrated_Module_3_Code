using Medical.Models;

namespace Medical.Data_Source_Layer.Module_3.P1_1.HealthPractitionerComponent.Repository
{
	// Repository for handling patient notes data
	public class PatientNote_RDG
	{
		private readonly SmartHealthPlatformContext _context;

		// Constructor to initialize the repository with database context
		public PatientNote_RDG(SmartHealthPlatformContext context)
		{
			_context = context;
		}

		// Saves a patient note asynchronously
		public async Task InsertNoteAsync(PatientNote_SDM note)
		{
			_context.PatientNotes.Add(note);
			await _context.SaveChangesAsync();
		}

		// Deletes a patient note by its ID asynchronously
		public async Task<bool> DeleteNoteByIdAsync(int noteId)
		{
			var note = await _context.PatientNotes.FindAsync(noteId);
			if (note != null)
			{
				_context.PatientNotes.Remove(note);
				await _context.SaveChangesAsync();
				return true;
			}
			return false;
		}
	}
}
