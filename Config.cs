using System;
using System.Collections.Generic;
using System.Linq;
using CitizenFX.Core.Native;
using Newtonsoft.Json.Linq;

namespace Kilo.Commons.Config
{
    public class Config<T> where T : JContainer
    {
        public JContainer Store { get; }
        private readonly JContainer _defaultConfig;
        public string Name { get; }

        public Config(string resourceName, string path, JContainer defaultConfig = null)
        {
            path = path.Replace(resourceName, "");
            _defaultConfig = defaultConfig ?? new JObject();
            Store = LoadResourceFile(resourceName, path) ?? defaultConfig;
            Name = path.Split('/').Last().Replace("/", "").Trim();
        }

        public I Get<I>(string key)
        {
            if (Store[key] is null)
                throw new NullReferenceException($"^7[Config] ^8Failed to get ^2{key}^8 from ^3{Name}^8.");
            return Store[key]!.ToObject<I>();
        }

        public void Remove(string key)
        {
            if (Store is JObject obj)
                if (obj.ContainsKey(key))
                    obj.Remove(key);
                else if (Store is JArray arr)
                    arr.Remove(arr.First(x => x["key"].ToString() == key));
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