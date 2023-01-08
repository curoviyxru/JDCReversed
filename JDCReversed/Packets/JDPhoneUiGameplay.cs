using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdPhoneUiGameplay : JdObject
{
    public JdPhoneUiGameplay() : base("JD_PhoneUiGameplay")
    {
    }

    [JsonProperty("text")] public string? Text { get; set; }

    //TODO: image URL destination?
    [JsonProperty("coachImage")] public string? CoachImage { get; set; }
    [JsonProperty("pauseSlider")] public JdPhoneItemSlider? PauseSlider { get; set; }
}