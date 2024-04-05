namespace Medical.Domain_Layer.Module_3.Mock;

public class MockDevice
{
    public MockDevice(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; set; }
    public string Name { get; set; }
}