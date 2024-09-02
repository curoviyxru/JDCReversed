using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdShortcutSetupConsoleCommandData : JdObject
{
    public JdShortcutSetupConsoleCommandData() : base("ShortcutSetup_ConsoleCommandData")
    {
    }

    [JsonProperty("isEnabled")]
    [JsonConverter(typeof(IntegerBooleanConverter))]
    public bool IsEnabled { get; set; }
}