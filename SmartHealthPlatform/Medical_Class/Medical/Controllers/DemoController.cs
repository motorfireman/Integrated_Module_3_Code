using System.Text.RegularExpressions;
using Medical.Data_Source_Layer;
using Medical.Domain_Layer.Module_3.Mock;
using Medical.Domain_Layer.Module_3.P1_2.Communication;
using Medical.Domain_Layer.Module_3.P1_2.MsgTemplate;
using Microsoft.AspNetCore.Mvc;

namespace Medical.Controllers;

public class DemoController : Controller
{
    private readonly IUserData _userData;
    private readonly IDeviceData _deviceData;
    private readonly IBiomarkerData _biomarkerData;
    private readonly SmartHealthPlatformContext _db;
    private readonly IRetrieveMessageTemplate _retrieveMessageTemplate;
    private readonly IMeasurementData _measurementData;
    private readonly IMessageSender _messageSender;

    public DemoController(IUserData userData, IDeviceData deviceData, IBiomarkerData biomarkerData, SmartHealthPlatformContext db, IRetrieveMessageTemplate retrieveMessageTemplate, IMeasurementData measurementData, IMessageSender messageSender)
    {
        _userData = userData;
        _deviceData = deviceData;
        _biomarkerData = biomarkerData;
        _db = db;
        _retrieveMessageTemplate = retrieveMessageTemplate;
        _measurementData = measurementData;
        _messageSender = messageSender;
    }
    
    public IActionResult Index()
    {
        return View("~/Views/Presentation Layer/Module 3/P1-2/Demo.cshtml");
    }
    
    public IActionResult GetUserDetails()
    {
        var user = _userData.GetCurrentUser();
        return user == null ? Json(new Dictionary<string, string> { { "logged_in", "false" } }) : Json(user);
    }

    [HttpGet("/Demo/Login/{userId:int}")]
    public IActionResult Login(int userId)
    {
        HttpContext.Session.SetInt32("userId", userId);
        return Ok();
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return Ok();
    }

    public IActionResult GetUserDevices()
    {
        var user = _userData.GetCurrentUser();
        if (user == null)
            return Json(new Dictionary<string, string>
            {
                { "status", "unauthorised" }
            });
        
        return Json(_deviceData.GetDevicesForPatient(user.Id));
    }

    [HttpGet("/Demo/GetBiomarkers/{deviceId:int}")]
    public IActionResult GetBiomarkers(int deviceId)
    {
        return Json(_biomarkerData.GetBiomarkersForDevice(deviceId));
    }
    
    [HttpGet]
    public IActionResult UploadReading()
    {
        var biomarkerId = int.Parse(HttpContext.Request.Query["biomarkerId"].ToString());
        var reading = int.Parse(HttpContext.Request.Query["reading"].ToString());
        var userId = _userData.GetCurrentUser()?.Id;

        if (userId == null)
            return Json(new Dictionary<string, string>
            {
                {"error", "unauthorised"}
            });
        
        // Get all alert rules that match user and biomarker id
        var rule = _db
            .BiomarkerAlertConfigs
            .FirstOrDefault(c => c.UserDeviceId == userId && c.BiomarkerId == biomarkerId && (c.MinThreshold > reading || c.MaxThreshold < reading));

        // No rule matched
        if (rule == null)
            return Ok();
        
        var template = _retrieveMessageTemplate.GetMessageTemplate(rule.MpId, rule.TemplateId);
        var message = template.Message;
        var matches = Regex.Matches(message, @"<\d+,(\d+)>");
        foreach (Match match in matches)
        {
            var templateBiomarkerId = int.Parse(match.Groups[1].Value);
            var data = templateBiomarkerId == biomarkerId ? reading: _measurementData.GetLatestMeasurementForUserBiomarker(userId ?? 0, templateBiomarkerId);
            message = message.Replace(match.ToString(), data.ToString());
        }
        
        if (rule.TelegramAlertEnabled)
            _messageSender.Send(rule.MpId, template.Subject, message, MessageType.Telegram);
        if (rule.EmailAlertEnabled)
            _messageSender.Send(rule.MpId, template.Subject, message, MessageType.Email);
        
        return Ok();
    }
    
}