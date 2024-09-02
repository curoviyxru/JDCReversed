using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdEnableLobbyStartbuttonConsoleCommandData : JdObject
{
    public JdEnableLobbyStartbuttonConsoleCommandData() : base("JD_EnableLobbyStartbutton_ConsoleCommandData")
    {
    }

    [JsonProperty("isEnabled")]
    [JsonConverter(typeof(IntegerBooleanConverter))]
    public bool IsEnabled { get; set; }
}