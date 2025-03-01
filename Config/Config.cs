using System;
using System.Collections.Generic;
using System.Linq;
using CitizenFX.Core.Native;
using Newtonsoft.Json.Linq;

namespace Kilo.Commons.Config
{
    public class Config<T>: IDocument
    {
        public JContainer Store { get; }
        private readonly JContainer _defaultConfig;
        public string Name { get; }

        public Config(string resourceName, string path, JContainer defaultConfig = null)
        {
            if (!typeof(T).IsInterface)
                throw new InvalidOperationException("Generic type T must be an interface.");
            path = path.Replace(resourceName, "");
            _defaultConfig = defaultConfig ?? new JObject();
            Store = LoadResourceFile(resourceName, path);
            Name = path.Split('/').Last().Replace("/", "").Trim();
        }
        
        public static T Create(string resourceName, string path, JContainer store)
        {
            return (T)Activator.CreateInstance(typeof(T), resourceName, path, store);
        }

        
        public object this[string key]
        {
            get
            {
                if (Store[key] is JToken token)
                    return token.ToObject(typeof(object)); // Convert JToken to an object automatically
                else
                    if (this._defaultConfig[key] is JToken token2)
                        return token2.ToObject(typeof(object));
                throw new KeyNotFoundException($"Key '{key}' not present in the config Store.");
            }
            set
            {
                if (Store is JObject obj)
                    obj[key] = JToken.FromObject(value);
            }
        }


        public I Get<I>(string key) where I : IDocument
        {
            if (Store[key] is null)
                throw new KeyNotFoundException($"^7[Config] ^8Failed to get ^2{key}^8 from ^3{Name}^8.");
            return (I)Activator.CreateInstance(typeof(I), Store[key]);
        }

        public void Remove(string key)
        {
            if (Store is JObject obj)
                if (obj.ContainsKey(key))
                    obj.Remove(key);
                else if (Store is JArray arr)
                    arr.Remove(arr.First(x => x["key"].ToString() == key));
        }

        public static JContainer LoadResourceFile(string resourceName, string path)
        {
            var file = API.LoadResourceFile(resourceName, path);
            if (file.StartsWith("[") && file.EndsWith("]"))
            {
                return JArray.Parse(file);
            }

            return JObject.Parse(file);
        }
    }
    
}