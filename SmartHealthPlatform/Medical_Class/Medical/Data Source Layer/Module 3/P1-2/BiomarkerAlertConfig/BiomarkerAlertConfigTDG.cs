using Microsoft.EntityFrameworkCore;
using BiomarkerAlertConfigEntity = Medical.Domain_Layer.Module_3.P1_2.BiomarkerAlertConfig.BiomarkerAlertConfig;

namespace Medical.Data_Source_Layer.Module_3.P1_2.BiomarkerAlertConfig
{
    public class BiomarkerAlertConfigTDG : IBiomarkerAlertConfigTDG
    {
        private readonly SmartHealthPlatformContext _context;

        public BiomarkerAlertConfigTDG(SmartHealthPlatformContext context)
        {
            _context = context;
        }

        public List<BiomarkerAlertConfigEntity> GetByUserId(int userId)
        {
            // Retrieve and return configurations matching the provided userId
            return _context.BiomarkerAlertConfigs
                           .Where(config => config.MpId == userId)
                           .ToList();
        }

        public void Insert(BiomarkerAlertConfigEntity config)
        {
            // Add a new BiomarkerAlertConfig to the context and save changes
            _context.BiomarkerAlertConfigs.Add(config);
            _context.SaveChanges();
        }

        public void Update(BiomarkerAlertConfigEntity config)
        {
            // Update an existing BiomarkerAlertConfig and save changes
            _context.BiomarkerAlertConfigs.Update(config);
            _context.SaveChanges();
        }

        public void Delete(int configId)
        {
            // Find a BiomarkerAlertConfig by ID and remove it, then save changes
            var config = _context.BiomarkerAlertConfigs.Find(configId);
            if (config != null)
            {
                _context.BiomarkerAlertConfigs.Remove(config);
                _context.SaveChanges();
            }
        }
    }
}