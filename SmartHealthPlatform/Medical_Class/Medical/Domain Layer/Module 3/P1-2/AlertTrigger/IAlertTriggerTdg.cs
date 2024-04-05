using Medical.Domain_Layer.Module_3.P1_2.AlertTrigger;

namespace Medical.Domain_Layer.Module_3.P1_2.AlertTrigger
{
    public interface IAlertTriggerTdg
    {
        List<AlertTriggerEntity> GetByUserId(int userId);
        void Insert(AlertTriggerEntity newAlertTrigger);
    }
}