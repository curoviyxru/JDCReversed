using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdPhoneUiLobby : JdObject
{
    public JdPhoneUiLobby() : base("JD_PhoneUiLobby")
    {
    }

    [JsonProperty("startButton")]
    public JdPhoneItem? StartButton { get; set; }
    
    [JsonProperty("coaches")]
    public JdPhoneUiLobbyItemBase[]? Coaches { get; set; }
}