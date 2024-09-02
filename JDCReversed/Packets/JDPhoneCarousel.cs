using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdPhoneCarousel : JdObject
{
    public JdPhoneCarousel() : base("Phone_Carousel")
    {
    }

    [JsonProperty("hasVisibleActions")]
    [JsonConverter(typeof(IntegerBooleanConverter))]
    public bool HasVisibleActions { get; set; }

    [JsonProperty("isEnabled")]
    [JsonConverter(typeof(IntegerBooleanConverter))]
    public bool IsEnabled { get; set; }

    [JsonProperty("rows")]
    public JdPhoneCarouselRow[]? Rows { get; set; }
}