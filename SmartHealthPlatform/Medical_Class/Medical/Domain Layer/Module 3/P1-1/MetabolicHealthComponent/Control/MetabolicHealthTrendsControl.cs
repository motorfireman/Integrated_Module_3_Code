using Medical.Domain_Layer.Module_3.P1_1.Interfaces;
using Medical.Models.Module_3.P1_1.MetabolicHealthComponent;
using Medical.ViewModel.Module_3.P1_1.MetabolicHealthComponent;
using Medical.Domain_Layer.Module_3.P1_1.HealthPractitionerComponent;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Text;
using Medical.Data_Source_Layer.Module_3.P1_1.AirPulseOximeterComponent;
using Microsoft.EntityFrameworkCore;
using Medical.Data_Source_Layer.Module_3.P1_1.MetabolicHealthComponent;
using Medical.Data_Source_Layer;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Medical.Domain_Layer.Module_3.P1_1.MetabolicHealthComponent.Control
{
	public class MetabolicHealthTrendsControl : IMetabolicAnalysis
	{
		// Uses transformer (Acts as interface for getting readings from Module 2)
		private readonly TransformPatientListViewModel _transformer;

		// Uses DB context
		private readonly SmartHealthPlatformContext _context;

		// Uses IMetabolicAssessment which is implemented by MetabolicHealthAssessmentControl
		private readonly IMetabolicAssessment _metabolicAssessmentControl;

		// Uses IIterator which is implemented by MetablicAssessmentIterator
		private readonly IIterator<MetabolicAssessment_SDM> _metabolicAssessmentIterator;

		// Uses ICollectionIterable which is implemented by MetabolicAssessmentCollection
		private readonly ICollectionIterable<MetabolicAssessment_SDM> _metabolicAssessmentCollection;

		// Constructor injection
		public MetabolicHealthTrendsControl(TransformPatientListViewModel transformer, IMetabolicAssessment metabolicAssessmentControl, SmartHealthPlatformContext context)
		{
			_transformer = transformer;
			_metabolicAssessmentControl = metabolicAssessmentControl;
			_context = context;
		}


		// Implement GenerateMetabolicAnalysisSummary()
		public MetabolicHealthAnalysisSummary GenerateMetabolicAnalysisSummary(int patientId)
		{
			// Initialize Summary
			MetabolicHealthAnalysisSummary metabolicSummary = new MetabolicHealthAnalysisSummary();


			// Retrieve list of readings using patient ID
			var metabolicHealthData = _transformer.TransformToMetabolicHealthAnalysis(patientId);

			// Initialize MetabolicAssessments_TDG
			var metabolicAssessments_TDG = new MetabolicAssessments_TDG(_context);

			// Initialize list of assessments
			List<MetabolicAssessment_SDM> listOfMetabolicAssessments = new List<MetabolicAssessment_SDM>();

			// Instantiate assessment collection
			MetabolicAssessmentCollection metabolicAssessmentCollection = new MetabolicAssessmentCollection();

			// Generate an assessment for each reading
			foreach (MetabolicHealthAnalysisViewModel reading in metabolicHealthData) {

				// Generate Metabolic Health Assessment
				MetabolicAssessment_SDM metabolicAssessment = _metabolicAssessmentControl.GenerateMetabolicAssessment(reading);

				// Add Assessment to the list
				listOfMetabolicAssessments.Add(metabolicAssessment);

				// Add Assessment to Collection
				metabolicAssessmentCollection.AddAssessment(metabolicAssessment);

				// Create and prepare object to push to DB
				MetabolicHealthAssessment newMetabolicAssessment = new MetabolicHealthAssessment();

				// Create flat list of risks and recommendations
				List<string> riskList = new List<string>();
				List<string> recommendationList = new List<string>();

				foreach (MetabolicRisk_SDM metabolicRisk in metabolicAssessment.MetabolicRisks)
				{
					// Populate list of Risk and Risk Types E.g [risk, riskType, risk, riskType]
					riskList.Add(metabolicRisk.MetabolicRisk);
					riskList.Add(metabolicRisk.MetabolicRiskType);
				}

				foreach (MetabolicRecommendation_SDM recommendation in metabolicAssessment.MetabolicRecommndations)
				{
					// Populate list of Cardio, Protein, and Risk recommendations E.g [cardioRecc, proteinRecc, riskRecc]
					recommendationList.Add(recommendation.CardioFitnessRecommendation);
					recommendationList.Add(recommendation.ProteinIntakeRecommendation);
					recommendationList.Add(recommendation.RiskRecommendation);
				}

				// Serialize lists to string to store in DB
				string riskString = JsonSerializer.Serialize(riskList);
				string recommendationString = JsonSerializer.Serialize(recommendationList);

				newMetabolicAssessment.PatientID = metabolicAssessment.reading.PatientID;
				newMetabolicAssessment.PatientName = metabolicAssessment.reading.PatientName;
				newMetabolicAssessment.Age = metabolicAssessment.reading.Age;
				newMetabolicAssessment.Gender = metabolicAssessment.reading.Gender;
				newMetabolicAssessment.Timestamp = metabolicAssessment.reading.Timestamp;
				newMetabolicAssessment.Weight = metabolicAssessment.reading.Weight;
				newMetabolicAssessment.Height = metabolicAssessment.reading.Height;
				newMetabolicAssessment.BMR = metabolicAssessment.reading.BMR;
				newMetabolicAssessment.BMI = metabolicAssessment.reading.BMI;
				newMetabolicAssessment.BodyFatPercentage = metabolicAssessment.reading.BodyFatPercentage;
				newMetabolicAssessment.VisceralFatRating = metabolicAssessment.reading.VisceralFatRating;
				newMetabolicAssessment.Protein = metabolicAssessment.reading.Protein;
				newMetabolicAssessment.MetabolicRisks = riskString;
				newMetabolicAssessment.MetabolicRecommendations = recommendationString;

				// Insert Assessment to DB using TDG
				metabolicAssessments_TDG.Upsert(newMetabolicAssessment);

			}

			// Generate Summary based on list of assessments
			metabolicSummary.MetabolicAnalysisSummary = listOfMetabolicAssessments;
			metabolicSummary.MetabolicTrendsSummary = GenerateTrendsSummary(metabolicAssessmentCollection);

			return metabolicSummary;
		}


		// Implement RetrieveWeeklyMeasurements for displaying numbers
		public List<MetabolicHealthAnalysisViewModel> RetrieveWeeklyMeasurements(int patientId)
		{
			// Retrieve list of readings using patient ID
			var metabolicHealthData = _transformer.TransformToMetabolicHealthAnalysis(patientId);

			return metabolicHealthData;
		}


		// Method to generate trends summary by iterating through collection of assessments
		public string GenerateTrendsSummary(MetabolicAssessmentCollection collection)
		{
			StringBuilder summaryBuilder = new StringBuilder();

			int increasingRiskCount = 0;
			int decreasingRiskCount = 0;
			int stableRiskCount = 0;

			IIterator<MetabolicAssessment_SDM> iterator = collection.CreateIterator();

			string previousRiskLevel = "";

			while (!iterator.IsDone())
			{

				MetabolicAssessment_SDM assessment = iterator.Current();
				string currentRiskLevel = GetRiskLevel(assessment);

				if (currentRiskLevel == previousRiskLevel)
				{
					stableRiskCount++;
				}
				else if (IsRiskLevelHigher(currentRiskLevel, previousRiskLevel))
				{
					increasingRiskCount++;
				}
				else
				{
					decreasingRiskCount++;
				}

				previousRiskLevel = currentRiskLevel;
				iterator.Next();
			}

			summaryBuilder.AppendLine("Trends Summary:");
			summaryBuilder.AppendLine($"- Assessments with increasing risk: {increasingRiskCount}");
			summaryBuilder.AppendLine($"- Assessments with decreasing risk: {decreasingRiskCount}");
			summaryBuilder.AppendLine($"- Assessments with stable risk: {stableRiskCount}");

			// Add line breaks for better formatting
			string summary = summaryBuilder.ToString().Replace(Environment.NewLine, "<br />");

			return summary;
		}

		private string GetRiskLevel(MetabolicAssessment_SDM assessment)
		{
			// Assuming the risk level is the first element of MetabolicRisks
			return assessment?.MetabolicRisks?.Count > 0 ? assessment.MetabolicRisks[0].MetabolicRisk : "Unknown";
		}

		private bool IsRiskLevelHigher(string currentRiskLevel, string previousRiskLevel)
		{
			return currentRiskLevel == "High metabolic risk";
		}


	}

}
