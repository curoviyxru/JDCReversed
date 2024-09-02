using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdProfilePhoneUiData : JdObject
{
    public JdProfilePhoneUiData() : base("JD_ProfilePhoneUiData")
    {
    }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("playerId")]
    public int PlayerId { get; set; }

    //TODO: Color object (RGBA, 0.0-1.0)
    [JsonProperty("color")]
    public double[]? Color { get; set; }

    //TODO: image URL destination?
    [JsonProperty("image")]
    public string? Image { get; set; }

    //TODO: image URL destination?
    [JsonProperty("skinImage")]
    public string? SkinImage { get; set; }

    [JsonProperty("showOnScreen")]
    [JsonConverter(typeof(IntegerBooleanConverter))]
    public bool ShowOnScreen { get; set; }

    [JsonProperty("showConfirmButton")]
    [JsonConverter(typeof(IntegerBooleanConverter))]
    public bool ShowConfirmButton { get; set; }

    [JsonProperty("additionalMessage")]
    public string? AdditionalMessage { get; set; }
}