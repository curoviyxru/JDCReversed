using Newtonsoft.Json;

namespace JDCReversed.JDTV;

public class JdtvItem
{
    [JsonProperty("components")] public JdtvComponent[]? Components { get; set; }
}