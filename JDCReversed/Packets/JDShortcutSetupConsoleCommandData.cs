using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdShortcutSetupConsoleCommandData : JdObject
{
    public JdShortcutSetupConsoleCommandData() : base("ShortcutSetup_ConsoleCommandData")
    {
    }

    [JsonProperty("isEnabled")] public int IsEnabled { get; set; } //TODO: integer boolean
}