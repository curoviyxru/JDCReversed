using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdPhoneUiData : JdObject
{
    public JdPhoneUiData() : base("JD_PhoneUiData")
    {
    }

    [JsonProperty("mainCarousel")]
    public JdPhoneCarousel? MainCarousel { get; set; }

    [JsonProperty("navigationBar")]
    public JdPhoneAction[]? NavigationBar { get; set; }

    [JsonProperty("shortcuts")]
    public JdPhoneAction[]? Shortcuts { get; set; }

    [JsonProperty("displayText")]
    public string? DisplayText { get; set; }

    [JsonProperty("lobbySetup")]
    public JdPhoneUiLobby? LobbySetup { get; set; }

    [JsonProperty("gameplaySetup")]
    public JdPhoneUiGameplay? GameplaySetup { get; set; }

    [JsonProperty("recapSetup")]
    public JdPhoneUiRecap? RecapSetup { get; set; }
}