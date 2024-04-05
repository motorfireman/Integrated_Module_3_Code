namespace Medical.Domain_Layer.Module_3.Mock;

public class Biomarker
{
    public Biomarker(int id, int deviceId, string name, string unit, int rangeMin, int rangeMax)
    {
        Id = id;
        DeviceId = deviceId;
        Name = name;
        Unit = unit;
        RangeMin = rangeMin;
        RangeMax = rangeMax;
    }

    public int Id { get; set; }
    public int DeviceId { get; set; }
    public string Name { get; set; }
    public string Unit { get; set; }
    public int RangeMin { get; set; }
    public int RangeMax { get; set; }
}