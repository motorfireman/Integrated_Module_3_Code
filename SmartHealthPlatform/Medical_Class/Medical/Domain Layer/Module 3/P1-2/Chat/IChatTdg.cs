using Medical.Domain_Layer.Module_3.P1_2.Chat;

namespace Medical.Data_Source_Layer.Module_3.P1_2.Chat
{
    public interface IChatTdg
    {
        List<ChatEntity> GetByUserId(int senderId, int recipientId);
        void Insert(ChatEntity newChat);
        bool DeleteByUserId(int senderId, int recipientId);
    }
}
