using Medical.Domain_Layer.Module_3.P1_1.Interfaces;
using Medical.Models;
using Medical.Models.Module_3.P1_1.AirPulseOximeterComponent;
using Medical.Models.Module_3.P1_1.MetabolicHealthComponent;
using Microsoft.EntityFrameworkCore;

namespace Medical.Data_Source_Layer.Module_3.P1_1.MetabolicHealthComponent
{
	public class MetabolicAssessments_TDG
	{
		private readonly SmartHealthPlatformContext _context;

		// Constructor to initialize the repository with database context
		public MetabolicAssessments_TDG(SmartHealthPlatformContext context)
		{
			_context = context;
		}


		// Retrieves all MetabolicAssessments
		public IEnumerable<MetabolicHealthAssessment> GetAllMetabolicAssessments()
		{
			return _context.Set<MetabolicHealthAssessment>().AsNoTracking().ToList();
		}



		// Add a new MetabolicAssessment
		public void AddMetabolicAssessments(MetabolicHealthAssessment assessment)
		{
			_context.Set<MetabolicHealthAssessment>().Add(assessment);
			_context.SaveChanges();
		}


		// Upserts (Adds or Updates) an MetabolicHealthAssessment
		public void Upsert(MetabolicHealthAssessment assessment)
		{
			if (assessment == null) throw new ArgumentNullException(nameof(assessment));

			if (_context == null) throw new InvalidOperationException("_context is not initialized.");

			// Attempt to find an existing assessment for the same patient
			var existingAssessment = _context.Set<MetabolicHealthAssessment>()
											  .FirstOrDefault(a => a.PatientID == assessment.PatientID && a.Timestamp == assessment.Timestamp);

			if (existingAssessment != null)
			{
				// If found, update the existing record
				existingAssessment.Weight = assessment.Weight;
				existingAssessment.Height = assessment.Height;
				existingAssessment.BMR = assessment.BMR;
				existingAssessment.BMI = assessment.BMI;
				existingAssessment.BodyFatPercentage = assessment.BodyFatPercentage;
				existingAssessment.VisceralFatRating = assessment.VisceralFatRating;
				existingAssessment.Protein = assessment.Protein;
				existingAssessment.MetabolicRisks = assessment.MetabolicRisks;
				existingAssessment.MetabolicRecommendations = assessment.MetabolicRecommendations;

				_context.Entry(existingAssessment).State = EntityState.Modified;
			}
			else
			{
				// If not found, add a new record
				_context.Set<MetabolicHealthAssessment>().Add(assessment);
			}
			_context.SaveChanges();
		}

	}
}
