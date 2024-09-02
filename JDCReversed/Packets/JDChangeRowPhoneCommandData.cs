using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdChangeRowPhoneCommandData : JdObject
{
    public JdChangeRowPhoneCommandData() : base("ChangeRow_PhoneCommandData")
    {
    }

    [JsonProperty("rowIndex")]
    public int RowIndex { get; set; }
}