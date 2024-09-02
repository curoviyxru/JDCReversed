using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdPhoneUiLobbyItemBase : JdObject
{
    public JdPhoneUiLobbyItemBase() : base("JD_PhoneUiLobbyItemBase")
    {
    }

    [JsonProperty("image")]
    public string? Image { get; set; } //TODO: image URL destination?

    [JsonProperty("selectedOnEnter")]
    [JsonConverter(typeof(IntegerBooleanConverter))]
    public bool SelectedOnEnter { get; set; }

    [JsonProperty("actions")]
    public JdPhoneAction[]? Actions { get; set; }
}