using System.Diagnostics.Contracts;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json.Nodes;
using Medical.Domain_Layer.DummyDeviceInterface;
using Medical.Domain_Layer.Module_3.Mock;
using Medical.Domain_Layer.Module_3.P1_2.MsgTemplate;
using Microsoft.AspNetCore.Mvc;

namespace Mediqu.Controllers;

public class MessageTemplateController : Controller
{
    private readonly IEditMessageTemplate _mtsdm;
    
    public MessageTemplateController(IEditMessageTemplate mtsdm)
    {
        _mtsdm = mtsdm;
    }
    // GET
    public ViewResult Index()
    {
        return View("~/Views/Presentation Layer/Module 3/P1-2/MessageTemplate.cshtml");
    }

    public IActionResult GetMessageTemplates()
    {
        try
        {
            // UserId is hardcoded as module does not implement Account system
            var userId = HttpContext.Session.GetInt32("userId") ?? 1;
            List<MessageTemplate> messageTemplates = _mtsdm.GetMessageTemplatesForEditor(userId);

            return Json(messageTemplates);
        }
        catch (Exception e)
        {
            return StatusCode(500,"Unable to retrieve message templates. Please try again later.");
        }

    }
    
    public IActionResult GetMessageTemplate(int templateId)
    {
        try
        {
            // UserId is hardcoded as module does not implement Account system
            var userId = HttpContext.Session.GetInt32("userId") ?? 1;
            var messageTemplate = _mtsdm.GetMessageTemplateForEditor(userId, templateId);
            return Json(messageTemplate);
        }
        catch (Exception e)
        {
            return StatusCode(500,"Unable to retrieve message template. Please try again later.");
        }
    }
    
    [HttpPost]
    public IActionResult AddMessageTemplate(MessageTemplate messageTemplate)
    {
        try
        {
            // UserId is hardcoded as module does not implement Account system
            var userId = HttpContext.Session.GetInt32("userId") ?? 1;
            messageTemplate.UserId = userId;
            _mtsdm.AddMessageTemplate(
                messageTemplate
            );
            return Ok("Message Templated Added Successfully!");
        }
        
        catch (Exception e)
        {
            if (e is InvalidDataException)
            {
                return BadRequest(e.Message);
            }
            return StatusCode(500,"Failed to add message template. Please try again later.");
        }

    }

    [HttpPost]
    public IActionResult EditMessageTemplate(MessageTemplate messageTemplate)
    {
        try
        {
            // UserId is hardcoded as module does not implement Account system
            var userId = HttpContext.Session.GetInt32("userId") ?? 1;
            messageTemplate.UserId = userId;

            _mtsdm.EditMessageTemplate(messageTemplate);

            return Ok("Modified!");
        }
        catch (Exception e)
        {
            if (e is InvalidDataException)
            {
                return BadRequest(e.Message);
            }
            return StatusCode(500,"Failed to update message template. Please try again later.");
        }
    }
    
    public IActionResult DeleteMessageTemplate([FromBody] int templateId)
    {
        try
        {
            // UserId is hardcoded as module does not implement Account system
            var userId = HttpContext.Session.GetInt32("userId") ?? 1;
            _mtsdm.DeleteMessageTemplate(userId, templateId);

            return Ok("Your message template is banished.");
        }
        catch (Exception e)
        {
            return StatusCode(500,"Failed to delete message template. Please try again later.");
        }
    }

    public IActionResult GetDeviceList()
    {
        try
        {
            // UserId is hardcoded as module does not implement Account system
            var userId = HttpContext.Session.GetInt32("userId") ?? 1;
            return Json(_mtsdm.GetAttributes(userId));
        }
        catch
        {
            return Forbid();
        }
    }
    
}