

using Medical.Domain_Layer.Module_3.P1_1.Interfaces;

namespace Medical.Domain_Layer.Module_3.P1_1.BloodGlucoseComponent.Control
{
	public class BloodGlucoseRiskControl: IBGRisk
	{
		public String GenerateBloodGlucoseRisks(double bloodGlucoseLevels)
		{
			if (bloodGlucoseLevels <70)
			{
				// Children and adolescents: Average range might be wider, from 90 to 130 mg/dL for children.
				// Opting for a midpoint.
				return "High risk of Hypoglycemia";
			}
			else if (bloodGlucoseLevels > 125)
			{
				// Adults: A commonly cited healthy range is from 80 to 130 mg/dL.
				// Opting for the lower end to encourage fitness.
				return "High risk of Hyperglycemia";
			}
			else
			{
				return "No risk";
			}
		}
	}
}
