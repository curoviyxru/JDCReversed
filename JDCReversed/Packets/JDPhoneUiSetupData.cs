using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdPhoneUiSetupData : JdObject
{
    public JdPhoneUiSetupData() : base("JD_PhoneUiSetupData")
    {
    }

    [JsonProperty("isPopup")]
    [JsonConverter(typeof(IntegerBooleanConverter))]
    public bool IsPopup { get; set; }

    [JsonProperty("inputSetup")]
    public JdInputSetupConsoleCommandData? InputSetup { get; set; }
    
    [JsonProperty("disabledSetup")]
    public JdDisabledScreenPhoneUiData? DisabledSetup { get; set; }

    [JsonProperty("setupData")]
    public JdPhoneUiData? SetupData { get; set; }
}