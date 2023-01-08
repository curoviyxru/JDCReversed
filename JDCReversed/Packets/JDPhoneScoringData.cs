using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdPhoneScoringData : JdObject
{
    //TODO: Coordinate data object (XYZ, double)

    public JdPhoneScoringData() : base("JD_PhoneScoringData")
    {
    }

    //TODO: Total count of accel data items that were sent previously
    [JsonProperty("timestamp")] public int Timestamp { get; set; }

    //TODO: Maximum 10 items in array
    [JsonProperty("accelData")] public double[][]? AccelData { get; set; }
}