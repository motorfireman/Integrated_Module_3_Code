using Medical.Domain_Layer.Module_3.P1_1.Interfaces;

namespace Medical.Domain_Layer.Module_3.P1_1.AirPulseOximeterComponent.Control
{
	public class IdealPerfusionIndexControl : IPerfusionIndex
	{
		// Define threshold values as constants
		private const double LowerNormalPI = 0.02; // 2% might be considered the lower normal bound in many cases
		private const double UpperNormalPI = 20.0; // 20% might be considered an upper normal bound

		public bool IsPerfusionIndexNormal(double perfusionIndex)
		{
			// Check if the perfusion index is within the normal range
			return perfusionIndex >= LowerNormalPI && perfusionIndex <= UpperNormalPI;
		}
	}
}