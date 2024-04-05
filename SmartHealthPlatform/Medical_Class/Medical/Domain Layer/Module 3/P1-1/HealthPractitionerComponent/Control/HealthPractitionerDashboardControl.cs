using Medical.Data_Source_Layer;
using Medical.Domain_Layer.Module_3.P1_1;
using Medical.Domain_Layer.Module_3.P1_1.BloodPressureComponent.Interface;
using Medical.Domain_Layer.Module_3.P1_1.BoneHealthComponent.Interfaces;
using Medical.Domain_Layer.Module_3.P1_1.Interfaces;
using Medical.Models;
using Medical.ViewModel.Module_3.P1_1;
using System.Text.Json;

namespace Mediqu.Domain.Services
{
	public class HealthPractitionerDashboardControl
	{
		private readonly List<PatientListViewModel> _allPatients;
		private readonly IRetrieveData _retrieveData;
		private readonly IAPOAnalysis _airpulse;
		private readonly IBGAnalysis _bg;
		private readonly IBPAnalysis _bp;
		private readonly IBoneHealthAnalysis _bh;
		private readonly IMetabolicAssessment _mb;


		private readonly SmartHealthPlatformContext _context;

		public HealthPractitionerDashboardControl(SmartHealthPlatformContext context, PatientListControl patientListService, IRetrieveData retrieveDataControl, IAPOAnalysis airpulsecontrol, IBGAnalysis bgviewmodel, IBPAnalysis bpviewmodel, IBoneHealthAnalysis bhviewmodel, IMetabolicAssessment metabolicviewmodel)
		{
			_context = context;
			_allPatients = patientListService.GetAllPatients(); // get all patient data
			_retrieveData = retrieveDataControl;
			_airpulse = airpulsecontrol;
			_bg = bgviewmodel;
			_bp = bpviewmodel;
			_bh = bhviewmodel;
			_mb = metabolicviewmodel;
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

					// Initialize the new HealthPractitionerDashboardViewModel with patient info and calculated values
					var dashboardViewModel = new HealthPractitionerDashboardViewModel
					{
						Patient = patient,
						ActiveDeviceCount = activeDeviceCount,
						LastVisitDate = lastVisitDate,
					};


					// Attempt to find the reading for all device for dashboard
					_airpulse.PopulateLatestAirPulseOximeterData(userId, ref dashboardViewModel);

					_bg.PopulateLatestBloodGlucoseData(userId, ref dashboardViewModel);

					_bp.PopulateLatestBloodPressureData(userId, ref dashboardViewModel);

					_bh.PopulateLatestBoneHealthData(userId, ref dashboardViewModel);

					_mb.PopulateLatestMetabolicData(userId, ref dashboardViewModel);


					return dashboardViewModel;
				}
			}

			// If no patient with the given ID was found, return null
			return null;
		}





	}
}
