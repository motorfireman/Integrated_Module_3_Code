using Mediqu.Domain.Services;
using Medical.ViewModel.Module_3.P1_1;
using Microsoft.AspNetCore.Mvc;
using Medical.Domain_Layer.Module_3.P1_1.HealthPractitionerComponent;
using Medical.Models;
using Microsoft.EntityFrameworkCore;
using System;
using Medical.Data_Source_Layer;

namespace Mediqu.Controllers
{
	// Controller for managing patient list functionality
	public class PatientListController : Controller
	{
		private readonly PatientListControl _patientListService;
		private readonly SmartHealthPlatformContext _context;

		// Constructor to initialize the controller with patient list service and database context
		public PatientListController(PatientListControl patientListService, SmartHealthPlatformContext context)
		{
			_patientListService = patientListService;
			_context = context;
		}

		// Displays the patient search page
		public IActionResult PatientList()
		{
			return View("~/Views/Presentation Layer/Module 3/P1-1/PatientList.cshtml");
		}


		// Returns the search result of patients based on applied filters
		public IActionResult PatientListResult(PatientListViewModel model, int page = 1, int pageSize = 10)
		{
			model.CurrentPage = page;
			model.PageSize = pageSize;

			// Calculate total count of filtered patients
			model.TotalCount = _patientListService.CountFilteredPatients(model.NameFilter, model.DeviceFilter, model.AgeGroupFilter, model.GenderFilter);

			// Calculate total pages needed based on total count and page size
			model.TotalPages = (int)Math.Ceiling(model.TotalCount / (double)model.PageSize);

			// Fetch filtered patients for the respective page
			model.SearchResults = _patientListService.FilterPatients(model, model.CurrentPage, model.PageSize, model.SortField, model.SortOrder);

			return View("~/Views/Presentation Layer/Module 3/P1-1/PatientListResult.cshtml", model);
		}



		// Responds to AJAX requests by returning the total count of filtered patient records
		[HttpGet("count")]
		public ActionResult<int> GetFilteredResultsCount(PatientListViewModel model)
		{
			// Apply filters and calculate total count of matching patient records
			int count = _patientListService.CountFilteredPatients(model.NameFilter, model.DeviceFilter, model.AgeGroupFilter, model.GenderFilter);

			// Return count in HTTP response body as JSON object
			return Ok(count);
		}
























		//public async Task<IActionResult> PatientList()
		//{
		//	if (!_context.P1_1PatientListData.Any())
		//	{
		//		var patients = PatientDataGenerator.GeneratePatients(25);
		//		await InsertPatientsInBatchesAsync(patients, 10);

		//		Console.WriteLine("Generated and inserted patient data.");
		//	}

		//	return View("~/Views/PresentationLayer/Module3/P1_1/PatientList.cshtml");
		//}

		//public async Task InsertPatientsInBatchesAsync(List<P1_1PatientListData> patients, int batchSize = 10)
		//{
		//	for (int i = 0; i < patients.Count; i += batchSize)
		//	{
		//		var batch = patients.Skip(i).Take(batchSize).ToList();
		//		await _context.P1_1PatientListData.AddRangeAsync(batch);
		//		await _context.SaveChangesAsync();
		//		_context.ChangeTracker.Clear(); // Clears the change tracker to free up memory
		//	}
		//}





	}
} 

		
