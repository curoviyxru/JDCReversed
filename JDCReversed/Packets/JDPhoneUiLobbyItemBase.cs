using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdPhoneUiLobbyItemBase : JdObject
{
    public JdPhoneUiLobbyItemBase() : base("JD_PhoneUiLobbyItemBase")
    {
    }

    [JsonProperty("image")] public string? Image { get; set; } //TODO: image URL destination?

    [JsonProperty("selectedOnEnter")] public int SelectedOnEnter { get; set; } //TODO: integer boolean

    [JsonProperty("actions")] public JdPhoneAction[]? Actions { get; set; }
}