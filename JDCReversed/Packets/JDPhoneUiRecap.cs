using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdPhoneUiRecap : JdObject
{
    public JdPhoneUiRecap() : base("JD_PhoneUiRecap")
    {
    }

    [JsonProperty("recapType")]
    public string? RecapType { get; set; }

    [JsonProperty("recapData")]
    public RecapDataSet? RecapData { get; set; }

    [JsonProperty("recapDisplayText")]
    public string? RecapDisplayText { get; set; }

    public class RecapDataSet
    {
        [JsonProperty("songScore")]
        public int SongScore { get; set; }

        [JsonProperty("stars")]
        public int Stars { get; set; }
    }
}