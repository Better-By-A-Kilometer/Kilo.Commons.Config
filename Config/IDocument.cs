using Newtonsoft.Json.Linq;

namespace Kilo.Commons.Config;

public interface IDocument
{
    JContainer Store { get; }
    public string Name { get; }
    
    public object this[string key]
    { get; set; }
}