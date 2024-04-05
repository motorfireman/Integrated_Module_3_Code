namespace Medical.Domain_Layer.Module_3.Mock;

public interface IMeasurementData
{
    public int GetLatestMeasurementForUserBiomarker(int userId, int biomarkerId);
}