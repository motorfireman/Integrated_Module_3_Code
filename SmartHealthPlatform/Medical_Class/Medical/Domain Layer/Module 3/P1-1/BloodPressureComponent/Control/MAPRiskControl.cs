using Medical.Domain_Layer.Module_3.P1_1.BloodPressureComponent.Interface;

namespace Medical.Domain_Layer.Module_3.P1_1.BloodPressureComponent.Control
{
	public class MAPRiskControl: IMAPRiskSeverity
	{
		public float generateMAPRiskSeverity(float SBP, float DBP)
		{
			return (SBP + (2.0f * DBP)) / 3.0f;
		}
	}
}
