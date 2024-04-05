using Medical.Data_Source_Layer;
using Medical.Domain_Layer.Module_3.P1_1;
using Medical.Models;
using Medical.ViewModel.Module_3.P1_1;
using System.Text.Json;

namespace Mediqu.Domain.Services
{
	public class HealthPractitionerDashboardControl
	{
		private readonly List<PatientListViewModel> _allPatients;
		private readonly IRetrieveData _retrieveData;
		private readonly SmartHealthPlatformContext _context;

		public HealthPractitionerDashboardControl(SmartHealthPlatformContext context, PatientListControl patientListService, IRetrieveData retrieveDataControl)
		{
			_context = context;
			_allPatients = patientListService.GetAllPatients(); // get all patient data
			_retrieveData = retrieveDataControl;
		}


		// With all patient data, can specify which patient data via ID
		public HealthPractitionerDashboardViewModel FetchDashboardDataById(int userId)
		{
			foreach (var patientListViewModel in _allPatients)
			{
				// Find the patient matching the given userId
				var patient = patientListViewModel.SearchResults.FirstOrDefault(p => p.ID == userId);
				if (patient != null)
				{
					// Calculate the distinct device names for ActiveDeviceCount
					int activeDeviceCount = patient.DeviceReadings.Select(dr => dr.DeviceName).Distinct().Count();

					// Find the most recent device reading timestamp for LastVisitDate
					var lastVisitDate = patient.DeviceReadings.MaxBy(dr => dr.Timestamp)?.Timestamp;

					// Fetch the latest 24 blood glucose readings from the "Blood Glucose" device
					var bloodGlucoseReadings = patient.DeviceReadings
						.Where(r => r.DeviceName == "Blood Glucose")
						.OrderByDescending(r => r.Timestamp)
						.Take(24)
						.SelectMany(r => r.ReadingValues.Where(rv => rv.Key == "blood glucose levels"))
						.Select(rv => rv.Value)
						.ToList();

					// Attempt to find the latest reading for "Air Pulse Oximeter"
					var latestReading = patient.DeviceReadings
						.Where(r => r.DeviceName == "Air Pulse Oximeter")
						.OrderByDescending(r => r.Timestamp)
						.FirstOrDefault();

					// Attempt to find the latest reading for "Bone Health"
					var latestBHReading = patient.DeviceReadings
						.Where(r => r.DeviceName == "Body Composition")
						.OrderByDescending(r => r.Timestamp)
						.FirstOrDefault();

					// Attempt to find the latest reading for "Blood Pressure Oximeter"
					var BPReadings = patient.DeviceReadings
						.Where(r => r.DeviceName == "Blood Pressure")
						.OrderByDescending(r => r.Timestamp)
						.FirstOrDefault();

					// Attempt to find the latest reading for "Blood Pressure Oximeter"
					var latestMetabolicReadings = patient.DeviceReadings
						.Where(r => r.DeviceName == "Body Composition")
						.OrderByDescending(r => r.Timestamp)
						.FirstOrDefault();

					// Initialize the new HealthPractitionerDashboardViewModel with patient info and calculated values
					var dashboardViewModel = new HealthPractitionerDashboardViewModel
					{
						Patient = patient,
						ActiveDeviceCount = activeDeviceCount,
						LastVisitDate = lastVisitDate,
					};

					// Populate latest reading in the dashboard view model for airpulse
					if (latestReading != null)
					{
						dashboardViewModel.LatestPerfusionIndex = latestReading.ReadingValues.FirstOrDefault(r => r.Key == "Perfusion Index")?.Value ?? 0;
						dashboardViewModel.LatestPulseRate = (int)(latestReading.ReadingValues.FirstOrDefault(r => r.Key == "Pulse rate")?.Value ?? 0);
						dashboardViewModel.LatestSpO2 = latestReading.ReadingValues.FirstOrDefault(r => r.Key == "SP02")?.Value ?? 0;
					}

					// Populate latest reading in the dashboard view model for airpulse
					if (latestBHReading != null)
					{
						dashboardViewModel.LatestBoneMass = latestBHReading.ReadingValues.FirstOrDefault(r => r.Key == "Bone Mass")?.Value ?? 0;
						dashboardViewModel.LatestLeanMass = latestBHReading.ReadingValues.FirstOrDefault(r => r.Key == "Lean Mass")?.Value ?? 0;
						dashboardViewModel.LatestProtein = latestBHReading.ReadingValues.FirstOrDefault(r => r.Key == "Protein")?.Value ?? 0;
					}

					// Populate latest reading in the dashboard view model for blood glucose

					if (bloodGlucoseReadings.Any())
					{
						dashboardViewModel.LatestMinBloodGlucose = bloodGlucoseReadings.Min();
						dashboardViewModel.LatestMaxBloodGlucose = bloodGlucoseReadings.Max();
						dashboardViewModel.LatestAvgBloodGlucose = bloodGlucoseReadings.Average();
					}

					// Populate latest reading in the dashboard view model for Blood Pressure

					if (BPReadings != null)
					{
						dashboardViewModel.LatestSystolic = (float)(BPReadings.ReadingValues.FirstOrDefault(rv => rv.Key == "systolic")?.Value ?? 0.0f);
						dashboardViewModel.LatestDiastolic = (float)(BPReadings.ReadingValues.FirstOrDefault(r => r.Key == "diastolic")?.Value ?? 0.0f);
					}

					// Populate latest reading in the dashboard view model for Blood Pressure

					if (latestMetabolicReadings != null)
					{
						dashboardViewModel.LatestWeight = (float)(latestMetabolicReadings.ReadingValues.FirstOrDefault(rv => rv.Key == "Weight")?.Value ?? 0.0f);
						dashboardViewModel.LatestBodyFatPercentage = (float)(latestMetabolicReadings.ReadingValues.FirstOrDefault(r => r.Key == "Body Fat Percentage")?.Value ?? 0.0f);
						dashboardViewModel.CurrentRisk = "Low Metabolic Risk";
					}

					// Continue here to populate whatever you want to show.


					return dashboardViewModel;
				}
			}

			// If no patient with the given ID was found, return null
			return null;
		}





	}
}
