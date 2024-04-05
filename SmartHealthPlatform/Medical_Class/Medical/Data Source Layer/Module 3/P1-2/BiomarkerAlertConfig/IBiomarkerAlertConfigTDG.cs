using System.Collections.Generic;
using BiomarkerAlertConfigEntity = Medical.Domain_Layer.Module_3.P1_2.BiomarkerAlertConfig.BiomarkerAlertConfig;

namespace Medical.Data_Source_Layer.Module_3.P1_2.BiomarkerAlertConfig;

public interface IBiomarkerAlertConfigTDG
{
    List<BiomarkerAlertConfigEntity> GetByUserId(int userId);
    void Insert(BiomarkerAlertConfigEntity config);
    void Update(BiomarkerAlertConfigEntity config);
    void Delete(int configId);
}