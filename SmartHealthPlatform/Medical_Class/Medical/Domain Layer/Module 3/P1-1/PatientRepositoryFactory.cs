using Medical.Data_Source_Layer.Module_3.P1_1.DataAccessLayerInterface;

namespace Medical.Data_Source_Layer.Module_3.P1_1.Repository
{
	// Factory for creating instances of patient repositories
	public class PatientRepositoryFactory : IPatientRepositoryFactory
	{
		private readonly IServiceScopeFactory _scopeFactory;

		// Constructor to initialize the factory with service scope factory
		public PatientRepositoryFactory(IServiceScopeFactory scopeFactory)
		{
			_scopeFactory = scopeFactory;
		}

		// Creates a new instance of a patient repository
		public IPatientRepository CreateRepository()
		{
			var scope = _scopeFactory.CreateScope();
			return scope.ServiceProvider.GetRequiredService<IPatientRepository>();
		}
	}
}
