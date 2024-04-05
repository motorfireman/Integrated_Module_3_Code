using Microsoft.EntityFrameworkCore;
using Medical.Models;
using System.Linq;
using Medical.Models.Module_3.P1_1.BloodPressureComponent;
using Medical.Models.Module_3.P1_1.AirPulseOximeterComponent;


namespace Medical.Data_Source_Layer.Module_3.P1_1.BloodPressureComponent
{
    // Repository for accessing BloodPressureAssessment data (RDG)
    public class BloodPressureAssessment_RDG
	{
		private readonly SmartHealthPlatformContext _context;

		// Constructor to initialize the repository with database context
		public BloodPressureAssessment_RDG(SmartHealthPlatformContext context)
		{
			_context = context;
		}

		// Find Assessment by ID
		public BloodPressureAssessment_SDM FindById(int id)
		{
			return _context.Set<BloodPressureAssessment_SDM>().Find(id);
		}

		// Find Assesment by patient ID
		public BloodPressureAssessment_SDM FindByPatientIdAndTimestamp(int patientId)
		{
			return _context.Set<BloodPressureAssessment_SDM>()
				.FirstOrDefault(a => a.PatientID == patientId);
		}

		// Update a particular assesment
		public void Update(BloodPressureAssessment_SDM assessment)
		{
			_context.Entry(assessment).State = EntityState.Modified;
			_context.SaveChanges();
		}

		// Delete an BloodPressureAssessment by its ID
		public void Delete(int id)
		{
			var assessment = _context.Set<BloodPressureAssessment_SDM>().Find(id);
			if (assessment != null)
			{
				_context.Set<BloodPressureAssessment_SDM>().Remove(assessment);
				_context.SaveChanges();
			}
		}
	}
}
