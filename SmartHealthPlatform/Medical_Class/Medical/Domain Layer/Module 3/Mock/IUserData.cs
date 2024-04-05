namespace Medical.Domain_Layer.Module_3.Mock;

public interface IUserData
{
    public User? GetUser(int userId);
    public User? GetCurrentUser();
    public List<User> GetPatientsForMedicalPractitioner(int userId);
    public User GetMyMedicalPractitioner(int patientId);
}