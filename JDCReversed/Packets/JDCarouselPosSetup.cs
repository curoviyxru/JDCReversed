using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdCarouselPosSetup : JdObject
{
    public JdCarouselPosSetup() : base("CarouselPosSetup")
    {
    }

    [JsonProperty("rowIndex")]
    public int RowIndex { get; set; }

    [JsonProperty("itemIndex")]
    public int ItemIndex { get; set; }

    [JsonProperty("actionIndex")]
    public int ActionIndex { get; set; }
}