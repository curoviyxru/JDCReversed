using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JDGoToConnectionPhoneCommandData : JdObject
{
    public JDGoToConnectionPhoneCommandData() : base("JD_GoToConnection_PhoneCommandData")
    {
    }

    [JsonProperty("connectionName")]
    public string? ConnectionName;
}