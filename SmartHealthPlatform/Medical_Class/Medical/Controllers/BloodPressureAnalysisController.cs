using Microsoft.AspNetCore.Mvc;
using Medical.Domain_Layer.Module_3.P1_1.Interfaces;
using Medical.Domain_Layer.Module_3.P1_1.BloodPressureComponent.Interface;

namespace Mediqu.Controllers
{
	// Controller for managing blood pressure analysis
	public class BloodPressureAnalysisController : Controller
	{
		
		private readonly IBPAnalysis _riskAssessmentControl;

		// Constructor to initialize the controller
		public BloodPressureAnalysisController(IBPAnalysis riskAssessmentControl)
		{
			_riskAssessmentControl = riskAssessmentControl;
		}
		
		public IActionResult BloodPressureAnalysis(int id)
		{

			// Perform risk assessment for the specified patient ID
			var bloodPressureData = _riskAssessmentControl.GetBPAnalysisSummary(id);

			// Pass assessment data to the view
			return View("~/Views/Presentation Layer/Module 3/P1-1/BloodPressureAnalysis.cshtml", bloodPressureData);
		}
	}
}
