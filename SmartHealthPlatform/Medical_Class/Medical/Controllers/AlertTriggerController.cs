using Microsoft.AspNetCore.Mvc;
using Medical.Domain_Layer.Module_3.P1_2.AlertTrigger;
using Medical.Domain_Layer.Module_3.P1_2.Communication;

namespace Medical.Controllers
{
    public class AlertTriggerController : Controller
    {
        private readonly AlertTriggerSdm _alertTriggerSdm;
        private readonly IMessageSender _messageSender;

        public AlertTriggerController(AlertTriggerSdm alertTriggerSdm, IMessageSender messageSender)
        {
            _alertTriggerSdm = alertTriggerSdm;
            _messageSender = messageSender;
        }

        public IActionResult AlertTrigger()
        {
            return View("~/Views/Presentation Layer/Module 3/P1-2/AlertTrigger.cshtml");
        }

        [HttpGet]
        public IActionResult GetAlerts()
        {
            List<AlertTriggerEntity> alerts = _alertTriggerSdm.GetAlerts();

            if (alerts != null)
            {
                return Json(alerts);
            }
            else
            {
                return BadRequest("Failed to retrieve alerts.");
            }
        }

        [HttpPost]
        public IActionResult SendManualAlert(int recipientId, string type, int caregiverId, int alertConfigId, string message)
        {
            bool success = _alertTriggerSdm.SendManualAlert(recipientId, type, caregiverId, alertConfigId, message);

            if (success)
            {
                if (type.Equals(MessageType.Email.ToString()))
                {
                    _messageSender.Send(recipientId, "Alert Trigger", message, MessageType.Email);
                    return Ok("Email send out");
                }
                else if (type.Equals(MessageType.Telegram.ToString()))
                {
                    _messageSender.Send(recipientId, "Alert Trigger", message, MessageType.Telegram);
                    return Ok("Telegram send out");
                }
            }

            return BadRequest("Failed to trigger alert.");
        }
    }
}