using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdDisabledScreenPhoneUiData : JdObject
{
    public JdDisabledScreenPhoneUiData() : base("JD_DisabledScreenPhoneUiData")
    {
    }

    [JsonProperty("displayText")] public string? DisplayText { get; set; }
}