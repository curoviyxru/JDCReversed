using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdPhoneDataCmdHandshakeContinue : JdObject
{
    public JdPhoneDataCmdHandshakeContinue() : base("JD_PhoneDataCmdHandshakeContinue")
    {
    }

    [JsonProperty("jdVersion")]
    public string? JdVersion { get; set; }

    [JsonProperty("protocolVersion")]
    public int ProtocolVersion { get; set; }

    [JsonProperty("phoneID")]
    public int PhoneId { get; set; }

    [JsonProperty("platformName")]
    public string? PlatformName { get; set; }

    [JsonProperty("titleID")]
    public string? TitleId { get; set; }

    [JsonProperty("consoleName")]
    public string? ConsoleName { get; set; }
}