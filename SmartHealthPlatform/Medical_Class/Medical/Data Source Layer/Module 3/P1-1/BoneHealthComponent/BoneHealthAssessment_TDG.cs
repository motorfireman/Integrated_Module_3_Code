using Medical.Models.Module_3.P1_1.BoneHealthComponent;
using Microsoft.EntityFrameworkCore;

namespace Medical.Data_Source_Layer.Module_3.P1_1.BoneHealthComponent
{
	// Repository for handling BoneHealthAssessment data (TDG)
	public class BoneHealthAssessment_TDG
	{
		private readonly SmartHealthPlatformContext _context;



		// Constructor to initialize the repository with database context
		public BoneHealthAssessment_TDG(SmartHealthPlatformContext context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}




		// Retrieves all BoneHealthAssessments
		public IEnumerable<BoneHealthAssessment_SDM> GetAll()
		{
			return _context.Set<BoneHealthAssessment_SDM>().AsNoTracking().ToList();
		}




		// Adds a new BoneHealthAssessment
		public void Add(BoneHealthAssessment_SDM assessment)
		{
			_context.Set<BoneHealthAssessment_SDM>().Add(assessment);
			_context.SaveChanges();
		}




		// Upserts (Adds or Updates) an BoneHealthAssessment
		public void Upsert(BoneHealthAssessment_SDM assessment)
		{
			if (assessment == null) throw new ArgumentNullException(nameof(assessment));

			if (_context == null) throw new InvalidOperationException("_context is not initialized.");

			// Attempt to find an existing assessment for the same patient
			var existingAssessment = _context.Set<BoneHealthAssessment_SDM>()
											  .FirstOrDefault(a => a.PatientID == assessment.PatientID && a.Timestamp == assessment.Timestamp);

			if (existingAssessment != null)
			{
				// If found, update the existing record
				existingAssessment.Weight = assessment.Weight;
				existingAssessment.BoneMass = assessment.BoneMass;
				existingAssessment.BodyFatPercentage = assessment.BodyFatPercentage;
				existingAssessment.LeanMass = assessment.LeanMass;
				existingAssessment.Protein = assessment.Protein;
				existingAssessment.VisceralFatRating = assessment.VisceralFatRating;
				// Update any other fields that are relevant

				_context.Entry(existingAssessment).State = EntityState.Modified;
			}
			else
			{
				// If not found, add a new record
				_context.Set<BoneHealthAssessment_SDM>().Add(assessment);
			}
			_context.SaveChanges();
		}
	}
}
