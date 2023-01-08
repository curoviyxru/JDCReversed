using Newtonsoft.Json;

namespace JDCReversed.JDTV;

public class JdtvCategory
{
    [JsonProperty("title")] public string? Title { get; set; }

    [JsonProperty("logoUrl")] public string? LogoUrl { get; set; }

    [JsonProperty("noItemsMsg")] public string? NoItemsMsg { get; set; }

    [JsonProperty("items")] public JdtvItem[]? Items { get; set; }
}