using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdPlayerActivityPhoneCommandData : JdObject
{
    public JdPlayerActivityPhoneCommandData() : base("JD_PlayerActivity_PhoneCommandData")
    {
    }

    // Should be 0 (close photo) or 1 (open photo) probably
    [JsonProperty("activity")]
    [JsonConverter(typeof(IntegerBooleanConverter))]
    public bool Activity { get; set; }
}