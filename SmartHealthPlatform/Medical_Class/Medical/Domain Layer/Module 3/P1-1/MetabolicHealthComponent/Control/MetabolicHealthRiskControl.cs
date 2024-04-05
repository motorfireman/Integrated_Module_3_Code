using Medical.Domain_Layer.Module_3.P1_1.Interfaces;
using Medical.Models.Module_3.P1_1.MetabolicHealthComponent;
using Medical.ViewModel.Module_3.P1_1.MetabolicHealthComponent;

namespace Medical.Domain_Layer.Module_3.P1_1.MetabolicHealthComponent.Control
{
	public class MetabolicHealthRiskControl : IMetabolicRisk
	{

		// Implement GenerateMetabolicRisks()
		public List<MetabolicRisk_SDM> GenerateMetabolicRisks(MetabolicHealthAnalysisViewModel reading)
		{
			// Initialize list of Risks
			List<MetabolicRisk_SDM> ListOfRisks = new List<MetabolicRisk_SDM>();

			int metabolicAge = AssessMetabolicAge(reading.BMR, reading.Age);

			var metabolicRisk = AssessMetabolicRisk(reading.BMI, reading.BodyFatPercentage, reading.VisceralFatRating, metabolicAge);


			// Generate Risk
			MetabolicRisk_SDM risk = new MetabolicRisk_SDM();
			risk.MetabolicRisk = metabolicRisk.riskLevel;
			risk.MetabolicRiskType = metabolicRisk.riskType;

			// Add Risk to list
			ListOfRisks.Add(risk);

			// Return list of risks
			return ListOfRisks;
		}


		public int AssessMetabolicAge(double BMR, int age)
		{
			// Average BMR values for different age groups
			double[] averageBMRByAgeGroup = { 1500, 1600, 1650, 1700, 1750 };

			// Assuming the age groups are evenly distributed
			int ageGroup = Math.Max(0, Math.Min(age / 10, averageBMRByAgeGroup.Length - 1));

			double averageBMR = averageBMRByAgeGroup[ageGroup];

			// Comparing individual's BMR with the average BMR of their age group
			double metabolicAgeRatio = BMR / averageBMR;

			// Determining metabolic age based on the ratio
			int metabolicAge = (int)Math.Round(age * metabolicAgeRatio);

			return metabolicAge;
		}


		public (string riskLevel, string riskType) AssessMetabolicRisk(double BMI, double bodyFat, double visceralFatRating, int metabolicAge)
		{
			// Define thresholds for metabolic risk assessment
			double bmiThreshold = 25.0;			// BMI threshold for overweight
			double bodyFatThreshold = 25.0;		// Body fat percentage threshold for overweight
			double visceralFatThreshold = 10.0; // Visceral fat rating threshold for high risk
			int metabolicAgeThreshold = 45;		// Metabolic age threshold for high risk

			// Default risk level and risk type
			string riskLevel = "Low metabolic risk";
			string riskType = "None";

			// Check metabolic risk factors using switch statements
			switch (true)
			{
				case var _ when BMI >= bmiThreshold:
					riskLevel = "High metabolic risk";
					riskType = "Overweight";
					break;
				case var _ when bodyFat >= bodyFatThreshold:
					riskLevel = "High metabolic risk";
					riskType = "High Body Fat";
					break;
				case var _ when visceralFatRating >= visceralFatThreshold:
					riskLevel = "High metabolic risk";
					riskType = "High Visceral Fat";
					break;
				case var _ when metabolicAge >= metabolicAgeThreshold:
					riskLevel = "High metabolic risk";
					riskType = "Elevated Metabolic Age";
					break;
				default:
					break;
			}

			return (riskLevel, riskType);
		}



	}
}