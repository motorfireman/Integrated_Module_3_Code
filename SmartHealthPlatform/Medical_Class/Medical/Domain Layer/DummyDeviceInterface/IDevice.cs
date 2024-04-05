namespace Medical.Domain_Layer.DummyDeviceInterface;

public class Attribute
{
    public Attribute(int id, int deviceId, string name, string unit, int rangeMin, int rangeMax)
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

public class Device
{
    public Device(int id, String name, String desc, List<Attribute> attributes)
    {
        Name = name;
        Id = id;
        Desc = desc;
        Attributes = attributes;
    }
    public String Name { get; set; }
    
    public int Id { get; set; }
    public String Desc { get; set; }
    public List<Attribute> Attributes { get; set; }
}

public interface IAttribute
{
    private readonly static Dictionary<int, Attribute> _attributes = new()
    {
        { 1, new Attribute(1, 1, "Blood Glucose Level", "mg/dl", 70, 100) },
        { 2, new Attribute(2, 2, "SpO2", "%", 95, 100) },
        { 3, new Attribute(3, 2, "Pulse Rate", "%", 60, 100) },
        { 4, new Attribute(4, 3, "Weight", "%", 50, 100) },
        { 5, new Attribute(5, 3, "BMI", "kg/m^2", 18, 25) },
        { 6, new Attribute(6, 3, "Body Fat", "%", 2, 20) },
        { 7, new Attribute(7, 3, "Muscle Mass", "kg", 75, 89) },
        { 8, new Attribute(8, 3, "Body Water", "%", 45, 65) },
        { 9, new Attribute(9, 3, "Lean Mass", "kg", 68, 90) },
        { 10, new Attribute(10, 3, "Bone Mass", "kg", 3, 5) },
        { 11, new Attribute(11, 3, "Protein", "g/dl", 6, 8) },
        { 12, new Attribute(12, 3, "Visceral Fat Reading", "%", 9, 11) },
        { 13, new Attribute(13, 3, "BMR", "kcal", 1000, 2000) },
        { 14, new Attribute(14, 3, "Metabolic Age", "years", 1, 100) },
        { 15, new Attribute(15, 3, "Heart Rate", "bpm", 60, 100) },
        { 16, new Attribute(16, 4, "Systolic Blood Pressure", "mmHg", 100, 120) },
        { 17, new Attribute(17, 4, "Diastolic Blood Pressure", "mmHg", 60, 80) },
        { 18, new Attribute(18, 4, "Pulse Rate", "bpm", 60, 100) }
    };
    private readonly static Dictionary<int, Device> _devices = new()
    {
        {1, new Device(1, "iHealth Gluco+ Wireless Smart Glucose Meter", "Glucose Device Description",
            new List<Attribute> {_attributes[1]})},
        {2, new Device(2, "iHealth Air Pulse Oximeter", "iNeedAir",
            new List<Attribute>{_attributes[2], _attributes[3]})
            
        },
        {
            3, new Device(3, "iHealth Nexus Pro Wireless Body Composition Scale", 
                "iAmFat",
                new List<Attribute>
                {
                    _attributes[4], _attributes[5], _attributes[6], _attributes[7],
                    _attributes[8], _attributes[9], _attributes[10], _attributes[11],
                    _attributes[12], _attributes[13], _attributes[14], _attributes[15]
                })
        },
        {4, new Device(4, "iHealth Air Pulse Oximeter", "iNeedAir",
            new List<Attribute>{_attributes[16],_attributes[17], _attributes[18]})
        }
    };
    public List<Attribute> GetAttributesForDevice(int deviceId)
    {
        return _devices[deviceId].Attributes;
    }
    public static Attribute GetAttribute(int biomarkerId)
    {
        return _attributes[biomarkerId];
    }
    
}

public interface IDevice
{
    private readonly static Dictionary<int, Attribute> _attributes = new()
    {
        { 1, new Attribute(1, 1, "Blood Glucose Level", "mg/dl", 70, 100) },
        { 2, new Attribute(2, 2, "SpO2", "%", 95, 100) },
        { 3, new Attribute(3, 2, "Pulse Rate", "%", 60, 100) },
        { 4, new Attribute(4, 3, "Weight", "%", 50, 100) },
        { 5, new Attribute(5, 3, "BMI", "kg/m^2", 18, 25) },
        { 6, new Attribute(6, 3, "Body Fat", "%", 2, 20) },
        { 7, new Attribute(7, 3, "Muscle Mass", "kg", 75, 89) },
        { 8, new Attribute(8, 3, "Body Water", "%", 45, 65) },
        { 9, new Attribute(9, 3, "Lean Mass", "kg", 68, 90) },
        { 10, new Attribute(10, 3, "Bone Mass", "kg", 3, 5) },
        { 11, new Attribute(11, 3, "Protein", "g/dl", 6, 8) },
        { 12, new Attribute(12, 3, "Visceral Fat Reading", "%", 9, 11) },
        { 13, new Attribute(13, 3, "BMR", "kcal", 1000, 2000) },
        { 14, new Attribute(14, 3, "Metabolic Age", "years", 1, 100) },
        { 15, new Attribute(15, 3, "Heart Rate", "bpm", 60, 100) },
        { 16, new Attribute(16, 4, "Systolic Blood Pressure", "mmHg", 100, 120) },
        { 17, new Attribute(17, 4, "Diastolic Blood Pressure", "mmHg", 60, 80) },
        { 18, new Attribute(18, 4, "Pulse Rate", "bpm", 60, 100) }
    };
    private readonly static Dictionary<int, Device> _devices = new()
    {
        {1, new Device(1, "iHealth Gluco+ Wireless Smart Glucose Meter", "Glucose Device Description",
            new List<Attribute> {_attributes[1]})},
        {2, new Device(2, "iHealth Air Pulse Oximeter", "iNeedAir",
            new List<Attribute>{_attributes[2], _attributes[3]})
            
        },
        {
            3, new Device(3, "iHealth Nexus Pro Wireless Body Composition Scale", 
                "iAmFat",
                new List<Attribute>
                {
                    _attributes[4], _attributes[5], _attributes[6], _attributes[7],
                    _attributes[8], _attributes[9], _attributes[10], _attributes[11],
                    _attributes[12], _attributes[13], _attributes[14], _attributes[15]
                })
        },
        {4, new Device(4, "iHealth Air Pulse Oximeter", "iNeedAir",
            new List<Attribute>{_attributes[16],_attributes[17], _attributes[18]})
        }
    };
    
    public List<Attribute> GetAttributesForDevice(int deviceId)
    {
        return _devices[deviceId].Attributes;
    }
    public Device GetDevice(int deviceId)
    {
        return _devices[deviceId];
    }
    
    
    // They say userID here is just to make sure user
    // has permission to access the required information
    public List<Device> getAllDevices(int userID)
    {
        return _devices.Values.ToList();
        // Fake attributes
        // var glucose = new Attribute()
        // {
        //     Name = "Blood Glucose Level",
        //     Unit = "mg/dl",
        //     Id = 1
        // };
        // var SpO2 = new Attribute()
        // {
        //     Name = "SpO2",
        //     Unit = "%",
        //     Id = 2,
        // };
        // var pulseRate = new Attribute()
        // {
        //     Name = "Pulse Rate",
        //     Unit = "bpm",
        //     Id = 3,
        // };
        // var Weight = new Attribute()
        // {
        //     Name = "Weight",
        //     Unit = "kg",
        //     Id = 4
        // };
        // var BMI = new Attribute()
        // {
        //     Name = "BMI",
        //     Unit = "kg/m2",
        //     Id = 5
        // };
        // var BodyFat = new Attribute()
        // {
        //     Name = "Body Fat",
        //     Unit = "%",
        //     Id = 6
        // };
        // var MuscleMass = new Attribute()
        // {
        //     Name = "Muscle Mass",
        //     Unit = "kg",
        //     Id = 7
        // };
        //
        // // Fake devices
        // var glucoseDevice = new Device()
        // {
        //     Name = "iHealth Gluco+ Wireless Smart Glucose Meter",
        //     Desc = "Glucose device description",
        //     Id = 1,
        //     Attributes = new List<Attribute> { glucose }
        // };
        // var airPulseDevice = new Device()
        // {
        //     Name = "iHealth Air Pulse Oximeter",
        //     Desc = "iNeedAir ",
        //     Id = 2,
        //     Attributes = new List<Attribute> { SpO2, pulseRate }
        // };
        // var nexusProDevice = new Device()
        // {
        //     Name = "iHealth Nexus Pro Wireless Body Composition Scale",
        //     Desc = "iAmFat",
        //     Id = 3,
        //     Attributes = new List<Attribute> {Weight, BMI, BodyFat, MuscleMass}
        // };
        // return new List<Device> { glucoseDevice, airPulseDevice, nexusProDevice };
    }
}
public interface IMeasurement
{
    public int GetLatestMeasurementForUserAttribute(int userId, int biomarkerId)
    {
        var biomarker = IAttribute.GetAttribute(biomarkerId);
        return new Random().Next(biomarker.RangeMin, biomarker.RangeMax + 1);
    }
}

public class DummyMeasurementControl: IMeasurement
{

}
public class DummyDeviceControl : IDevice
{
    
}

public class DummyAttributeControl : IAttribute
{
    
}