using System.Collections.Generic;
using CitizenFX.Core.Native;
using Newtonsoft.Json.Linq;

namespace Kilo.Commons.Config
{
    public class Config<T> where T : JContainer
    {
        public JContainer Store { get; }

        public Config(string resourceName, string path)
        {
            path = path.Replace(resourceName, "");
            Store = LoadResourceFile(resourceName, path);
        }

        public static T LoadResourceFile(string resourceName, string path)
        {
            var file = API.LoadResourceFile(resourceName, path);
            if (file.StartsWith("[") && file.EndsWith("]"))
            {
                return JArray.Parse(file).ToObject<T>();
            }
            return JObject.Parse(file).ToObject<T>();
        }
    }
}