using Newtonsoft.Json;

namespace JDCReversed.JDTV;

public class JdtvCatalog
{
    [JsonProperty("categories")] public JdtvCategory[]? Categories { get; set; }
}