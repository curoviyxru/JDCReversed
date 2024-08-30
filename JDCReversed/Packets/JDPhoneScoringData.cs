using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JDCReversed.Packets;

public struct AccelDataItem
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }
}

public class AccelDataItemConverter : JsonConverter<AccelDataItem>
{
    public override void WriteJson(JsonWriter writer, AccelDataItem value, JsonSerializer serializer)
    {
        JArray array = [value.X, value.Y, value.Z];
        array.WriteTo(writer);
    }

    public override AccelDataItem ReadJson(JsonReader reader, Type objectType, AccelDataItem existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        JArray array = JArray.Load(reader);
        return new AccelDataItem {
            X = array[0].Value<double>(),
            Y = array[1].Value<double>(),
            Z = array[2].Value<double>()
        };
    }
}

public class JdPhoneScoringData : JdObject
{
    public JdPhoneScoringData() : base("JD_PhoneScoringData")
    {
    }

    //TODO: Total count of accel data items that were sent previously
    [JsonProperty("timestamp")]
    public int Timestamp { get; set; }

    //TODO: Maximum 10 items in array
    [JsonProperty("accelData")] 
    public AccelDataItem[]? AccelData { get; set; }
}