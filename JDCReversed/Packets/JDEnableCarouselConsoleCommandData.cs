using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdEnableCarouselConsoleCommandData : JdObject
{
    public JdEnableCarouselConsoleCommandData() : base("EnableCarousel_ConsoleCommandData")
    {
    }

    [JsonProperty("isEnabled")] 
    [JsonConverter(typeof(IntegerBooleanConverter))]
    public bool IsEnabled { get; set; }

    [JsonProperty("applyOnPopup")]
    [JsonConverter(typeof(IntegerBooleanConverter))]
    public bool ApplyOnPopup { get; set; }
}