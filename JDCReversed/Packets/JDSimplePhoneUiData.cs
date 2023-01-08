using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdSimplePhoneUiData : JdObject
{
    public JdSimplePhoneUiData() : base("JD_SimplePhoneUiData")
    {
    }

    [JsonProperty("displayText")] public string? DisplayText { get; set; }
}