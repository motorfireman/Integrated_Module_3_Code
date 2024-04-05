using Medical.Interfaces;
using Medical.Models;
using Microsoft.AspNetCore.Mvc;

// Controller for managing patient notes
[Route("[controller]")]
public class PatientNotesController : Controller
{
	private readonly IPatientNotes _noteService;

	// Constructor to initialize the controller with the patient notes service
	public PatientNotesController(IPatientNotes noteService)
	{
		_noteService = noteService;
	}

	// Saves a patient note
	[HttpPost]
	[Route("SavePatientNote")]
	public async Task<IActionResult> SavePatientNote([FromBody] PatientNote_SDM note)
	{
		if (ModelState.IsValid)
		{
			await _noteService.SavePatientNote(note);
			return Json(new { success = true, message = "Note saved successfully" });
		}
		return Json(new { success = false, message = "Invalid note data" });
	}

	// Retrieves patient notes by patient ID
	[HttpGet("GetNotesByPatient/{patientId}")]
	public IActionResult GetNotesByPatient(int patientId)
	{
		var notes = _noteService.GetNotesByPatient(patientId);
		return Json(notes);
	}

	// Deletes a patient note by its ID
	[HttpDelete("DeleteNote/{noteId}")]
	public async Task<IActionResult> DeleteNote(int noteId)
	{
		await _noteService.DeleteNoteById(noteId);
		return NoContent(); // 204 No Content status code
	}


}
