using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdPhoneUiCoachItem : JdObject
{
    public JdPhoneUiCoachItem() : base("JD_PhoneUiCoachItem")
    {
    }

    //TODO: image URL destination?
    [JsonProperty("image")]
    public string? Image { get; set; }

    [JsonProperty("selectedOnEnter")]
    [JsonConverter(typeof(IntegerBooleanConverter))]
    public bool SelectedOnEnter { get; set; }

    [JsonProperty("actions")]
    public JdPhoneAction[]? Actions { get; set; }
}