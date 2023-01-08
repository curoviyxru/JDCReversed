using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdPlayerActivityPhoneCommandData : JdObject
{
    public JdPlayerActivityPhoneCommandData() : base("JD_PlayerActivity_PhoneCommandData")
    {
    }

    [JsonProperty("activity")] public int Activity { get; set; }
}