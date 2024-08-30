using JDCReversed.JDTV;
using JDCReversed.Packets;
using Newtonsoft.Json;

namespace JDCReversed;

public class JdObject
{
    [JsonConstructor]
    protected JdObject(string? className = null)
    {
        ClassName = className ?? throw new ArgumentNullException(nameof(className));
    }

    [JsonProperty("__class")] public string ClassName { get; private set; }

    public static JdObject? Deserialize(string data)
    {
        //TODO: automatic registration
        //TODO: JsonConverter
        //TODO: JD_Custom_PhoneCommandData from modern client and other new packets
        var dict = new Dictionary<string, Type>
        {
            { "JD_PhoneDataCmdHandshakeHello", typeof(JdPhoneDataCmdHandshakeHello) },
            { "JD_PhoneDataCmdHandshakeContinue", typeof(JdPhoneDataCmdHandshakeContinue) },
            { "JD_PhoneDataCmdSync", typeof(JdPhoneDataCmdSync) },
            { "JD_PhoneDataCmdSyncStart", typeof(JdPhoneDataCmdSyncStart) },
            { "JD_PhoneDataCmdSyncEnd", typeof(JdPhoneDataCmdSyncEnd) },
            { "JD_EnableAccelValuesSending_ConsoleCommandData", typeof(JdEnableAccelValuesSendingConsoleCommandData) },
            { "JD_DisableAccelValuesSending_ConsoleCommandData", typeof(JdDisableAccelValuesSendingConsoleCommandData) },
            { "JD_PhoneUiSetupData", typeof(JdPhoneUiSetupData) },
            { "InputSetup_ConsoleCommandData", typeof(JdInputSetupConsoleCommandData) },
            { "JD_ProfilePhoneUiData", typeof(JdProfilePhoneUiData) },
            { "JD_PlaySound_ConsoleCommandData", typeof(JdPlaySoundConsoleCommandData) },
            { "JD_TriggerTransition_ConsoleCommandData", typeof(JdTriggerTransitionConsoleCommandData) },
            { "EnableCarousel_ConsoleCommandData", typeof(JdEnableCarouselConsoleCommandData) },
            { "JD_EnableLobbyStartbutton_ConsoleCommandData", typeof(JdEnableLobbyStartbuttonConsoleCommandData) },
            { "JD_NewPhoto_ConsoleCommandData", typeof(JdNewPhotoConsoleCommandData) },
            { "JD_ClosePopup_ConsoleCommandData", typeof(JdClosePopupConsoleCommandData) },
            { "ShortcutSetup_ConsoleCommandData", typeof(JdShortcutSetupConsoleCommandData) },
            { "JD_OpenPhoneKeyboard_ConsoleCommandData", typeof(JdOpenPhoneKeyboardConsoleCommandData) },
            { "EnablePhoto_ConsoleCommandData", typeof(JdEnablePhotoConsoleCommandData) },
            { "CarouselPosSetup", typeof(JdCarouselPosSetup) },
            { "JD_DisabledScreenPhoneUiData", typeof(JdDisabledScreenPhoneUiData) },
            { "JD_PhoneUiData", typeof(JdPhoneUiData) },
            { "Phone_Carousel", typeof(JdPhoneCarousel) },
            { "Phone_CarouselRow", typeof(JdPhoneCarouselRow) },
            { "Phone_Item", typeof(JdPhoneItem) },
            { "Phone_Action", typeof(JdPhoneAction) },
            { "JD_PlayerActivity_PhoneCommandData", typeof(JdPlayerActivityPhoneCommandData) },
            { "ChangeAction_PhoneCommandData", typeof(JdChangeActionPhoneCommandData) },
            { "ChangeItem_PhoneCommandData", typeof(JdChangeItemPhoneCommandData) },
            { "ChangeRow_PhoneCommandData", typeof(JdChangeRowPhoneCommandData) },
            { "JD_PhoneScoringData", typeof(JdPhoneScoringData) },
            { "ValidateAction_PhoneCommandData", typeof(JdValidateActionPhoneCommandData) },
            { "JD_CancelKeyboard_PhoneCommandData", typeof(JdCancelKeyboardPhoneCommandData) },
            { "JD2015_NotPhoneScoring", typeof(Jd2015NotPhoneScoring) },
            { "JD_PhoneUiLobby", typeof(JdPhoneUiLobby) },
            { "JD_PhoneUiLobbyItemBase", typeof(JdPhoneUiLobbyItemBase) },
            { "JD_PhoneUiGameplay", typeof(JdPhoneUiGameplay) },
            { "Phone_ItemSlider", typeof(JdPhoneItemSlider) },
            { "JD_PhoneUiRecap", typeof(JdPhoneUiRecap) },
            { "JD_PhoneUiCoachItem", typeof(JdPhoneUiCoachItem) },
            { "JD_ChangeCoach_PhoneCommandData", typeof(JdChangeCoachPhoneCommandData) },
            { "JD_Pause_PhoneCommandData", typeof(JdPausePhoneCommandData) },
            { "JD_StartGame_PhoneCommandData", typeof(JdStartGamePhoneCommandData) },
            { "JD_SubmitKeyboard_PhoneCommandData", typeof(JdSubmitKeyboardPhoneCommandData) },
            { "JD_SimplePhoneUiData", typeof(JdSimplePhoneUiData) },
            { "JD_CarouselContentComponent_JDM", typeof(JdtvComponent) },
            { "JD_CarouselContentComponent_Autodance", typeof(JdtvComponent) },
            { "JD_CarouselContentComponent_Dancer", typeof(JdtvComponent) },
            { "JD_CarouselContentComponent_Song", typeof(JdtvComponent) }
        };
        var stub = JsonConvert.DeserializeObject<JdObject>(data);
        if (stub == null || !dict.ContainsKey(stub.ClassName))
            return stub;

        var className =
            stub.ClassName.Trim().Split(new[] { "::" }, StringSplitOptions.None).LastOrDefault() ??
            string.Empty;
        return JsonConvert.DeserializeObject(data, dict[className], new JsonSerializerSettings
        {
            MissingMemberHandling = MissingMemberHandling.Error
        }) as JdObject ?? stub;
    }
}