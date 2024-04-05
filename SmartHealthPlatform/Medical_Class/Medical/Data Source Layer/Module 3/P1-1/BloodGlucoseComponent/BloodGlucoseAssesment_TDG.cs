
using Medical.Models;
using Microsoft.EntityFrameworkCore;
using Medical.Models.Module_3.P1_1.BloodGlucoseComponent;
using System;
using System.Collections.Generic;
using System.Linq;









namespace Medical.Data_Source_Layer.Module_3.P1_1.BloodGlucoseComponent
{
	// Repository for handling BloodGlucoseAssessment data (TDG)
	public class BloodGlucoseAssessment_TDG
	{
		private readonly SmartHealthPlatformContext _context;



		// Constructor to initialize the repository with database context
		public BloodGlucoseAssessment_TDG(SmartHealthPlatformContext context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}




		// Retrieves all BloodGlucoseAssessments
		public IEnumerable<BloodGlucoseAssessment_SDM> GetAll()
		{
			return _context.Set<BloodGlucoseAssessment_SDM>().AsNoTracking().ToList();
		}




		// Adds a new BloodGlucoseAssessment
		public void Add(BloodGlucoseAssessment_SDM assessment)
		{
			_context.Set<BloodGlucoseAssessment_SDM>().Add(assessment);
			_context.SaveChanges();
		}




		// Upserts (Adds or Updates) an BloodGlucoseAssessment
		public void Upsert(BloodGlucoseAssessment_SDM assessment)
		{
			if (assessment == null) throw new ArgumentNullException(nameof(assessment));

			if (_context == null) throw new InvalidOperationException("_context is not initialized.");

			// Attempt to find an existing assessment for the same patient
			var existingAssessment = _context.Set<BloodGlucoseAssessment_SDM>()
											  .FirstOrDefault(a => a.PatientID == assessment.PatientID && a.Timestamp == assessment.Timestamp);

			if (existingAssessment != null)
			{

				// If found, update the existing record
				existingAssessment.BloodGlucoseLevels = assessment.BloodGlucoseLevels;
				existingAssessment.RiskMessagesJson = assessment.RiskMessagesJson;
				// Update any other fields that are relevant

				_context.Entry(existingAssessment).State = EntityState.Modified;
			}
			else
			{
				// If not found, add a new record
				_context.Set<BloodGlucoseAssessment_SDM>().Add(assessment);
			}
			_context.SaveChanges();
		}



	}
}
