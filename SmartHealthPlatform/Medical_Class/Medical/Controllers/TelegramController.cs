using System.Text.Json.Nodes;
using Medical.Domain_Layer.Module_3.P1_2.Communication.TelegramRegistration;
using Microsoft.AspNetCore.Mvc;

namespace Medical.Controllers;

public class TelegramController : Controller
{
    private readonly ITelegramRegistrationSdm _telegramRegistrationSdm;

    public TelegramController(ITelegramRegistrationSdm telegramRegistrationSdm)
    {
        _telegramRegistrationSdm = telegramRegistrationSdm;
    }
    
    // GET
    public IActionResult Index()
    {
        return View("~/Views/Presentation Layer/Module 3/P1-2/Telegram.cshtml");
    }

    public IActionResult GetRegistrationStatus()
    {
        var result = _telegramRegistrationSdm.GetRegistrationStatus();
        return Ok(result);
    }

    public IActionResult GenerateToken()
    {
        var result = _telegramRegistrationSdm.GenerateToken();
        return Ok(result);
    }

    public IActionResult DeRegisterAccount()
    {
        var result = _telegramRegistrationSdm.DeRegisterAccount();
        return Ok(result);
    }

    // From Telegram Bot
    [HttpPost]
    public IActionResult ReceiveMessage([FromBody] JsonNode update)
    {
        _telegramRegistrationSdm.ReceiveMessage(update);
        return Ok();
    }
}