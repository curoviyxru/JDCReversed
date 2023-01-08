using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdChangeCoachPhoneCommandData : JdObject
{
    public JdChangeCoachPhoneCommandData() : base("JD_ChangeCoach_PhoneCommandData")
    {
    }

    [JsonProperty("coachId")] public int CoachId { get; set; }
}