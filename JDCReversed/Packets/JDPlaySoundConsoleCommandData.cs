using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdPlaySoundConsoleCommandData : JdObject
{
    public JdPlaySoundConsoleCommandData() : base("JD_PlaySound_ConsoleCommandData")
    {
    }

    [JsonProperty("soundIndex")]
    public int SoundIndex { get; set; }
}