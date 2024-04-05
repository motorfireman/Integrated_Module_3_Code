using Medical.Domain_Layer.Module_3.P1_2.Chat;

namespace Medical.Data_Source_Layer.Module_3.P1_2.Chat
{
    public class ChatTdg: IChatTdg
    {
        private readonly SmartHealthPlatformContext _dbContext;

        public ChatTdg(SmartHealthPlatformContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Method to get chat messages by user ID
        public List<ChatEntity> GetByUserId(int senderId, int recipientId)
        {
            // Retrieve chat details, order by timestamp in descending order
            var chats = _dbContext.Chats
                .Where(c => (c.SenderId == senderId && c.RecipientId == recipientId) ||
                            (c.SenderId == recipientId && c.RecipientId == senderId))
                .OrderBy(c => c.Timestamp)  
                .ToList();

            return chats;
        }

        // Method to insert chat messages by user ID
        public void Insert(ChatEntity newChat)
        {
            try
            {
                // Add the chat entity to the database context and save changes
                _dbContext.Chats.Add(newChat);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while inserting chat: {ex.Message}");
                throw;
            }
        }

        // Method to delete chat messages by sender and recipient IDs
        public bool DeleteByUserId(int senderId, int recipientId)
        {
            try
            {
                // Retrieve chat messages to delete
                var messagesToDelete = _dbContext.Chats
                    .Where(c => (c.SenderId == senderId && c.RecipientId == recipientId) ||
                                (c.SenderId == recipientId && c.RecipientId == senderId))
                    .ToList();

                // Remove the chat messages from the database context
                _dbContext.Chats.RemoveRange(messagesToDelete);

                // Save changes to the database
                _dbContext.SaveChanges();

                return true; // Return true indicating successful deletion
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while deleting chat messages: {ex.Message}");
                return false; // Return false indicating failure
            }
        }
    }
}
