using Medical.Models.Module_3.P1_1.BoneHealthComponent;
using Microsoft.EntityFrameworkCore;

namespace Medical.Data_Source_Layer.Module_3.P1_1.BoneHealthComponent
{
	// Repository for handling BoneHealthAssessment data (RDG)
	public class BoneHealthAssessment_RDG
	{
		private readonly SmartHealthPlatformContext _context;

		// Constructor to initialize the repository with database context
		public BoneHealthAssessment_RDG(SmartHealthPlatformContext context)
		{
			_context = context;
		}



		// Finds BoneHealthAssessment by its ID
		public BoneHealthAssessment_SDM FindById(int id)
		{
			return _context.Set<BoneHealthAssessment_SDM>().Find(id);
		}





		// Finds BoneHealthAssessment by patient ID and timestamp
		public BoneHealthAssessment_SDM FindByPatientIdAndTimestamp(int patientId, DateTime timestamp)
		{
			return _context.Set<BoneHealthAssessment_SDM>()
						   .FirstOrDefault(a => a.PatientID == patientId && a.Timestamp == timestamp);
		}



		// Updates an BoneHealthAssessment
		public void Update(BoneHealthAssessment_SDM assessment)
		{
			_context.Entry(assessment).State = EntityState.Modified;
			_context.SaveChanges();
		}




		// Deletes an BoneHealthAssessment by its ID
		public void Delete(int id)
		{
			var assessment = FindById(id);
			if (assessment != null)
			{
				_context.Set<BoneHealthAssessment_SDM>().Remove(assessment);
				_context.SaveChanges();
			}
		}
	}
}
