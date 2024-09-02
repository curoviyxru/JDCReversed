using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdChangeActionPhoneCommandData : JdObject
{
    public JdChangeActionPhoneCommandData() : base("ChangeAction_PhoneCommandData")
    {
    }
    
    [JsonProperty("rowIndex")]
    public int RowIndex { get; set; }

    [JsonProperty("itemIndex")]
    public int ItemIndex { get; set; }
    
    [JsonProperty("actionIndex")]
    public int ActionIndex { get; set; }
}