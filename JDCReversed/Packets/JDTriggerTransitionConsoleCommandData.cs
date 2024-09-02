using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdTriggerTransitionConsoleCommandData : JdObject
{
    public JdTriggerTransitionConsoleCommandData() : base("JD_TriggerTransition_ConsoleCommandData")
    {
    }

    [JsonProperty("displayText")]
    public string? DisplayText { get; set; }
}