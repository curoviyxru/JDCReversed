using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdPhoneDataCmdSyncStart : JdObject
{
    public JdPhoneDataCmdSyncStart() : base("JD_PhoneDataCmdSyncStart")
    {
    }

    [JsonProperty("phoneID")]
    public int PhoneId { get; set; }
}