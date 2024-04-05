using Medical.Domain_Layer.Module_3.P1_1.Interfaces;

namespace Medical.Domain_Layer.Module_3.P1_1.AirPulseOximeterComponent.Control
{
	public class IdealSpO2Control : ISpO2
	{
		public double GenerateIdealSpO2()
		{
			return 95.0;
		}
	}
}