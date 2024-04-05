namespace Medical.Domain_Layer.Module_3.Mock;

public class MockMeasurementData: IMeasurementData
{
    private readonly IBiomarkerData _biomarkerData;

    public MockMeasurementData(IBiomarkerData biomarkerData)
    {
        _biomarkerData = biomarkerData;
    }
    
    public int GetLatestMeasurementForUserBiomarker(int userId, int biomarkerId)
    {
        var biomarker = _biomarkerData.GetBiomarker(biomarkerId);
        return new Random().Next(biomarker.RangeMin, biomarker.RangeMax + 1);
    }
}