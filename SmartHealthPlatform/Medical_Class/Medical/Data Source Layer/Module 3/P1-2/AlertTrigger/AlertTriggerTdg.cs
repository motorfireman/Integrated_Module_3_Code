using Medical.Domain_Layer.Module_3.P1_2.AlertTrigger;
using Medical.Models;

namespace Medical.Data_Source_Layer.Module_3.P1_2.AlertTrigger
{
    public class AlertTriggerTdg : IAlertTriggerTdg
    {
        private readonly SmartHealthPlatformContext _dbContext;

        public AlertTriggerTdg(SmartHealthPlatformContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<AlertTriggerEntity> GetByUserId(int userId)
        {
            try
            {
                List<AlertTriggerEntity> alertTriggers = _dbContext.AlertTriggers
                    .Where(alertTrigger => alertTrigger.userId == userId)
                    .ToList();
                return alertTriggers;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error occurred while retrieving alert trigger: {e.Message}");
                throw;
            }
        }

        public void Insert(AlertTriggerEntity newAlertTrigger)
        {
            try
            {
                _dbContext.AlertTriggers.Add(newAlertTrigger);
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error occurred while inserting alert trigger: {e.Message}");
                throw;
            }
        }
    }
}