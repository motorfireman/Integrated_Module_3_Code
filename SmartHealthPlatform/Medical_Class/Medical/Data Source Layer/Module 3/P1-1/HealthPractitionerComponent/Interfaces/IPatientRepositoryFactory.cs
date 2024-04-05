namespace Medical.Data_Source_Layer.Module_3.P1_1.DataAccessLayerInterface
{


    /*
     * The factory pattern is used here to address the lifecycle mismatch between the PatientListControl and the IPatientRepository.
     * PatientListControl is designed as a singleton, meaning there's only one instance throughout the application's lifetime.
     * However, repository instances typically need to have a scoped lifetime, particularly when they work with a database context,
     * to ensure that database connections are properly managed and do not become stale or overused.
     *
     * By introducing a repository factory with a singleton lifetime, we can generate new repository instances with a scoped lifetime
     * whenever needed by the singleton service. This allows the PatientListControl to request fresh repository instances that are
     * scoped to an individual operation, thus avoiding the issues that would arise if a repository was held with a longer lifetime than its
     * associated database context.
     *
     * Essentially, the factory acts as a bridge, ensuring that each operation within the singleton service can interact with the database
     * using a repository that is correctly scoped for that operation, without violating the principles of dependency injection and
     * service lifetimes.
     */

    // Interface for the factory pattern implementation for creating repository instances.
    public interface IPatientRepositoryFactory
    {
        IPatientRepository CreateRepository();
    }
}
