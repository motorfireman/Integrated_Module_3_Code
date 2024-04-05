using System.Text.Json.Nodes;

namespace Mediqu.Controllers;

public class Data
{

    private string name { get; set; }
    private string hobby { get; set; }

    public Data(string name, string hobby)
    {
        this.name = name;
        this.hobby = hobby;
    }
    
}