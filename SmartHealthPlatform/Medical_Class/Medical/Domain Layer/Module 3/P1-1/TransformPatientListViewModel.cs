using Medical.ViewModel.Module_3.P1_1.AirPulseOximeterComponent;
using Medical.ViewModel.Module_3.P1_1.BloodGlucoseComponent;
using Medical.ViewModel.Module_3.P1_1.BloodPressureComponent;
using Medical.ViewModel.Module_3.P1_1.BoneHealthComponent;
using Medical.ViewModel.Module_3.P1_1.MetabolicHealthComponent;
using Mediqu.Domain.Services;

namespace Medical.Domain_Layer.Module_3.P1_1
{
	public class TransformPatientListViewModel
	{
		private readonly ILogger<TransformPatientListViewModel> _logger;
		private readonly PatientListControl _patientListControl;

		public TransformPatientListViewModel(ILogger<TransformPatientListViewModel> logger, PatientListControl patientListControl)
		{
			_logger = logger;
			_patientListControl = patientListControl;
		}

		public List<AirPulseOximeterAnalysisViewModel> TransformToAirPulseOximeterAnalysis(int patientId)
		{
			var patientData = _patientListControl.GetAllPatients()
				.SelectMany(plvm => plvm.SearchResults)
				.FirstOrDefault(p => p.ID == patientId);

			var airPulseOximeterAnalysisList = new List<AirPulseOximeterAnalysisViewModel>();

			if (patientData != null)
			{
				foreach (var deviceReading in patientData.DeviceReadings.Where(d => d.DeviceName == "Air Pulse Oximeter"))
				{
					var analysisViewModel = new AirPulseOximeterAnalysisViewModel
					{
						PatientID = patientData.ID,
						PatientName = patientData.Name,
						Age = patientData.Age,
						Gender = patientData.Gender,
						Timestamp = deviceReading.Timestamp,
						PerfusionIndex = deviceReading.ReadingValues.FirstOrDefault(rv => rv.Key == "Perfusion Index")?.Value ?? 0,
						PulseRate = deviceReading.ReadingValues.FirstOrDefault(rv => rv.Key == "Pulse rate")?.Value ?? 0,
						SpO2 = deviceReading.ReadingValues.FirstOrDefault(rv => rv.Key == "SP02")?.Value ?? 0,
					};

					_logger.LogInformation($"Transformed data for Patient ID: {patientData.ID}, Name: {patientData.Name}, Age: {patientData.Age}, Gender: {patientData.Gender}, Timestamp: {deviceReading.Timestamp}, Device: {deviceReading.DeviceName}, Perfusion Index: {analysisViewModel.PerfusionIndex}, Pulse Rate: {analysisViewModel.PulseRate}, SpO2: {analysisViewModel.SpO2}");


					airPulseOximeterAnalysisList.Add(analysisViewModel);
				}

				// After populating the list, log each item in the list
				LogList(airPulseOximeterAnalysisList);

			}
			else
			{
				_logger.LogWarning($"No patient found with ID: {patientId}");
			}

			return airPulseOximeterAnalysisList;
		
		
		}

		public List<BloodGlucoseAnalysisViewModel> TransformToBloodGluccoseAnalysis(int patientId)
		{
			var patientData = _patientListControl.GetAllPatients()
				.SelectMany(plvm => plvm.SearchResults)
				.FirstOrDefault(p => p.ID == patientId);

			var bloodGlucoseAnalysisList = new List<BloodGlucoseAnalysisViewModel>();


			_logger.LogInformation($"Total device readings found for Patient ID: {patientData.ID}: {patientData.DeviceReadings.Count}");

			if (patientData != null)
			{
				// Determine the most recent timestamp from the patient's readings
				var latestTimestamp = patientData.DeviceReadings
									   .Where(d => d.DeviceName == "Blood Glucose")
									   .OrderByDescending(d => d.Timestamp)
									   .FirstOrDefault()?.Timestamp;

				// If there's at least one reading, calculate seven days ago from the latest reading's date
				DateTime? sevenDaysAgo = latestTimestamp?.AddDays(-7);

				// Proceed only if we have a latest timestamp and hence a valid sevenDaysAgo date
				if (sevenDaysAgo.HasValue)
				{
					foreach (var deviceReading in patientData.DeviceReadings.Where(d => d.DeviceName == "Blood Glucose" && d.Timestamp >= sevenDaysAgo))
					{
						var analysisViewModel = new BloodGlucoseAnalysisViewModel
						{
							PatientID = patientData.ID,
							PatientName = patientData.Name,
							Age = patientData.Age,
							Gender = patientData.Gender,
							Timestamp = deviceReading.Timestamp,
							BloodGlucoseLevels = deviceReading.ReadingValues.FirstOrDefault(rv => rv.Key == "blood glucose levels")?.Value ?? 0
						};

						_logger.LogInformation($"Transformed data for Patient ID: {patientData.ID}, Name: {patientData.Name}, Age: {patientData.Age}, Gender: {patientData.Gender}, Timestamp: {deviceReading.Timestamp}, Device: {deviceReading.DeviceName}, Blood Glucose Levels: {analysisViewModel.BloodGlucoseLevels}");

						bloodGlucoseAnalysisList.Add(analysisViewModel);
					}
				}
				else
				{
					_logger.LogWarning($"No recent 'Blood Glucose' readings found for Patient ID: {patientId}.");
				}
			}
			return bloodGlucoseAnalysisList;
		}


		// add bloodpressure part here
		public List<BloodPressureAnalysisViewModel> TransformToBloodPressureAnalysis(int patientId)
		{
			var patientData = _patientListControl.GetAllPatients()
				.SelectMany(plvm => plvm.SearchResults)
				.FirstOrDefault(p => p.ID == patientId);

			var BloodPressureAnalysisList = new List<BloodPressureAnalysisViewModel>();


			if (patientData != null)
			{

				_logger.LogInformation($"Processing Blood Pressure for Patient ID: {patientId}"); // Safe logging
				_logger.LogDebug($"Found {patientData.DeviceReadings.Count()} device readings for Patient ID: {patientId}"); // Example of safe detailed logging


				foreach (var deviceReading in patientData.DeviceReadings.Where(d => d.DeviceName == "Blood Pressure"))
				{
					var analysisViewModel = new BloodPressureAnalysisViewModel
					{
						PatientID = patientData.ID,
						PatientName = patientData.Name,
						Age = patientData.Age,
						Gender = patientData.Gender,
						Timestamp = deviceReading.Timestamp,
						SystolicPressure = (float)(deviceReading.ReadingValues.FirstOrDefault(rv => rv.Key == "systolic")?.Value ?? 0.0f),
						DiastolicPressure = (float)(deviceReading.ReadingValues.FirstOrDefault(rv => rv.Key == "diastolic")?.Value ?? 0.0f),
					};

					BloodPressureAnalysisList.Add(analysisViewModel);
				}

				return BloodPressureAnalysisList;
			}
			else
			{
				_logger.LogWarning($"No patient found with ID: {patientId}");
				return new List<BloodPressureAnalysisViewModel>(); // Return an empty list if no patient data is found
			}
		}

		// Method to Calculate BMR
		static double CalculateBMR(string gender, double weightKg, int ageYears, int heightCm)
		{
			if (gender.ToLower() == "male")
			{
				return 66.5 + (13.75 * weightKg) + (5.003 * heightCm) - (6.755 * ageYears);
			}
			else if (gender.ToLower() == "female")
			{
				return 655.1 + (9.563 * weightKg) + (1.850 * heightCm) - (4.676 * ageYears);
			}
			else
			{
				throw new ArgumentException("Invalid gender specified. Please provide 'male' or 'female'.");
			}
		}

		// Method to Calculate BMI
		static double CalculateBMI(double weightKg, int heightCm)
		{
			double heightM = heightCm / 100.0; // converting height to meters
			return weightKg / (heightM * heightM);
		}

		static int GetRandomHeight()
		{
			Random random = new Random();
			// Generating a random height between 140 and 190 cm
			return random.Next(140, 191);
		}



		// Method to transform Metabolic Health readings from DB using patient ID
		public List<MetabolicHealthAnalysisViewModel> TransformToMetabolicHealthAnalysis(int patientId)
		{
			var patientData = _patientListControl.GetAllPatients()
				.SelectMany(plvm => plvm.SearchResults)
				.FirstOrDefault(p => p.ID == patientId);

			var MetabolicHealthAnalysisList = new List<MetabolicHealthAnalysisViewModel>();


			if (patientData != null)
			{

				foreach (var deviceReading in patientData.DeviceReadings.Where(d => d.DeviceName == "Body Composition"))
				{
					// Temporary variables
					var weight = deviceReading.ReadingValues.FirstOrDefault(rv => rv.Key == "Weight")?.Value ?? 0;
					var height = GetRandomHeight();

					// Calculate BMR
					var BMR = CalculateBMR(patientData.Gender, weight, patientData.Age, height);
					var BMI = CalculateBMI(weight, height);

					var analysisViewModel = new MetabolicHealthAnalysisViewModel
					{
						PatientID = patientData.ID,
						PatientName = patientData.Name,
						Age = patientData.Age,
						Gender = patientData.Gender,
						Timestamp = deviceReading.Timestamp,
						Weight = weight,
						Height = height,
						BMR = BMR,
						BMI = BMI,
						BodyFatPercentage = deviceReading.ReadingValues.FirstOrDefault(rv => rv.Key == "Body Fat Percentage")?.Value ?? 0,
						VisceralFatRating = deviceReading.ReadingValues.FirstOrDefault(rv => rv.Key == "Visceral Fat Rating")?.Value ?? 0,
						Protein = deviceReading.ReadingValues.FirstOrDefault(rv => rv.Key == "Protein")?.Value ?? 0
					};

					MetabolicHealthAnalysisList.Add(analysisViewModel);
				}

				return MetabolicHealthAnalysisList;
			}
			else
			{
				_logger.LogWarning($"No patient found with ID: {patientId}");
				return new List<MetabolicHealthAnalysisViewModel>(); // Return an empty list if no patient data is found
			}
		}

		public List<BoneHealthAnalysisViewModel> TransformToBoneHealthAnalysis(int patientId)
		{
			var patientData = _patientListControl.GetAllPatients()
				.SelectMany(plvm => plvm.SearchResults)
				.FirstOrDefault(p => p.ID == patientId);

			var boneHealthAnalysisList = new List<BoneHealthAnalysisViewModel>();

			if (patientData != null)
			{
				foreach (var deviceReading in patientData.DeviceReadings.Where(d => d.DeviceName == "Body Composition"))
				{
					var analysisViewModel = new BoneHealthAnalysisViewModel
					{
						PatientID = patientData.ID,
						PatientName = patientData.Name,
						Age = patientData.Age,
						Gender = patientData.Gender,
						Timestamp = deviceReading.Timestamp,
						Weight = deviceReading.ReadingValues.FirstOrDefault(rv => rv.Key == "Weight")?.Value ?? 0,
						BoneMass = deviceReading.ReadingValues.FirstOrDefault(rv => rv.Key == "Bone Mass")?.Value ?? 0,
						BodyFatPercentage = deviceReading.ReadingValues.FirstOrDefault(rv => rv.Key == "Body Fat Percentage")?.Value ?? 0,
						LeanMass = deviceReading.ReadingValues.FirstOrDefault(rv => rv.Key == "Lean Mass")?.Value ?? 0,
						Protein = deviceReading.ReadingValues.FirstOrDefault(rv => rv.Key == "Protein")?.Value ?? 0,
						VisceralFatRating = deviceReading.ReadingValues.FirstOrDefault(rv => rv.Key == "Visceral Fat Rating")?.Value ?? 0
					};

					_logger.LogInformation($"Transformed data for Patient ID: {patientData.ID}, Name: {patientData.Name}, Age: {patientData.Age}, Gender: {patientData.Gender}, Timestamp: {deviceReading.Timestamp}, Device: {deviceReading.DeviceName}, " +
						$"Weight: {analysisViewModel.Weight}, Bone Mass: {analysisViewModel.BoneMass}, Body Fat Percentage: {analysisViewModel.BodyFatPercentage}," +
						$" Lean Mass: {analysisViewModel.LeanMass}, Protein: {analysisViewModel.Protein}, Visceral Fat Rating: {analysisViewModel.VisceralFatRating}");


					boneHealthAnalysisList.Add(analysisViewModel);
				}

				// After populating the list, log each item in the list
				//LogList(boneHealthAnalysisList);

			}
			else
			{
				_logger.LogWarning($"No patient found with ID: {patientId}");
			}

			return boneHealthAnalysisList;


		}




















		/*----------------------------------------helper loggin function---------------------*/
		private void LogList(List<AirPulseOximeterAnalysisViewModel> list)
		{
			foreach (var item in list)
			{
				_logger.LogInformation($"List item: {ItemToString(item)}");
			}
		}

		private void LogBGList(List<BloodGlucoseAnalysisViewModel> list)
		{
			foreach (var item in list)
			{
				_logger.LogInformation($"List item: {ItemToBGString(item)}");
			}
		}
		private string ItemToBGString(BloodGlucoseAnalysisViewModel item)
		{
			return $"Patient ID: {item.PatientID}, Name: {item.PatientName}, Age: {item.Age}, Gender: {item.Gender}, Timestamp: {item.Timestamp}, BloodGlucoseLevels: {item.BloodGlucoseLevels}";
		}

		private string ItemToString(AirPulseOximeterAnalysisViewModel item)
		{
			return $"Patient ID: {item.PatientID}, Name: {item.PatientName}, Age: {item.Age}, Gender: {item.Gender}, Timestamp: {item.Timestamp}, Perfusion Index: {item.PerfusionIndex}, Pulse Rate: {item.PulseRate}, SpO2: {item.SpO2}";
		}



	}
}
