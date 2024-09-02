using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdValidateActionPhoneCommandData : JdObject
{
    public JdValidateActionPhoneCommandData() : base("ValidateAction_PhoneCommandData")
    {
    }

    [JsonProperty("rowIndex")]
    public int RowIndex { get; set; }

    [JsonProperty("itemIndex")]
    public int ItemIndex { get; set; }

    [JsonProperty("actionIndex")]
    public int ActionIndex { get; set; }
}