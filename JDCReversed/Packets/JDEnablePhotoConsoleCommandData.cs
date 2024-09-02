using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdEnablePhotoConsoleCommandData : JdObject
{
    public JdEnablePhotoConsoleCommandData() : base("EnablePhoto_ConsoleCommandData")
    {
    }

    [JsonProperty("isEnabled")]
    [JsonConverter(typeof(IntegerBooleanConverter))]
    public bool IsEnabled { get; set; }
}