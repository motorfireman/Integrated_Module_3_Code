using Microsoft.AspNetCore.Mvc;
using Medical.Domain_Layer.Module_3.P1_1.Interfaces;
using Medical.ViewModel.Module_3.P1_1.MetabolicHealthComponent;

namespace Medical.Controllers
{
    public class MetabolicHealthAnalysisController : Controller
    {
        // Uses MetabolicHealthTrends Control
        private readonly IMetabolicAnalysis _metabolicHealthTrendsControl;

        // Contructor injection
		public MetabolicHealthAnalysisController(IMetabolicAnalysis metabolicHealthTrendsControl)
		{
			_metabolicHealthTrendsControl = metabolicHealthTrendsControl;
		}

		public IActionResult MetabolicHealthAnalysis (int id) // Patient ID retrieved from URL
        {
			// Generate Metabolic Summary
			var metabolicHealthSummary = _metabolicHealthTrendsControl.GenerateMetabolicAnalysisSummary(id);

            // Retrieve Weekly Measurements
            var metabolicHealthWeeklyMeasurements = _metabolicHealthTrendsControl.RetrieveWeeklyMeasurements(id);

			// Combine summary and measurements into data view model
			var viewData = new MetabolicHealthAnalysisDataViewModel
			{
				Summary = metabolicHealthSummary,
				WeeklyMeasurements = metabolicHealthWeeklyMeasurements
			};

			// Return the view along with the model
			return View("~/Views/Presentation Layer/Module 3/P1-1/MetabolicHealthAnalysis.cshtml", viewData);
        }
    }
}
