﻿using Newtonsoft.Json;

namespace JDCReversed.Packets;

public class JdOpenPhoneKeyboardConsoleCommandData : JdObject
{
    public JdOpenPhoneKeyboardConsoleCommandData() : base("JD_OpenPhoneKeyboard_ConsoleCommandData")
    {
    }

    [JsonProperty("isPassword")]
    [JsonConverter(typeof(IntegerBooleanConverter))]
    public bool IsPassword { get; set; }
}