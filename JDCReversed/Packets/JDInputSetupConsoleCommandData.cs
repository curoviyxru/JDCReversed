using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdInputSetupConsoleCommandData : JdObject
{
    public JdInputSetupConsoleCommandData() : base("InputSetup_ConsoleCommandData")
    {
    }

    [JsonProperty("isEnabled")] public int IsEnabled { get; set; } //TODO: integer boolean

    [JsonProperty("applyOnPopup")] public int ApplyOnPopup { get; set; } //TODO: integer boolean

    [JsonProperty("carouselPosSetup")] public JdCarouselPosSetup? CarouselPosSetup { get; set; }
}