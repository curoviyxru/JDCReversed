using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdPhoneCarouselRow : JdObject
{
    public JdPhoneCarouselRow() : base("Phone_CarouselRow")
    {
    }

    [JsonProperty("title")] public string? Title { get; set; } //TODO: formatted string

    [JsonProperty("items")] public JdPhoneItem[]? Items { get; set; }
}