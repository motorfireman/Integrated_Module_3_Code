
using Medical.Data_Source_Layer;
using Medical.Data_Source_Layer.Module_3.P1_2.BiomarkerAlertConfig;
using Medical.Domain_Layer.Module_3.Mock;
using Medical.Domain_Layer.Module_3.P1_2.MsgTemplate;
using Microsoft.Extensions.Logging;

namespace Medical.Domain_Layer.Module_3.P1_2.BiomarkerAlertConfig
{
    public class BiomarkerAlertConfigSDM : IRetrieveThreshold
    {
        private readonly IBiomarkerAlertConfigTDG _biomarkerAlertConfigTDG;
        private readonly IUserData _userData;
        private readonly IDeviceData _deviceData;
        private readonly IBiomarkerData _biomarkerData;
        private readonly IRetrieveMessageTemplate _retrieveMessage;
        private readonly ILogger<BiomarkerAlertConfigSDM> _logger;



        public BiomarkerAlertConfigSDM(IBiomarkerAlertConfigTDG biomarkerAlertConfigTDG, IUserData userData,
            IDeviceData deviceData, IBiomarkerData biomarkerData, IRetrieveMessageTemplate retrieveMessage,
             ILogger<BiomarkerAlertConfigSDM> logger)
        {
            _biomarkerAlertConfigTDG = biomarkerAlertConfigTDG;
            _userData = userData;
            _deviceData = deviceData;
            _biomarkerData = biomarkerData;
            _logger = logger;
            _retrieveMessage = retrieveMessage;

        }



        public List<BiomarkerAlertConfig> GetAlertConfigurations(int userId)
        {
            return _biomarkerAlertConfigTDG.GetByUserId(userId);
        }

        public void AddAlertConfiguration(BiomarkerAlertConfig config)
        {
            _biomarkerAlertConfigTDG.Insert(config);
        }

        public void EditAlertConfiguration(BiomarkerAlertConfig config)
        {
            _biomarkerAlertConfigTDG.Update(config);
        }

        public void DeleteAlertConfiguration(int configId)
        {
            _biomarkerAlertConfigTDG.Delete(configId);
        }
        
        


        //Method to automatically create alert configurations for patients associated with a logged-in practitioner
        public void CreateAlertConfigsForAllAssociatedPatients(int userId)
        {
            _logger.LogInformation($"Starting to create alert configs for practitioner with ID: {userId}"); // Log starting message
            var patientIds = _userData.GetPatientsForMedicalPractitioner(userId);

            foreach (var patientId in patientIds)
            {
                // Retrieve message templates for this patient
                var messageTemplates = _retrieveMessage.GetMessageTemplates(patientId.Id);
                if (messageTemplates.Any())
                {
                    // Select a template ID based on your criteria. Here, we're just taking the first one.
                    int templateId = messageTemplates.First().Id;

                    _logger.LogInformation($"Processing patient with ID: {patientId.Id}");
                    var devices = _deviceData.GetDevicesForPatient(patientId.Id); // Assuming GetDevicesForPatient returns devices for a given patient ID
                    foreach (var device in devices)
                    {
                        _logger.LogInformation($"Processing device with ID: {device.Id} for patient {patientId.Id}");
                        var biomarkers = _biomarkerData.GetBiomarkersForDevice(device.Id); // Assuming GetBiomarkersForDevice returns biomarkers for a given device ID
                        foreach (var biomarker in biomarkers)
                        {
                            _logger.LogInformation($"Adding config for biomarker {biomarker.Id} on device {device.Id} for patient {patientId.Id}");
                            var config = new BiomarkerAlertConfig
                            {
                                UserDeviceId = device.Id,
                                MpId = userId,
                                BiomarkerId = biomarker.Id,
                                TelegramAlertEnabled = true, //by default
                                EmailAlertEnabled = true,   //by default
                                Severity = "Normal",
                                MaxThreshold = biomarker.RangeMax,
                                MinThreshold = biomarker.RangeMin,
                                TemplateId = templateId, // Use the selected template ID
                            };
                            _biomarkerAlertConfigTDG.Insert(config);
                        }
                    }
                }

            }
        }

        public class BiomarkerThreshold
        {
            public int BiomarkerId { get; set; }
            public double MinThreshold { get; set; }
            public double MaxThreshold { get; set; }
        }

        public List<BiomarkerThreshold> GetThresholdsForUser(int userId)
        {
            var configs = _biomarkerAlertConfigTDG.GetByUserId(userId);
            var thresholds = configs.Select(config => new BiomarkerThreshold
            {
                BiomarkerId = config.BiomarkerId,
                MinThreshold = config.MinThreshold,
                MaxThreshold = config.MaxThreshold
            }).ToList();

            return thresholds;
        }



    }
}