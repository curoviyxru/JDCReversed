using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdOpenPhoneKeyboardConsoleCommandData : JdObject
{
    public JdOpenPhoneKeyboardConsoleCommandData() : base("JD_OpenPhoneKeyboard_ConsoleCommandData")
    {
    }

    [JsonProperty("isPassword")] public int IsPassword { get; set; } //TODO: integer boolean
}