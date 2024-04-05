using Medical.Domain_Layer.Module_3.P1_1.Interfaces;
using Medical.Models.Module_3.P1_1.MetabolicHealthComponent;
using Medical.ViewModel.Module_3.P1_1.MetabolicHealthComponent;

namespace Medical.Domain_Layer.Module_3.P1_1.MetabolicHealthComponent.Control
{
	public class MetabolicHealthRecommendationControl : IMetabolicRecommendation
	{

		// Implements GenerateMetabolicRecommendations()
		public List<MetabolicRecommendation_SDM> GenerateMetabolicRecommendations(List<MetabolicRisk_SDM> metabolicRisks, MetabolicHealthAnalysisViewModel reading)
		{
			// Initialize list of Recommendations
			List<MetabolicRecommendation_SDM> ListOfRecommendations = new List<MetabolicRecommendation_SDM>();

			// Generate a Recommendation for every Risk
			foreach (MetabolicRisk_SDM risk in metabolicRisks)
			{
				// Generate Recommendation
				MetabolicRecommendation_SDM recommendation = generateRecommendation(risk, reading);

				// Add to list of recommendations
				ListOfRecommendations.Add(recommendation);
			}

			return ListOfRecommendations;
		}


		private MetabolicRecommendation_SDM generateRecommendation(MetabolicRisk_SDM metabolicRisk, MetabolicHealthAnalysisViewModel reading)
		{
			// Generate a recommendation using the risk
			MetabolicRecommendation_SDM recommendation = new MetabolicRecommendation_SDM();

			int age = reading.Age;
			string gender = reading.Gender;
			double BMI = reading.BMI;
			double bodyFatPercentage = reading.BodyFatPercentage;
			double BMR = reading.BMR;

			double recommendedProteinIntake = CalculateProteinIntake(age, gender, BMR);

			// Generate recommendation
			recommendation.CardioFitnessRecommendation = GenerateCardioRecommendation(age, gender, BMI, bodyFatPercentage);
			recommendation.ProteinIntakeRecommendation = "Recommended Protein Intake: " + recommendedProteinIntake.ToString("0.0") + "g/day";
			recommendation.RiskRecommendation = GenerateRiskRecommendation(metabolicRisk);

			return recommendation;
		}

		private string GenerateCardioRecommendation(int age, string gender, double BMI, double bodyFatPercentage)
		{
			// Define thresholds for cardio fitness recommendation
			double bmiThreshold = 25.0; // BMI threshold for overweight
			double bodyFatThreshold = 25.0; // Body fat percentage threshold for overweight

			// Default recommendation message
			string recommendation = "";

			// Check BMI and body fat percentage using switch statements
			switch (true)
			{
				case var _ when BMI >= bmiThreshold && bodyFatPercentage >= bodyFatThreshold:
					recommendation = "Patient's BMI and body fat percentage indicate that they may benefit from increasing their cardio exercise to improve their overall fitness.";
					break;
				case var _ when BMI >= bmiThreshold:
					recommendation = "Patient's BMI indicates that they may benefit from increasing their cardio exercise to improve their overall fitness.";
					break;
				case var _ when bodyFatPercentage >= bodyFatThreshold:
					recommendation = "Patient's body fat percentage indicates that they may benefit from increasing their cardio exercise to improve their overall fitness.";
					break;
				default:
					recommendation = "Patient's BMI and body fat percentage are within healthy ranges. Continue with their current cardio exercise routine to maintain their fitness.";
					break;
			}

			return recommendation;
		}


		public double CalculateProteinIntake(int age, string gender, double BMR)
		{
			double proteinIntakeGoal = 0;

			if (age >= 19 && age <= 70)
			{
				// For adults aged 19-70, recommendations are:
				// - 0.8 grams/kg/day for men
				// - 0.8 grams/kg/day for women
				proteinIntakeGoal = (gender == "male") ? 0.8 * BMR : 0.8 * BMR;
			}
			else if (age > 70)
			{
				// For adults over 70, protein intake recommendations may be slightly higher:
				// - 1.0 grams/kg/day for men
				// - 1.0 grams/kg/day for women
				proteinIntakeGoal = (gender == "male") ? 1.0 * BMR : 1.0 * BMR;
			}
			// No specific recommendation for children or adolescents without activity level

			return proteinIntakeGoal;
		}



		private string GenerateRiskRecommendation(MetabolicRisk_SDM metabolicRisk)
		{
			// Initialize an empty recommendation message
			string recommendation = "";

			// Generate recommendation based on the provided metabolic risk
			switch (metabolicRisk.MetabolicRisk)
			{
				case "Low metabolic risk":
					recommendation = "Patient's metabolic risk is low. They should continue with healthy lifestyle habits.";
					break;
				case "High metabolic risk":
					recommendation = $"Patient's metabolic risk is high due to {metabolicRisk.MetabolicRiskType}. Monitor patient closely.";
					break;
				default:
					recommendation = "NA";
					break;
			}

			return recommendation;
		}

	}
}