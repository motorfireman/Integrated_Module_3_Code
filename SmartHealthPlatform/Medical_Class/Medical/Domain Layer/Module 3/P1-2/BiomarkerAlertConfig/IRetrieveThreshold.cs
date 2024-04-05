namespace Medical.Domain_Layer.Module_3.P1_2.BiomarkerAlertConfig
{
    public interface IRetrieveThreshold
    {
        List<BiomarkerAlertConfigSDM.BiomarkerThreshold> GetThresholdsForUser(int userId);
    }
}
