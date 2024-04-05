using Medical.Domain_Layer.Module_3.P1_1.Interfaces;



namespace Medical.Domain_Layer.Module_3.P1_1.BloodGlucoseComponent.Control
{
	public class IdealBloodGlucoseControl : IBloodGlucose
	{
		public int GenerateIdealBloodGlucoseLevel(int age)
		{
			if (age < 18)
			{
				// Children and adolescents: Average range might be wider, from 90 to 130 mg/dL for children.
				// Opting for a midpoint.
				return 110;
			}
			else if (age >= 18 && age <= 65)
			{
				// Adults: A commonly cited healthy range is from 80 to 130 mg/dL.
				// Opting for the lower end to encourage fitness.
				return 105;
			}
			else
			{
				// Older adults: It's common for the blood glucose level to increase slightly with age.
				// Adjusting upwards slightly.
				return 130;
			}
		}

	}
}
