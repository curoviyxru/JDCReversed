using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdPhoneItemSlider : JdObject
{
    public JdPhoneItemSlider() : base("Phone_ItemSlider")
    {
    }

    [JsonProperty("title")] public string? Title { get; set; }

    [JsonProperty("image")] public string? Image { get; set; } //TODO: image URL destination?

    [JsonProperty("isEnabled")] public int IsEnabled { get; set; } //TODO: integer boolean

    [JsonProperty("actions")] public JdPhoneAction[]? Actions { get; set; }
}