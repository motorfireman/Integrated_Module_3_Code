using Medical.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Medical.Interfaces
{
	//For PatientNotesControl class
	public interface IPatientNotes
	{
		Task SavePatientNote(PatientNote_SDM note);
		IEnumerable<PatientNote_SDM> GetNotesByPatient(int patientId);
		Task<bool> DeleteNoteById(int noteId); 
	}
}
