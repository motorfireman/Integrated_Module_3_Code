using Medical.Data_Source_Layer.Module_3.P1_2.Chat;
using Medical.Domain_Layer.Module_3.Mock;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Medical.Domain_Layer.Module_3.P1_2.Chat
{
    public class ChatSdm : Hub
    {
        private readonly IChatTdg _chatTdg;
        private readonly IUserData _userData;

        // Maps usernames to connection IDs
        private static ConcurrentDictionary<string, string> _users = new ConcurrentDictionary<string, string>();

        public ChatSdm(IUserData userData, IChatTdg chatTdg)
        {
            _userData = userData;
            _chatTdg = chatTdg;
        }

        // Handle user connection
        public override async Task OnConnectedAsync()
        {
            var details = _userData.GetCurrentUser();

            var username = Context.GetHttpContext().Request.Query["username"];

            // Perform null check on username
            if (!string.IsNullOrEmpty(username))
            {
                _users.AddOrUpdate(username, Context.ConnectionId, (key, oldValue) => Context.ConnectionId);
            }
            await base.OnConnectedAsync();
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

        // Send a message to the specified recipient and writes to database
        public async Task SendMessageToHub(string recipientId, string message, string currentUserId)
        {

            if (_users.TryGetValue(recipientId, out var recipientConnectionId))
            {
                await Clients.Client(recipientConnectionId).SendAsync("ReceiveMessage", currentUserId, message);
            }


            SendChatMessage(int.Parse(currentUserId), int.Parse(recipientId), message);

            // Send the message back to the sender as well
            if (_users.TryGetValue(currentUserId.ToString(), out var senderConnectionId))
            {
                await Clients.Client(senderConnectionId).SendAsync("ReceiveMessage", "You", message);
            }
        }

        // Retrieve messages between two users and send them to the caller
        public async Task GetMessagesAndInsertToHub(int recipientId, int senderId)
        {
            // Retrieve messages from the database
            var messages = GetMessages(senderId, recipientId);

            // Send messages to the caller
            await Clients.Caller.SendAsync("LoadMessages", messages);
        }

        // Insert chat message data to database
        public bool SendChatMessage(int userId, int recipientId, string message)
        {
            try
            {
                // Create a new ChatEntity instance and set its details
                ChatEntity newChat = new ChatEntity();

                // Set the sender's and recipient's IDs and the message content
                newChat.SenderId = userId;
                newChat.RecipientId = recipientId;
                newChat.Message = message;
                newChat.Timestamp = DateTime.Now;

                // Call the method in the ChatTdg class to insert the chat
                _chatTdg.Insert(newChat);

                // Return true indicating successful insertion
                return true;
            }
            catch (Exception ex)
            {
                // Return false indicating failure
                return false;
            }
        }

        // Delete a chat message between two users
        public bool DeleteChatMessage(int senderId, int recipientId)
        {
            try
            {
                // Call the method in the ChatTdg class to delete the chat message
                bool success = _chatTdg.DeleteByUserId(senderId, recipientId);

                // Return true indicating successful deletion
                return success;
            }
            catch (Exception ex)
            {
                // Log the error message
                Console.WriteLine($"Error occurred while deleting chat message: {ex.Message}");

                // Return false indicating failure
                return false;
            }
        }

        // Retrieve messages between two users
        public List<ChatEntity> GetMessages(int senderId, int recipientId)
        {
            try
            {     
                // Call the method in the ChatTdg class to retrieve messages
                List<ChatEntity> messages = _chatTdg.GetByUserId(senderId, recipientId);

                // Update sender and recipient IDs with usernames in the message entities
                foreach (var message in messages)
                {
                    if (message.SenderId == senderId)
                    {
                        message.Message = $"You: {message.Message}";
                    }
                    else
                    {
                        message.Message = $"{recipientId}: {message.Message}";
                    }
                }

                // Return the retrieved messages
                return messages;
            }
            catch (Exception ex)
            {
                // Log the error message
                Console.WriteLine($"Error occurred while retrieving messages: {ex.Message}");

                // Return null indicating failure
                return null;
            }
        }
    }
}
