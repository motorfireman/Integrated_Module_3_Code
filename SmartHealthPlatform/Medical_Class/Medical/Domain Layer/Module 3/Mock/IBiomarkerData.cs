namespace Medical.Domain_Layer.Module_3.Mock;

public interface IBiomarkerData
{
    public List<Biomarker> GetBiomarkersForDevice(int deviceId);
    public Biomarker GetBiomarker(int biomarkerId);
}