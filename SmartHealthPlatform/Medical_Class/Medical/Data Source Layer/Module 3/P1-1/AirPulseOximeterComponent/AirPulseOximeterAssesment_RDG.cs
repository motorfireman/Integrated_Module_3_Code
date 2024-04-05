using Medical.Models.Module_3.P1_1.AirPulseOximeterComponent;
using Microsoft.EntityFrameworkCore;
using Medical.Models;
using System.Linq;

namespace Medical.Data_Source_Layer.Module_3.P1_1.AirPulseOximeterComponent
{
	// Repository for accessing AirPulseOximeterAssessment data (RDG)
	public class AirPulseOximeterAssessment_RDG
	{
		private readonly SmartHealthPlatformContext _context;

		// Constructor to initialize the repository with database context
		public AirPulseOximeterAssessment_RDG(SmartHealthPlatformContext context)
		{
			_context = context;
		}



		// Finds AirPulseOximeterAssessment by its ID
		public AirPulseOximeterAssessment_SDM FindById(int id)
		{
			return _context.Set<AirPulseOximeterAssessment_SDM>().Find(id);
		}





		// Finds AirPulseOximeterAssessment by patient ID and timestamp
		public AirPulseOximeterAssessment_SDM FindByPatientIdAndTimestamp(int patientId, DateTime timestamp)
		{
			return _context.Set<AirPulseOximeterAssessment_SDM>()
						   .FirstOrDefault(a => a.PatientID == patientId && a.Timestamp == timestamp);
		}



		// Updates an AirPulseOximeterAssessment
		public void Update(AirPulseOximeterAssessment_SDM assessment)
		{
			_context.Entry(assessment).State = EntityState.Modified;
			_context.SaveChanges();
		}




		// Deletes an AirPulseOximeterAssessment by its ID
		public void Delete(int id)
		{
			var assessment = FindById(id);
			if (assessment != null)
			{
				_context.Set<AirPulseOximeterAssessment_SDM>().Remove(assessment);
				_context.SaveChanges();
			}
		}
	}
}
