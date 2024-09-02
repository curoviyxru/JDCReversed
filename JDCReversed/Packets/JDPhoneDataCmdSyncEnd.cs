using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdPhoneDataCmdSyncEnd : JdObject
{
    public JdPhoneDataCmdSyncEnd() : base("JD_PhoneDataCmdSyncEnd")
    {
    }

    [JsonProperty("phoneID")]
    public int PhoneId { get; set; }
}