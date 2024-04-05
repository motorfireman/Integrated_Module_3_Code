using Microsoft.EntityFrameworkCore;
using Medical.Domain_Layer.Module_3.P1_2.Prescription;
using Medical.Domain_Layer.Module_3.Mock;

namespace Medical.Data_Source_Layer.Module_3.P1_2.Prescription
{
    public class PrescriptionTDG
    {
        private readonly SmartHealthPlatformContext _context;

        public PrescriptionTDG(SmartHealthPlatformContext context)
        {
            _context = context;
        }

        public PrescriptionEntity GetPrescriptionById(int prescriptionId)
        {
            return _context.Prescriptions.FirstOrDefault(p => p.Id == prescriptionId);
        }

        public List<PrescriptionEntity> GetPrescriptionsByPatientId(int patientId)
        {
            return _context.Prescriptions
                           .Where(p => p.UserId == patientId)
                           .ToList();
        }

        public List<PrescriptionEntity> GetByUserId(int userId)
        {
            return _context.Prescriptions
                           .Where(p => p.PrescribedBy == userId)
                           .ToList();
        }

        public void Insert(PrescriptionEntity prescription)
        {
            _context.Prescriptions.Add(prescription);
            _context.SaveChanges();
        }

        public void Update(PrescriptionEntity prescription)
        {
            _context.Prescriptions.Update(prescription);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var prescription = _context.Prescriptions.Find(id);
            if (prescription != null)
            {
                _context.Prescriptions.Remove(prescription);
                _context.SaveChanges();
            }
        }
    }
}
