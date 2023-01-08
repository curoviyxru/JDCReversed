using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdEnableLobbyStartbuttonConsoleCommandData : JdObject
{
    public JdEnableLobbyStartbuttonConsoleCommandData() : base("JD_EnableLobbyStartbutton_ConsoleCommandData")
    {
    }

    [JsonProperty("isEnabled")] public int IsEnabled { get; set; } //TODO: integer boolean
}