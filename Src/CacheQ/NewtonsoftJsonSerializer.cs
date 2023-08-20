using System.Text;
using Newtonsoft.Json;

namespace CacheQ;

public class NewtonsoftJsonSerializer : ISerializer
{
    private readonly JsonSerializerSettings _settings;

    public NewtonsoftJsonSerializer(JsonSerializerSettings settings)
    {
        _settings = settings;
    }

    public string Serialize<T>(T obj)
    {
        return JsonConvert.SerializeObject(obj, _settings);
    }

    public T Deserialize<T>(string data)
    {
        return JsonConvert.DeserializeObject<T>(data, _settings);
    }

    public byte[] SerializeToBytes<T>(T obj)
    {
        var serialized = Serialize(obj);
        return Encoding.UTF8.GetBytes(serialized);
    }

    public T DeserializeFromBytes<T>(byte[] data)
    {
        return Deserialize<T>(Encoding.UTF8.GetString(data));
    }
}
