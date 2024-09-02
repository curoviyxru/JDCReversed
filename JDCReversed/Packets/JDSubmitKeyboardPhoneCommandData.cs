using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdSubmitKeyboardPhoneCommandData : JdObject
{
    public JdSubmitKeyboardPhoneCommandData() : base("JD_SubmitKeyboard_PhoneCommandData")
    {
    }

    [JsonProperty("keyboardOutput")]
    public string? KeyboardOutput { get; set; }
}