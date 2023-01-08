using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdPhoneUiCoachItem : JdObject
{
    public JdPhoneUiCoachItem() : base("JD_PhoneUiCoachItem")
    {
    }

    //TODO: image URL destination?
    [JsonProperty("image")] public string? Image { get; set; }

    //TODO: integer boolean
    [JsonProperty("selectedOnEnter")] public int SelectedOnEnter { get; set; }
    [JsonProperty("actions")] public JdPhoneAction[]? Actions { get; set; }
}