using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdChangeItemPhoneCommandData : JdObject
{
    public JdChangeItemPhoneCommandData() : base("ChangeItem_PhoneCommandData")
    {
    }

    [JsonProperty("rowIndex")] public int RowIndex { get; set; }

    [JsonProperty("itemIndex")] public int ItemIndex { get; set; }
}