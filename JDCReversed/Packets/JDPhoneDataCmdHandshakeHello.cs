using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdPhoneDataCmdHandshakeHello : JdObject
{
    public JdPhoneDataCmdHandshakeHello() : base("JD_PhoneDataCmdHandshakeHello")
    {
    }

    [JsonProperty("clientVersion")] public string? ClientVersion { get; set; }

    [JsonProperty("accelAcquisitionFreqHz")]
    public double AccelAcquisitionFreqHz { get; set; }

    [JsonProperty("accelAcquisitionLatency")]
    public double AccelAcquisitionLatency { get; set; }

    [JsonProperty("accelMaxRange")] public double AccelMaxRange { get; set; }

    [JsonProperty("jmcsToken")] public string? JmcsToken { get; set; }
}