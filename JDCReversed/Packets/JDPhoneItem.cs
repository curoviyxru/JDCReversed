using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdPhoneItem : JdObject
{
    public JdPhoneItem() : base("Phone_Item")
    {
    }

    [JsonProperty("title")]
    public string? Title { get; set; }

    [JsonProperty("image")]
    public string? Image { get; set; } //TODO: image URL destination?

    [JsonProperty("isEnabled")]
    [JsonConverter(typeof(IntegerBooleanConverter))]
    public bool IsEnabled { get; set; }

    [JsonProperty("isLocked")] 
    [JsonConverter(typeof(IntegerBooleanConverter))]
    public bool IsLocked { get; set; }

    [JsonProperty("actions")]
    public JdPhoneAction[]? Actions { get; set; }
}