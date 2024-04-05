using Medical.Domain_Layer.Module_3.P1_1;
using Mediqu.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Mediqu.Controllers
{
	// Controller for managing health practitioner dashboard
	public class HealthPractitionerDashboardController : Controller
	{

		// Constructor to initialize the controller with health practitioner dashboard service
		private readonly HealthPractitionerDashboardControl _patientDataService;

		public HealthPractitionerDashboardController(HealthPractitionerDashboardControl patientDataService)
		{
			_patientDataService = patientDataService;
		}




		// Displays health practitioner dashboard with data related to the specified patient ID
		public IActionResult HealthPractitionerDashboard(int id)
		{
			// Fetch data for the specified patient ID
			var patientData = _patientDataService.FetchDashboardDataById(id);

			if (patientData == null)
			{
				// Handle the case where no patient data is found
				return NotFound();
			}

			// Pass patient data to the dashboard view
			return View("~/Views/Presentation Layer/Module 3/P1-1/HealthPractitionerDashboard.cshtml", patientData);
		}
	}
}
