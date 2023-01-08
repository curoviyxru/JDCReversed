using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdPhoneUiSetupData : JdObject
{
    public JdPhoneUiSetupData() : base("JD_PhoneUiSetupData")
    {
    }

    //TODO: integer boolean
    [JsonProperty("isPopup")] public int IsPopup { get; set; }
    [JsonProperty("inputSetup")] public JdInputSetupConsoleCommandData? InputSetup { get; set; }
    [JsonProperty("disabledSetup")] public JdDisabledScreenPhoneUiData? DisabledSetup { get; set; }
    [JsonProperty("setupData")] public JdPhoneUiData? SetupData { get; set; }
}