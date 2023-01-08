using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdPhoneItem : JdObject
{
    public JdPhoneItem() : base("Phone_Item")
    {
    }

    [JsonProperty("title")] public string? Title { get; set; }

    [JsonProperty("image")] public string? Image { get; set; } //TODO: image URL destination?

    [JsonProperty("isEnabled")] public int IsEnabled { get; set; } //TODO: integer boolean

    [JsonProperty("isLocked")] public int IsLocked { get; set; } //TODO: integer boolean

    [JsonProperty("actions")] public JdPhoneAction[]? Actions { get; set; }
}