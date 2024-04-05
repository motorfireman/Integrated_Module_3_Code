namespace Medical.Domain_Layer.Module_3.Mock;

public class MockDeviceData: IDeviceData
{
    private readonly Dictionary<int, MockDevice> _devices = new()
    {
        {1, new MockDevice(1, "iHealth Gluco+ Wireless Smart Glucose Meter")},
        {2, new MockDevice(2, "iHealth Air Pulse Oximeter")},
        {3, new MockDevice(3, "iHealth Nexus Pro Wireless Body Composition Scale")},
        {4, new MockDevice(4, "iHealth Ease Wireless Monitor")}
    };
    
    public List<MockDevice> GetDevicesForPatient(int userId)
    {
        // Assume each use has all the devices
        return _devices.Values.ToList();
    }
}