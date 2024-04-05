using Microsoft.AspNetCore.Mvc;
using Medical.Domain_Layer.Module_3.P1_2.Chat;

namespace Mediqu.Controllers
{
    public class ChatController : Controller
    {
        private readonly ChatSdm _chatSdm;

        public ChatController(ChatSdm chatSdm)
        {
            _chatSdm = chatSdm;
        }

        public IActionResult Chat()
        {
            return View("~/Views/Presentation Layer/Module 3/P1-2/Chat.cshtml");
        }

        [HttpPost]
        public IActionResult SendChatMessage(int userId, int recipientId, string message)
        {
            // Call the SendChatMessage method of the ChatSdm class
            bool success = _chatSdm.SendChatMessage(userId, recipientId, message);

            // Check if the message was sent successfully
            if (success)
            {
                // Return a success response
                return Ok("Chat message sent successfully.");
            }
            else
            {
                // Return an error response
                return BadRequest("Failed to send chat message.");
            }
        }

        [HttpGet]
        public IActionResult RetrieveMessages(int senderId, int recipientId)
        {
            // Call the method in domain layer to retrieve messages
            List<ChatEntity> messages = _chatSdm.GetMessages(senderId, recipientId);

            // Check if messages were retrieved successfully
            if (messages != null)
            {
                // Return the messages as JSON
                return Json(messages);
            }
            else
            {
                // Return an error response
                return BadRequest("Failed to retrieve messages.");
            }
        }

        [HttpDelete]
        public IActionResult DeleteChatMessage(int senderId, int recipientId)
        {
            // Call the method in domain layer to delete the chat message
            bool success = _chatSdm.DeleteChatMessage(senderId, recipientId);

            // Check if the message was deleted successfully
            if (success)
            {
                // Return a success response
                return Ok("Chat message deleted successfully.");
            }
            else
            {
                // Return an error response
                return BadRequest("Failed to delete chat message.");
            }
        }

        public IActionResult GetUserDetails()
        {
            // Call the method in domain layer to get the user details
            var result = _chatSdm.GetUserDetails();
            return Ok(result);
        }
    }
}
