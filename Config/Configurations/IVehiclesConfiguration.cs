#nullable enable
using Newtonsoft.Json.Linq;

namespace Kilo.Commons.Config.Example;

public interface IPoliceVehicleConfigurationEntry
{
    public string name { get; }
    public string vehicle { get; }
    public bool isAvailableForEveryone { get; }
    public bool useRanks { get; }
    public JArray? availableForRanks { get; }
    public JArray? availableForDepartments { get; }
}

public interface IVehiclesConfiguration : IDocument
{
    IPoliceVehicleConfigurationEntry[] police { get; }
    JArray ambulance { get; }
    JArray airAmbulance { get; }
    JArray firedept { get; }
    JArray coroner { get; }
    JArray towtruck { get; }
    JArray mechanic { get; }
    JArray prisontransport { get; }
    JArray animalControl { get; }
    JArray taxi { get; }
}