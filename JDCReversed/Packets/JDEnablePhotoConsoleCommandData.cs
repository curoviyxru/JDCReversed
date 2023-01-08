using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdEnablePhotoConsoleCommandData : JdObject
{
    public JdEnablePhotoConsoleCommandData() : base("EnablePhoto_ConsoleCommandData")
    {
    }

    [JsonProperty("isEnabled")] public int IsEnabled { get; set; } //TODO: integer boolean
}