using Medical.Data_Source_Layer.Module_3.P1_2.Prescription;
using Medical.Domain_Layer.Module_3.Mock;

namespace Medical.Domain_Layer.Module_3.P1_2.Prescription
{
    public class PrescriptionSDM
    {
        private readonly PrescriptionTDG _prescriptionTDG;
        private readonly IUserData _userData;

        public PrescriptionSDM(IUserData userData, PrescriptionTDG prescriptionTDG)
        {
            _userData = userData;
            _prescriptionTDG = prescriptionTDG;
        }

        public PrescriptionEntity GetPrescriptionById(int prescriptionId)
        {
            return _prescriptionTDG.GetPrescriptionById(prescriptionId);
        }

        public List<PrescriptionEntity> GetPrescriptionsByPatientId(int patientId)
        {
            return _prescriptionTDG.GetPrescriptionsByPatientId(patientId);
        }

        public List<PrescriptionEntity> GetPrescriptions(int userId)
        {
            return _prescriptionTDG.GetByUserId(userId);
        }

        public void AddPrescription(PrescriptionEntity prescription)
        {
            _prescriptionTDG.Insert(prescription);
        }

        public void EditPrescription(PrescriptionEntity prescription)
        {
            _prescriptionTDG.Update(prescription);
        }

        public void DeletePrescription(int id)
        {
            _prescriptionTDG.Delete(id);
        }

        public void OrderPrescriptions(List<int> ids)
        {
            foreach (var id in ids)
            {
                var prescription = _prescriptionTDG.GetByUserId(id).FirstOrDefault(p => p.Id == id);
                if (prescription != null)
                {
                    // Placeholder for ordering logic
                    Console.WriteLine($"Ordering prescription with ID: {prescription.Id}");
                }
            }
        }

        // Simulated retrieval of getting user details from Module 1
        public Dictionary<string, object> GetUserDetails()
        {
            var user = _userData.GetCurrentUser();

            // User not logged in
            if (user == null)
            {
                return new Dictionary<string, object>
                {
                    { "status", "unauthorised" }
                };
            }

            if (user.Role == "Medical Practitioner")
            {
                return new Dictionary<string, object>
                {
                    { "user_relationship", _userData.GetPatientsForMedicalPractitioner(user.Id) },
                    { "user", user }
                };
            }
            else
            {
                return new Dictionary<string, object>
                {
                    { "user_relationship", _userData.GetMyMedicalPractitioner(user.Id) },
                    { "user", user }
                };
            }
        }
    }
}
