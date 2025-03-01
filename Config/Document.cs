using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Kilo.Commons.Config;

public class Document<T>: IDocument
{
    public JContainer Store { get; }
    public string Name { get; }
    public Document(JToken token)
    {
        Name = token.Path.Split('.').Last();
        Store = (JContainer)token;
    }
    
    public object this[string key]
    {
        get
        {
            if (Store[key] is JToken token)
                return token.ToObject(typeof(object)); // Convert JToken to an object automatically
            throw new KeyNotFoundException($"Key '{key}' not present in the config Store.");
        }
        set
        {
            if (Store is JObject obj)
                obj[key] = JToken.FromObject(value);
        }
    }

    public T As()
    {
        return (T)Activator.CreateInstance(typeof(T), this);
    }
    
    public I Get<I>(string key) where I : IDocument
    {
        if (Store[key] is null)
            throw new NullReferenceException($"^7[Config] ^8Failed to get ^2{key}^8 from ^3{Name}^8.");
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

    public Document<T> Set(string key, JToken value)
    {
        Store[key] = value;
        return this;
    }
}
