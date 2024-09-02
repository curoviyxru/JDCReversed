using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace JDCReversed.Packets;

public class JdCustomPhoneCommandData : JdObject
{
    public enum ActionIdentifier
    {
        PAUSE,
        SHORTCUT_DANCERPROFILE,
        SHORTCUT_UPLOAD,
        SHORTCUT_EXTRA,
        SHORTCUT_TAUNTMESSAGE,
        SHORTCUT_SEARCH,
        SHORTCUT_WDF_NEWS,
        SHORTCUT_CONNECT_PHONE,
        SHORTCUT_ON,
        SHORTCUT_OFF,
        SHORTCUT_BACK,
        SHORTCUT_CHANGE_DANCERCARD,
        SHORTCUT_FAVORITE,
        SHORTCUT_GOTO_SONGSTAB,
        SHORTCUT_SKIP,
        SHORTCUT_SORTING,
        SHORTCUT_SWAP_GENDER,
        SHORTCUT_SWEAT_ACTIVATION,
        SHORTCUT_TOGGLE_COOP,
        SHORTCUT_UPLAY,
        SHORTCUT_ACTIVATE_DANCERCARD,
        SHORTCUT_DELETE_DANCERCARD,
        SHORTCUT_DELETE_PLAYLIST,
        SHORTCUT_SAVE_PLAYLIST,
        SHORTCUT_PLAYLIST_RENAME,
        SHORTCUT_PLAYLIST_DELETE_SONG,
        SHORTCUT_PLAYLIST_MOVE_SONG_LEFT,
        SHORTCUT_PLAYLIST_MOVE_SONG_RIGHT,
        SHORTCUT_TIPS_NEXT,
        SHORTCUT_TIPS_PREVIOUS,
    }

    public JdCustomPhoneCommandData() : base("JD_Custom_PhoneCommandData")
    {
    }

    [JsonProperty("identifier")]
    [JsonConverter(typeof(StringEnumConverter))]
    public ActionIdentifier Identifier;
}