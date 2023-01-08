using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdPhoneDataCmdSync : JdObject
{
    public JdPhoneDataCmdSync() : base("JD_PhoneDataCmdSync")
    {
    }

    [JsonProperty("phoneID")] public int PhoneId { get; set; }
}