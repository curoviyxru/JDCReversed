using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdInputSetupConsoleCommandData : JdObject
{
    public JdInputSetupConsoleCommandData() : base("InputSetup_ConsoleCommandData")
    {
    }

    [JsonProperty("isEnabled")]
    [JsonConverter(typeof(IntegerBooleanConverter))]
    public bool IsEnabled { get; set; }

    [JsonProperty("applyOnPopup")]
    [JsonConverter(typeof(IntegerBooleanConverter))]
    public bool ApplyOnPopup { get; set; }

    [JsonProperty("carouselPosSetup")]
    public JdCarouselPosSetup? CarouselPosSetup { get; set; }
}