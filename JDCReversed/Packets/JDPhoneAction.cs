using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdPhoneAction : JdObject
{
    //(why is it sending without "root" in code ???)
    //(on validation triggered -> if "root" exists -> change class "ChangeAction_PhoneCommandData" ???)

    public JdPhoneAction() : base("Phone_Action")
    {
    }

    [JsonProperty("title")] public string? Title { get; set; }

    //TODO: json string / JD action object that should be sent on click
    [JsonProperty("command")] public string? Command { get; set; }
}