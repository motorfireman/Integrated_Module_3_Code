namespace Medical.Domain_Layer.Module_3.P1_1.BloodPressureComponent.Interface
{
	public interface IHeartHealthRiskSeverity
	{
		int generateHHRiskSeverity(int age, string gender, float SBP, float DBP);
	}
}
