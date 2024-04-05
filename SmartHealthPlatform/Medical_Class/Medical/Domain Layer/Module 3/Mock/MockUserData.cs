using Medical.Data_Source_Layer;

namespace Medical.Domain_Layer.Module_3.Mock;

public class MockUserData: IUserData
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly SmartHealthPlatformContext _db;

    public MockUserData(IHttpContextAccessor httpContextAccessor, SmartHealthPlatformContext db)
    {
        _httpContextAccessor = httpContextAccessor;
        _db = db;
    }
    
    private readonly Dictionary<int, User> _users = new()
    {
        { 201, new User(201, "Lim Chee Hean", "lychee0504@gmail.com", "Medical Practitioner") },
        { 202, new User(202, "Neo Lok Jun", "lokjunneo@gmail.com", "Medical Practitioner") },
        { 203, new User(203, "Russel Poon Wei Quan", "russel@smarthealth.spmovy.com", "Medical Practitioner")},
        { 204, new User(204, "Lau Yi Ling Celeste", "celeste@smarthealth.spmovy.com", "Medical Practitioner")},
        { 205, new User(205, "Kwok Sze Xuan Reina", "reina@smarthealth.spmovy.com", "Medical Practitioner")},
        { 206, new User(206, "Shu Yu Jian", "yujian@smarthealth.spmovy.com", "Medical Practitioner")},
        { 207, new User(207, "Jackson Ng", "jackson@smarthealth.spmovy.com", "Medical Practitioner")},
        { 208, new User(208, "Xen", "xen@smarthealth.spmovy.com", "Medical Practitioner")},
        { 209, new User(209, "Tiara", "tiara@smarthealth.spmovy.com", "Medical Practitioner")},
        { 210, new User(210, "Nicholas", "nicholas@smarthealth.spmovy.com", "Medical Practitioner")},
        { 211, new User(211, "Yu Rui", "yurui@smarthealth.spmovy.com", "Medical Practitioner")},
        { 212, new User(212, "Aaron", "aaron@smarthealth.spmovy.com", "Medical Practitioner")},
    };
    
    public User? GetUser(int userId)
    {
        var user = _users!.GetValueOrDefault(userId, null);
        
        // User found in list
        if (user != null)
            return user;
        
        // Find from db
        var patient = _db.P1_1PatientListData.FirstOrDefault(p => p.ID == userId);

        // User not found
        if (patient == null)
            return null;
        
        return new User(patient.ID, patient.Name, $"{patient.Name}@smarthealth.spmovy.com", "Patient");
    }

    public User? GetCurrentUser()
    {
        var userId = _httpContextAccessor.HttpContext?.Session.GetInt32("userId");

        return userId == null ? null : GetUser((int)userId);
    }

    public List<User> GetPatientsForMedicalPractitioner(int userId)
    {
        if (userId is < 201 or > 212)
            throw new Exception("User is not a medical practitioner");
        var patientCount = _db.P1_1PatientListData.Count();
        var medicalPractitionerCount = _users.Count;
        var patientsPerMedicalPractitioner = patientCount / medicalPractitionerCount;
        var patientStartingId = (userId - 201) * patientsPerMedicalPractitioner + 1;
        var patientEndingId = patientStartingId + patientsPerMedicalPractitioner - 1;
        return _db
            .P1_1PatientListData
            .Where(p => p.ID > patientStartingId && p.ID <= patientEndingId)
            .Select(p => new User(p.ID, p.Name, $"{p.Name}@smarthealth.spmovy.com", "Patient"))
            .ToList();
    }
    public User GetMyMedicalPractitioner(int patientId)
    {
        if (patientId is >= 201 and <= 212)
            throw new Exception("This isn't a patient's Id!");
        var patientCount = _db.P1_1PatientListData.Count();
        var medicalPractitionerCount = _users.Count;
        var patientsPerMedicalPractitioner = patientCount / medicalPractitionerCount;
        var patientStartingId = (patientId - 1) / patientsPerMedicalPractitioner * patientsPerMedicalPractitioner + 1;
        var patientEndingId = patientStartingId + patientsPerMedicalPractitioner - 1;
        var mpId = patientStartingId / patientsPerMedicalPractitioner + 201;

        return _users[mpId];
    }
}

