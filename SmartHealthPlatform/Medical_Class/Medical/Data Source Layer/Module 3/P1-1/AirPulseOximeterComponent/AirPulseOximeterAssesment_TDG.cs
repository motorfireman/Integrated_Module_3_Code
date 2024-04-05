using Medical.Models.Module_3.P1_1.AirPulseOximeterComponent;
using Microsoft.EntityFrameworkCore;
using Medical.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Medical.Data_Source_Layer.Module_3.P1_1.AirPulseOximeterComponent
{
	// Repository for handling AirPulseOximeterAssessment data (TDG)
	public class AirPulseOximeterAssessment_TDG
	{
		private readonly SmartHealthPlatformContext _context;



		// Constructor to initialize the repository with database context
		public AirPulseOximeterAssessment_TDG(SmartHealthPlatformContext context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}




		// Retrieves all AirPulseOximeterAssessments
		public IEnumerable<AirPulseOximeterAssessment_SDM> GetAll()
		{
			return _context.Set<AirPulseOximeterAssessment_SDM>().AsNoTracking().ToList();
		}




		// Adds a new AirPulseOximeterAssessment
		public void Add(AirPulseOximeterAssessment_SDM assessment)
		{
			_context.Set<AirPulseOximeterAssessment_SDM>().Add(assessment);
			_context.SaveChanges();
		}




		// Upserts (Adds or Updates) an AirPulseOximeterAssessment
		public void Upsert(AirPulseOximeterAssessment_SDM assessment)
		{
			if (assessment == null) throw new ArgumentNullException(nameof(assessment));

			if (_context == null) throw new InvalidOperationException("_context is not initialized.");

			// Attempt to find an existing assessment for the same patient
			var existingAssessment = _context.Set<AirPulseOximeterAssessment_SDM>()
											  .FirstOrDefault(a => a.PatientID == assessment.PatientID && a.Timestamp == assessment.Timestamp);

			if (existingAssessment != null)
			{
				// If found, update the existing record
				existingAssessment.PerfusionIndex = assessment.PerfusionIndex;
				existingAssessment.PulseRate = assessment.PulseRate;
				existingAssessment.SpO2 = assessment.SpO2;
				existingAssessment.RiskMessagesJson = assessment.RiskMessagesJson;
				// Update any other fields that are relevant

				_context.Entry(existingAssessment).State = EntityState.Modified;
			}
			else
			{
				// If not found, add a new record
				_context.Set<AirPulseOximeterAssessment_SDM>().Add(assessment);
			}
			_context.SaveChanges();
		}
	}
}
