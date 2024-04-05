using Microsoft.AspNetCore.Mvc;
using Medical.Domain_Layer.Module_3.P1_1.Interfaces;

namespace Mediqu.Controllers
{
	// Controller for managing air pulse oximeter analysis
	public class AirPulseOximeterAnalysisController : Controller
	{
		private readonly IAPOAnalysis _riskAssessmentControl;

		// Constructor to initialize the controller with air pulse oximeter analysis service
		public AirPulseOximeterAnalysisController(IAPOAnalysis riskAssessmentControl)
		{
			_riskAssessmentControl = riskAssessmentControl;
		}
		 


		// Displays air pulse oximeter analysis for the specified patient ID
		public IActionResult AirPulseOximeterAnalysis(int id)
		{
			// Perform risk assessment for the specified patient ID
			var airPulseOximeterData = _riskAssessmentControl.AssessRiskForPatient(id);

			// Pass assessment data to the view
			return View("~/Views/Presentation Layer/Module 3/P1-1/AirPulseOximeterAnalysis.cshtml", airPulseOximeterData);
		}
	}
}
