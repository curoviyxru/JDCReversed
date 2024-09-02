using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdPhoneItemSlider : JdObject
{
    public JdPhoneItemSlider() : base("Phone_ItemSlider")
    {
    }

    [JsonProperty("title")]
    public string? Title { get; set; }

    [JsonProperty("image")]
    public string? Image { get; set; }

    [JsonProperty("isEnabled")]
    [JsonConverter(typeof(IntegerBooleanConverter))]
    public bool IsEnabled { get; set; }

    [JsonProperty("actions")]
    public JdPhoneAction[]? Actions { get; set; }
}