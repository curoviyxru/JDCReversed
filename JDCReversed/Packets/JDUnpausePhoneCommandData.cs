using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JDUnpausePhoneCommandData : JdObject
{
    public JDUnpausePhoneCommandData() : base("JD_Unpause_PhoneCommandData")
    {
    }

    [JsonProperty("status")]
    public string? Status;
}