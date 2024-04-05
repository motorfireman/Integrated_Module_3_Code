using Microsoft.AspNetCore.Mvc;
using Medical.Domain_Layer.Module_3.P1_2.BiomarkerAlertConfig;
using Medical.Domain_Layer.Module_3.Mock;
using System.Reflection;
using static Org.BouncyCastle.Math.EC.ECCurve;
// Namespace for BiomarkerAlertConfigSDM
// Other necessary usings

public class BiomarkerAlertConfigController : Controller
{
    private readonly BiomarkerAlertConfigSDM _biomarkerAlertConfigSDM;
    private readonly IUserData _userData;

    public BiomarkerAlertConfigController(BiomarkerAlertConfigSDM biomarkerAlertConfigSDM, IUserData userData)
    {
        _biomarkerAlertConfigSDM = biomarkerAlertConfigSDM;
        _userData = userData;
    }

    // Display list of BiomarkerAlertConfigs
    public IActionResult Index()
    {
        var currentUser = _userData.GetCurrentUser();
        if (currentUser == null)
        {
            // Handle the case where no user is logged in or the user could not be retrieved
            return Unauthorized(); // Or another appropriate response
        }

        var userId = currentUser.Id;
        var configs = _biomarkerAlertConfigSDM.GetAlertConfigurations(userId);
        return View("~/Views/Presentation Layer/Module 3/P1-2/BiomarkerAlertConfig/Index.cshtml", configs);
    }
    // Display form to create a new BiomarkerAlertConfig
    public IActionResult Create()
    {
        return View("~/Views/Presentation Layer/Module 3/P1-2/BiomarkerAlertConfig/Create.cshtml");
    }

    // POST: Create a new BiomarkerAlertConfig
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(BiomarkerAlertConfig config)
    {
        var u = _userData.GetCurrentUser();
        config.MpId = u.Id;
        if (ModelState.IsValid)
        {
            _biomarkerAlertConfigSDM.AddAlertConfiguration(config);
            return RedirectToAction(nameof(Index));
        }
        return View(config);
    }

    // Display form to edit an existing BiomarkerAlertConfig
    public IActionResult Edit(int id)
    {
        var config = _biomarkerAlertConfigSDM.GetAlertConfigurations(id).FirstOrDefault(); // Assuming a method to fetch a single config by ID
        if (config == null)
        {
            return NotFound();
        }
        return View(config);
    }

    // POST: Update an existing BiomarkerAlertConfig
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(BiomarkerAlertConfig config)
    {
        if (ModelState.IsValid)
        {
            _biomarkerAlertConfigSDM.EditAlertConfiguration(config);
            return RedirectToAction(nameof(Index));
        }
        return View(config);
    }

    // GET: Delete a BiomarkerAlertConfig
    public IActionResult Delete(int id)
    {
        _biomarkerAlertConfigSDM.DeleteAlertConfiguration(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult SaveEdit([FromBody] BiomarkerAlertConfig config)
    {
        try
        {
            if (ModelState.IsValid)
            {
                _biomarkerAlertConfigSDM.EditAlertConfiguration(config);
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Invalid data." });
        }
        catch (Exception ex)
        {
            // Log the error
            return Json(new { success = false, message = ex.Message });
        }
    }
}