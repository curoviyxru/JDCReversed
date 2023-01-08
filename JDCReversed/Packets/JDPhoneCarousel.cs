using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdPhoneCarousel : JdObject
{
    public JdPhoneCarousel() : base("Phone_Carousel")
    {
    }

    //TODO: integer boolean
    [JsonProperty("hasVisibleActions")] public int HasVisibleActions { get; set; }

    //TODO: integer boolean
    [JsonProperty("isEnabled")] public int IsEnabled { get; set; }
    [JsonProperty("rows")] public JdPhoneCarouselRow[]? Rows { get; set; }
}