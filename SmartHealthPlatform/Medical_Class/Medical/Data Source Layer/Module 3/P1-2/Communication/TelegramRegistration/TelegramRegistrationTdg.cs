using Microsoft.EntityFrameworkCore;

namespace Medical.Data_Source_Layer.Module_3.P1_2.Communication;

public class TelegramRegistrationTdg: ITelegramRegistrationTdg
{
    private readonly SmartHealthPlatformContext _db;

    public TelegramRegistrationTdg(SmartHealthPlatformContext db)
    {
        _db = db;
    }
    
    public TelegramRegistrationDao? GetByUserId(int userId)
    {
        return _db.TelegramRegistrations.FirstOrDefault(t => t.UserId == userId);
    }
    
    public TelegramRegistrationDao? GetByToken(string token)
    {
        return _db.TelegramRegistrations.FirstOrDefault(t => t.Token == token);
    }
    
    public void Insert(int userId, string token)
    {
        var registration = new TelegramRegistrationDao();
        registration.UserId = userId;
        registration.Token = token;
        registration.TokenGenerated = DateTime.Now;
        _db.TelegramRegistrations.Add(registration);
        _db.SaveChanges();
    }
    
    public void UpdateTokenById(int id, string token)
    {
        _db
            .TelegramRegistrations
            .Where(t => t.Id == id)
            .ExecuteUpdate(p => p
                .SetProperty(t => t.Token, token)
                .SetProperty(t => t.TokenGenerated, DateTime.Now)
            );
        _db.SaveChanges();
    }
    
    public void UpdateTelegramIdByToken(string token, int telegramId)
    {
        _db
            .TelegramRegistrations
            .Where(t => t.Token == token)
            .ExecuteUpdate(p => p
                .SetProperty(t => t.TelegramId, telegramId)
            );
        _db.SaveChanges();
    }
    
    public void DeleteByUserId(int userId)
    {
        _db.TelegramRegistrations.Where(t => t.UserId == userId).ExecuteDelete();
        _db.SaveChanges();
    }
    
}