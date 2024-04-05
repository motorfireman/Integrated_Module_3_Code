using Medical.Models.Module_3.P1_1.BloodGlucoseComponent;
using Medical.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;





namespace Medical.Data_Source_Layer.Module_3.P1_1.BloodGlucoseComponent
{
	// Repository for accessing BloodGlucoseAssessment data (RDG)
	public class BloodGlucoseAssessment_RDG
	{
		private readonly SmartHealthPlatformContext _context;

		// Constructor to initialize the repository with database context
		public BloodGlucoseAssessment_RDG(SmartHealthPlatformContext context)
		{
			_context = context;
		}



		// Finds BloodGlucoseAssessment by its ID
		public BloodGlucoseAssessment_SDM FindById(int id)
		{
			return _context.Set<BloodGlucoseAssessment_SDM>().Find(id);
		}





		// Finds BloodGlucoseAssessment by patient ID and timestamp
		public BloodGlucoseAssessment_SDM FindByPatientIdAndTimestamp(int patientId, DateTime timestamp)
		{
			return _context.Set<BloodGlucoseAssessment_SDM>()
						   .FirstOrDefault(a => a.PatientID == patientId && a.Timestamp == timestamp);
		}



		// Updates anBloodGlucoseAssessment
		public void Update(BloodGlucoseAssessment_SDM assessment)
		{
			_context.Entry(assessment).State = EntityState.Modified;
			_context.SaveChanges();
		}




		// Deletes an BloodGlucoseAssessment by its ID
		public void Delete(int id)
		{
			var assessment = FindById(id);
			if (assessment != null)
			{
				_context.Set<BloodGlucoseAssessment_SDM>().Remove(assessment);
				_context.SaveChanges();
			}
		}
	}
}
