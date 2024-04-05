using Medical.Domain_Layer.Module_3.P1_1.BoneHealthComponent.Interfaces;
using Medical.Domain_Layer.Module_3.P1_1.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Medical.Controllers
{
	// Controller for managing bone health analysis
	public class BoneHealthAnalysisController : Controller
	{
		private readonly IBoneHealthAnalysis _riskAssessmentControl;

		// Constructor to initialize the controller with bone health analysis service
		public BoneHealthAnalysisController(IBoneHealthAnalysis riskAssessmentControl)
		{
			_riskAssessmentControl = riskAssessmentControl;
		}



		// Displays bone health analysis for the specified patient ID
		public IActionResult BoneHealthAnalysis(int id)
		{
			// Perform risk assessment for the specified patient ID
			var boneHealthData = _riskAssessmentControl.AssessRiskForPatient(id);

			// Pass assessment data to the view
			return View("~/Views/Presentation Layer/Module 3/P1-1/BoneHealthAnalysis.cshtml", boneHealthData);
		}
	}
}
