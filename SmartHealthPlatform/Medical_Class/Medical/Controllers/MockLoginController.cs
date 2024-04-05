using System.Text.Json.Nodes;
using Medical.Domain_Layer.Module_3.P1_2.Communication;
using Microsoft.AspNetCore.Mvc;

namespace Medical.Controllers;

public class MockLoginController : Controller
{
    private readonly IMessageSender _messageSender;

    public MockLoginController(IMessageSender messageSender)
    {
        _messageSender = messageSender;
    }
    
    // GET
    public IActionResult Index()
    {
        return Ok();
    }

    public IActionResult CheckLogin()
    {
        var userId = HttpContext.Session.GetInt32("userId");
        return Json(new JsonObject()
        {
            {"logged_in", userId != null}
        });
    }
    
    [HttpGet("/MockLogin/Login/{userId}")]
    public IActionResult Login(int userId)
    {
        HttpContext.Session.SetInt32("userId", userId);
        Console.WriteLine($"Logged in as {userId}");
        return Ok();
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return Ok();
    }

    [HttpPost]
    public IActionResult SendEmail([FromBody] JsonNode data)
    {
        var userId = data["userId"]?.GetValue<int>() ?? 0;
        var subject = data["subject"]?.GetValue<string>() ?? "";
        var message = data["message"]?.GetValue<string>() ?? "";
        _messageSender.Send(userId, subject, message, MessageType.Email);
        return Ok();
    }

    [HttpPost]
    public IActionResult SendTelegram([FromBody] JsonNode data)
    {
        var userId = data["userId"]?.GetValue<int>() ?? 0;
        var subject = data["subject"]?.GetValue<string>() ?? "";
        var message = data["message"]?.GetValue<string>() ?? "";
        _messageSender.Send(userId, subject, message, MessageType.Telegram);
        return Ok();
    }
    
}