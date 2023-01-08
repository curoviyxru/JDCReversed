using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdEnableCarouselConsoleCommandData : JdObject
{
    public JdEnableCarouselConsoleCommandData() : base("EnableCarousel_ConsoleCommandData")
    {
    }

    [JsonProperty("isEnabled")] public int IsEnabled { get; set; } //TODO: integer boolean

    [JsonProperty("applyOnPopup")] public int ApplyOnPopup { get; set; } //TODO: integer boolean
}