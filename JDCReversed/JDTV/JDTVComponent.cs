using Newtonsoft.Json;

namespace JDCReversed.JDTV;

public class JdtvComponent : JdObject
{
    [JsonProperty("ugcNumber")]
    public string? UgcNumber { get; set; }

    [JsonProperty("mapName")]
    public string? MapName { get; set; }

    [JsonProperty("ugcId")]
    public string? UgcId { get; set; }

    [JsonProperty("content")]
    public JdtvContent? Content { get; set; }

    [JsonProperty("views")]
    public int Views { get; set; }

    [JsonProperty("likes")]
    public int Likes { get; set; }

    [JsonProperty("like")]
    public bool Like { get; set; }

    [JsonProperty("comVideo")]
    public bool ComVideo { get; set; }

    [JsonProperty("time")]
    public long Time { get; set; } //in seconds

    [JsonProperty("profileId")]
    public string? ProfileId { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("avatar")]
    public int Avatar { get; set; }

    [JsonProperty("skin")]
    public int Skin { get; set; }

    [JsonProperty("country")]
    public int Country { get; set; }

    [JsonProperty("title")]
    public string? Title { get; set; }

    [JsonProperty("description")]
    public string? Description { get; set; }

    public class JdtvContent
    {
        [JsonProperty("autodance.mp4")]
        public JdtvLink? AutodanceVideo { get; set; }

        [JsonProperty("autodance_t0_m1.jpg")]
        public JdtvLink? AutodanceImage { get; set; }

        public class JdtvLink
        {
            [JsonProperty("url")]
            public string? Url { get; set; }
        }
    }
}