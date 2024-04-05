using Medical.Domain_Layer.Module_3.Mock;
using Medical.Domain_Layer.Module_3.P1_2.Chat;
using Medical.Domain_Layer.Module_3.P1_2.Prescription;
using Microsoft.AspNetCore.Mvc;

namespace Medical.Controllers
{
    public class PrescriptionController : Controller
    {
        private readonly PrescriptionSDM _prescriptionSDM;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PrescriptionController(IHttpContextAccessor httpContextAccessor, PrescriptionSDM prescriptionSDM)
        {
            _httpContextAccessor = httpContextAccessor;
            _prescriptionSDM = prescriptionSDM;
        }

        // GET: Prescription
        public IActionResult Index()
        {
            return View("~/Views/Presentation Layer/Module 3/P1-2/Prescription/Index.cshtml");
        }

        public IActionResult GetPrescriptionsById(int id)
        {
            var prescription = _prescriptionSDM.GetPrescriptions(id);
            if (prescription == null)
            {
                return NotFound();
            }
            return Ok(prescription);
        }

        public IActionResult GetPrescriptionsByPatientId(int id)
        {
            var prescription = _prescriptionSDM.GetPrescriptionsByPatientId(id);
            if (prescription == null)
            {
                return NotFound();
            }
            return Ok(prescription);
        }

        // GET: Prescription/Details/5
        public IActionResult Details(int id)
        {
            var prescription = _prescriptionSDM.GetPrescriptions(0).FirstOrDefault(p => p.Id == id);
            if (prescription == null)
            {
                return NotFound();
            }
            return View("~/Views/Presentation Layer/Module 3/P1-2/Prescription/Details.cshtml", prescription);
        }

        // GET: Prescription/Create
        public IActionResult Create()
        {
            var prescribers = _prescriptionSDM.GetUserDetails();
            var patients = (IEnumerable<User>)prescribers["user_relationship"];
            var patientIds = new List<int>();
            var patientNames = new List<string>();

            foreach (var patient in patients)
            {
                patientIds.Add(patient.Id);
                patientNames.Add(patient.Name);
            }

            ViewBag.PatientIds = patientIds;
            ViewBag.PatientNames = patientNames;

            return View("~/Views/Presentation Layer/Module 3/P1-2/Prescription/Create.cshtml");
        }

        // POST: Prescription/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PrescriptionEntity prescription)
        {
            var userId = _httpContextAccessor.HttpContext?.Session.GetInt32("userId");

            if (ModelState.IsValid)
            {
                prescription.PrescribedBy = userId.Value;
                prescription.PrescribedOn = DateTime.Now;
                _prescriptionSDM.AddPrescription(prescription);
                return RedirectToAction(nameof(Index), new { userId = userId });
            }
            return View(prescription);
        }

        // GET: Prescription/Edit/5
        public IActionResult Edit(int id)
        {
            var prescription = _prescriptionSDM.GetPrescriptionById(id);
            if (prescription == null)
            {
                return NotFound();
            }
            return View("~/Views/Presentation Layer/Module 3/P1-2/Prescription/Edit.cshtml", prescription);
        }

        // POST: Prescription/Edit/5
        [HttpPost]
        public IActionResult Edit(int id, string medicationName, int quantity, int unit)
        {
            var existingPrescription = _prescriptionSDM.GetPrescriptionById(id);

            existingPrescription.MedicationName = medicationName;
            existingPrescription.Quantity = quantity;
            existingPrescription.Unit = unit;

            if (ModelState.IsValid)
            {
                var userId = _httpContextAccessor.HttpContext?.Session.GetInt32("userId");
                _prescriptionSDM.EditPrescription(existingPrescription);
                return RedirectToAction(nameof(Index), new { userId = existingPrescription.PrescribedBy });
            }
            return View("~/Views/Presentation Layer/Module 3/P1-2/Prescription/Index.cshtml", existingPrescription);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _prescriptionSDM.DeletePrescription(id);
            return RedirectToAction("Index");
        }



        // Custom method to handle ordering prescriptions
        public IActionResult OrderPrescriptions(List<int> ids)
        {
            _prescriptionSDM.OrderPrescriptions(ids);
            // Redirect to the list of prescriptions, adjust as needed
            return RedirectToAction(nameof(Index));
        }

        public IActionResult GetUserDetails()
        {
            // Call the method in domain layer to get the user details
            var result = _prescriptionSDM.GetUserDetails();
            return Ok(result);
        }
    }
}
