using Medical.Models.Module_3.P1_1.BloodPressureComponent;
using Microsoft.EntityFrameworkCore;
using Medical.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Medical.Models.Module_3.P1_1.AirPulseOximeterComponent;


namespace Medical.Data_Source_Layer.Module_3.P1_1.BloodPressureComponent
{
    // Repository for handling BloodPressureAssessment data (TDG)
    public class BloodPressureAssessment_TDG
    {
		private readonly SmartHealthPlatformContext _context;

		// Constructor to initialize the repository with database context
		public BloodPressureAssessment_TDG(SmartHealthPlatformContext context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}

        // Retrieves all BloodPressureAssessments
        public IEnumerable<BloodPressureAssessment_SDM> GetAll()
		{
			return _context.Set<BloodPressureAssessment_SDM>().AsNoTracking().ToList();
		}

		// Add new assessment
		public void Add(BloodPressureAssessment_SDM assessment)
		{
			_context.Set<BloodPressureAssessment_SDM>().Add(assessment);
			_context.SaveChanges();
		}

		public void Upsert(BloodPressureAssessment_SDM assessment)
		{
			if (assessment == null) throw new ArgumentNullException(nameof(assessment));

			if (_context == null) throw new InvalidOperationException("_context is not initialized.");

			// Attempt to find an existing assessment for the same patient
			var existingAssessment = _context.Set<BloodPressureAssessment_SDM>()
											  .FirstOrDefault(a => a.PatientID == assessment.PatientID && a.Timestamp == assessment.Timestamp);

			if (existingAssessment != null)
			{
				// If found, update the existing record
				existingAssessment.SystolicPressure = assessment.SystolicPressure;
				existingAssessment.DiastolicPressure = assessment.DiastolicPressure;
				existingAssessment.HeartHealthRiskSeverity = assessment.HeartHealthRiskSeverity;
				existingAssessment.HeartHealthRiskMessage = assessment.HeartHealthRiskMessage;
				existingAssessment.MAPRiskSeverity = assessment.MAPRiskSeverity;
				existingAssessment.MAPRiskMessage = assessment.MAPRiskMessage;
				// Update any other fields that are relevant

				_context.Entry(existingAssessment).State = EntityState.Modified;
			}
			else
			{
				// If not found, add a new record
				_context.Set<BloodPressureAssessment_SDM>().Add(assessment);
			}
			_context.SaveChanges();
		}

	}
}