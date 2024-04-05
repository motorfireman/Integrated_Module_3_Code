namespace Medical.Data_Source_Layer.Module_3.P1_2.Communication;

public interface ITelegramRegistrationTdg
{
    public TelegramRegistrationDao? GetByUserId(int userId);
    public TelegramRegistrationDao? GetByToken(string token);
    public void Insert(int userId, string token);
    public void UpdateTokenById(int id, string token);
    public void UpdateTelegramIdByToken(string token, int telegramId);
    public void DeleteByUserId(int userId);
}