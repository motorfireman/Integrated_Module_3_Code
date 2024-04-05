using Medical.Domain_Layer.Module_3.P1_1.Interfaces;

namespace Medical.Domain_Layer.Module_3.P1_1.AirPulseOximeterComponent.Control
{
	public class IdealPulseRateControl : IPulseRate
	{
		public int GenerateIdealPulseRate(int age)
		{
			if (age < 18)
			{
				// Children and adolescents: Average range might be wider, from 70 to 100 bpm.
				// Opting for a midpoint.
				return 85;
			}
			else if (age >= 18 && age <= 65)
			{
				// Adults: A commonly cited healthy range is from 60 to 100 bpm.
				// Opting for the lower end to encourage fitness.
				return 60;
			}
			else
			{
				// Older adults: It's common for the resting heart rate to increase slightly with age.
				// Adjusting upwards slightly.
				return 70;
			}
		}

	}
}
