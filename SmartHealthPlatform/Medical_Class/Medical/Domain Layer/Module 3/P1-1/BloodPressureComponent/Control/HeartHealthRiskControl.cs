using System.Reflection;
using System.Runtime.Intrinsics.Arm;
using Medical.Domain_Layer.Module_3.P1_1.BloodPressureComponent.Interface;
using Medical.Domain_Layer.Module_3.P1_1.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Medical.Domain_Layer.Module_3.P1_1.BloodPressureComponent.Control
{
	public class HeartHealthRiskControl : IHeartHealthRiskSeverity
	{
		public int generateHHRiskSeverity(int age, string gender, float SBP, float DBP)
		{

			int BloodPressure = 0;

			if (gender == "Female")
			{
				BloodPressure += 1;
			}

			if (age >= 60)
			{
				BloodPressure += 2;
			}
			else if (59 <= age && age >= 40)
			{
				BloodPressure += 1;
			}

			if (SBP >= 140)
			{
				BloodPressure += 3;
			}
			else if (120 >= SBP && SBP <= 139)
			{
				BloodPressure += 2;
			}
			else if (SBP <= 90)
			{
				BloodPressure += 1;
			}
			else
			{
				BloodPressure += 0;
			}

			if (DBP >= 90)
			{
				BloodPressure += 3;
			}
			else if (80 >= DBP && DBP <= 89)
			{
				BloodPressure += 2;
			}
			else if (DBP < 60)
			{
				BloodPressure += 1;
			}
			else
			{
				BloodPressure += 0;
			}

			return BloodPressure;
		}
	}
}
