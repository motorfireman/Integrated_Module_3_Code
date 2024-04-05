namespace Medical.Domain_Layer.Module_3.Mock;

public interface IDeviceData
{
    public List<MockDevice> GetDevicesForPatient(int userId);
}