using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdInputPhoneCommandData : JdObject
{
    public enum ActionInput : long
    {
        UP = 3690595578,
        RIGHT = 1099935642,
        DOWN = 2467711647,
        LEFT = 3652315484,
        ACCEPT = 1084313942,
    }

    public JdInputPhoneCommandData() : base("JD_Input_PhoneCommandData")
    {
    }

    [JsonProperty("input")]
    public ActionInput Input;
}