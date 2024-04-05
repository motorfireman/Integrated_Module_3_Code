using Medical.Data_Source_Layer.Module_3.P1_1.HealthPractitionerComponent.Repository;
using Medical.Interfaces;
using Medical.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Medical.Services
{

	//Instantiate the repository, to be able to access the database.
	public class PatientNotesControl : IPatientNotes
	{
		private readonly PatientNote_TDG _noteRepositoryTDG;
		private readonly PatientNote_RDG _noteRepositoryRDG;
		public PatientNotesControl(PatientNote_TDG noteRepositoryTDG, PatientNote_RDG noteRepositoryRDG)
		{
			_noteRepositoryTDG = noteRepositoryTDG;
			_noteRepositoryRDG = noteRepositoryRDG;
		}

		public async Task SavePatientNote(PatientNote_SDM note)
		{
			await _noteRepositoryRDG.InsertNoteAsync(note);
		}

		public IEnumerable<PatientNote_SDM> GetNotesByPatient(int patientId)
		{
			return _noteRepositoryTDG.GetNotesByPatient(patientId);
		}

		public async Task<bool> DeleteNoteById(int noteId)
		{
			return await _noteRepositoryRDG.DeleteNoteByIdAsync(noteId);
		}
	}
}
