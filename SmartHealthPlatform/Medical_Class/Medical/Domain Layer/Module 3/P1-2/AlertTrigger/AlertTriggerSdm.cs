using Medical.Data_Source_Layer.Module_3.P1_2.AlertTrigger;
using Medical.Domain_Layer.Module_3.Mock;

namespace Medical.Domain_Layer.Module_3.P1_2.AlertTrigger
{
    public class AlertTriggerSdm
    {
        private readonly IUserData _userData;
        private readonly IAlertTriggerTdg _alertTriggerTdg;

        public AlertTriggerSdm(IUserData userData, IAlertTriggerTdg alertTriggerTdg)
        {
            _userData = userData;
            _alertTriggerTdg = alertTriggerTdg;
        }

        public List<AlertTriggerEntity> GetAlerts()
        {
            var user = _userData.GetCurrentUser();

            if (user != null)
            {
                List<AlertTriggerEntity> alerts = _alertTriggerTdg.GetByUserId(user.Id);

                foreach (var alert in alerts)
                {
                    alert.message = alert.message;
                }
                return alerts;
            }

            return new List<AlertTriggerEntity>();
        }

        public bool SendManualAlert(int recipientId, string type, int caregiverId, int alertConfigId, string message)
        {
            try
            {
                var user = _userData.GetCurrentUser();

                if (user != null)
                {
                    AlertTriggerEntity newAlertTrigger = new AlertTriggerEntity();

                    newAlertTrigger.userId = user.Id;
                    newAlertTrigger.recipientId = recipientId;
                    newAlertTrigger.type = type;
                    newAlertTrigger.caregiverId = caregiverId;
                    newAlertTrigger.alertConfigId = alertConfigId;
                    newAlertTrigger.triggeredOn = DateTime.Now;
                    newAlertTrigger.message = message;

                    _alertTriggerTdg.Insert(newAlertTrigger);

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}