using Medical.Models;

namespace Medical.Data_Source_Layer.Module_3.P1_1.MetabolicHealthComponent
{
	public class MetabolicAssessment_RDG
	{
		private readonly SmartHealthPlatformContext _context;

		// Constructor to initialize the repository with database context
		public MetabolicAssessment_RDG(SmartHealthPlatformContext context)
		{
			_context = context;
		}
	}
}
