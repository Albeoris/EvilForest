namespace EvilForest.EventSource;

public static class Globals
{
    [StorageView(StorageScope.Global, StorageType.UInt16, 0)]
    public static int ScenarioCounter
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 2)]
    public static int FieldEntrance
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 4)]
    public static int Byte_4
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 5)]
    public static int Byte_5
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 6)]
    public static int Byte_6
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 7)]
    public static int Byte_7
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 8)]
    public static int Byte_8
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 9)]
    public static int EnvironmentSoundSlot0Id
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 11)]
    public static int EnvironmentSoundSlot1Id
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 13)]
    public static int EnvironmentSoundSlot0State
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 14)]
    public static int EnvironmentSoundSlot1State
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 16)]
    public static int TranceGaugeFlag
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 17)]
    public static int GarnetDepressionFlag
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 18)]
    public static int Byte_18
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 19)]
    public static int UInt16_19
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 21)]
    public static int UInt16_21
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.SByte, 28)]
    public static int SByte_28
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.SByte, 60)]
    public static int SByte_60
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.SByte, 61)]
    public static int SByte_61
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.SByte, 62)]
    public static int SByte_62
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.SByte, 63)]
    public static int SByte_63
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 64)]
    public static int Int24_64
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 67)]
    public static int Int16_67
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 69)]
    public static int Int24_69
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 72)]
    public static int Byte_72
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 74)]
    public static int Int24_74
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 77)]
    public static int Int16_77
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 79)]
    public static int Int24_79
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 82)]
    public static int Byte_82
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 83)]
    public static int Int24_83
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 86)]
    public static int Int16_86
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 88)]
    public static int Int24_88
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 91)]
    public static int Byte_91
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 104)]
    public static int Byte_104
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 112)]
    public static int Int24_112
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 115)]
    public static int Int24_115
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 118)]
    public static int Int16_118
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 129)]
    public static int Byte_129
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 131)]
    public static int Byte_131
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 132)]
    public static int Byte_132
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 134)]
    public static int Byte_134
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 136)]
    public static int UInt16_136
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 138)]
    public static int Byte_138
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 139)]
    public static int Byte_139
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 140)]
    public static int Int16_140
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 142)]
    public static int UInt16_142
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 146)]
    public static int UInt16_146
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 148)]
    public static int UInt16_148
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 150)]
    public static int UInt16_150
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 152)]
    public static int UInt16_152
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 154)]
    public static int UInt16_154
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 156)]
    public static int UInt16_156
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 158)]
    public static int Byte_158
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 159)]
    public static int Byte_159
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 160)]
    public static int Byte_160
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 161)]
    public static int Byte_161
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 162)]
    public static int Byte_162
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 163)]
    public static int Byte_163
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 164)]
    public static int Byte_164
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 165)]
    public static int Byte_165
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 166)]
    public static int Byte_166
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 167)]
    public static int Byte_167
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 168)]
    public static int Byte_168
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 171)]
    public static int Byte_171
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 172)]
    public static int Int24_172
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 175)]
    public static int Int16_175
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 178)]
    public static int UInt16_178
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 182)]
    public static int UInt16_182
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 184)]
    public static bool Bit_184
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 184)]
    public static int Int24_184
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 187)]
    public static int Int24_187
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 189)]
    public static bool Bit_189
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 190)]
    public static int Byte_190
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 191)]
    public static bool Bit_191
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 191)]
    public static int Byte_191
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 196)]
    public static bool Bit_196
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 197)]
    public static bool Bit_197
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 198)]
    public static bool Bit_198
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 198)]
    public static int Byte_198
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 199)]
    public static bool Bit_199
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 200)]
    public static int Byte_200
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 208)]
    public static int Byte_208
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 209)]
    public static int Byte_209
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 210)]
    public static int Byte_210
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 211)]
    public static int Byte_211
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 212)]
    public static int Byte_212
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 213)]
    public static int Byte_213
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 214)]
    public static int Int16_214
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 216)]
    public static int Int16_216
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 218)]
    public static int Int16_218
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 220)]
    public static int Int16_220
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 222)]
    public static int Int16_222
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 224)]
    public static int Int16_224
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 226)]
    public static int Byte_226
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 228)]
    public static int Int16_228
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 234)]
    public static int UInt16_234
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 236)]
    public static int UInt16_236
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.SByte, 238)]
    public static int SByte_238
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 239)]
    public static int Int16_239
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 241)]
    public static int Int16_241
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 243)]
    public static int Int16_243
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 245)]
    public static int Int16_245
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 247)]
    public static int Int16_247
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 249)]
    public static int Byte_249
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 250)]
    public static int Byte_250
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 251)]
    public static int UInt16_251
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 256)]
    public static int Byte_256
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 257)]
    public static int Byte_257
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 258)]
    public static int Byte_258
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 259)]
    public static int Byte_259
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 260)]
    public static int Byte_260
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 263)]
    public static int Byte_263
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 264)]
    public static int Byte_264
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 265)]
    public static int Byte_265
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 266)]
    public static int Byte_266
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 267)]
    public static int Int16_267
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 269)]
    public static int Int16_269
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 271)]
    public static int Int16_271
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 273)]
    public static int Int16_273
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 275)]
    public static int Int16_275
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 277)]
    public static int Int16_277
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 279)]
    public static int Byte_279
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 280)]
    public static int Byte_280
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 281)]
    public static int Byte_281
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 282)]
    public static int Byte_282
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 283)]
    public static int Byte_283
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 284)]
    public static int Byte_284
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 285)]
    public static int Byte_285
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 286)]
    public static int Byte_286
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 287)]
    public static int Byte_287
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 288)]
    public static int Byte_288
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 289)]
    public static int Byte_289
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 290)]
    public static int Byte_290
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 291)]
    public static int Byte_291
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 292)]
    public static int Byte_292
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.SByte, 293)]
    public static int SByte_293
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 294)]
    public static int UInt16_294
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.SByte, 296)]
    public static int SByte_296
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 297)]
    public static int UInt16_297
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 299)]
    public static int Byte_299
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 303)]
    public static int Byte_303
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 312)]
    public static int Byte_312
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 313)]
    public static int Byte_313
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 314)]
    public static int UInt16_314
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 316)]
    public static int UInt16_316
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 318)]
    public static int UInt16_318
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 320)]
    public static int UInt16_320
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 322)]
    public static int UInt16_322
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 329)]
    public static int Byte_329
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 333)]
    public static int UInt16_333
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 335)]
    public static int UInt16_335
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 355)]
    public static int UInt16_355
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 358)]
    public static int Byte_358
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 361)]
    public static int Byte_361
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 362)]
    public static int Byte_362
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 363)]
    public static int Byte_363
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 369)]
    public static int Byte_369
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 370)]
    public static int Byte_370
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 396)]
    public static int UInt16_396
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 398)]
    public static int Byte_398
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 399)]
    public static int Byte_399
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 400)]
    public static int Byte_400
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 401)]
    public static int Byte_401
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 408)]
    public static int Byte_408
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 409)]
    public static int Byte_409
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 418)]
    public static int Int16_418
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 420)]
    public static int Int16_420
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 426)]
    public static int Int24_426
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 429)]
    public static int Int24_429
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 433)]
    public static int Byte_433
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 434)]
    public static int Byte_434
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 435)]
    public static int Int16_435
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 437)]
    public static int Int16_437
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 439)]
    public static int Int16_439
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 441)]
    public static int Byte_441
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 442)]
    public static int Byte_442
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 444)]
    public static int Byte_444
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 445)]
    public static int Int24_445
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 450)]
    public static int Byte_450
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 455)]
    public static int Byte_455
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 456)]
    public static int Int16_456
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 458)]
    public static int Byte_458
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 465)]
    public static int Byte_465
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 467)]
    public static int Byte_467
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 468)]
    public static int Byte_468
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 469)]
    public static int Int16_469
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 471)]
    public static int Byte_471
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 472)]
    public static int Byte_472
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 475)]
    public static int Byte_475
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 479)]
    public static int Byte_479
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 483)]
    public static int Byte_483
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 484)]
    public static int Byte_484
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 489)]
    public static int Byte_489
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 490)]
    public static int Byte_490
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 491)]
    public static int Byte_491
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 492)]
    public static int Byte_492
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 496)]
    public static int Int24_496
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 499)]
    public static int Int24_499
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 502)]
    public static int Int24_502
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 506)]
    public static int UInt16_506
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 508)]
    public static int Byte_508
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 509)]
    public static int Byte_509
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 736)]
    public static bool Bit_736
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 737)]
    public static bool Bit_737
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 738)]
    public static bool Bit_738
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 739)]
    public static bool Bit_739
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 740)]
    public static bool Bit_740
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 741)]
    public static bool Bit_741
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 742)]
    public static bool Bit_742
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 743)]
    public static bool Bit_743
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 744)]
    public static bool Bit_744
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 745)]
    public static bool Bit_745
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 746)]
    public static bool Bit_746
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 747)]
    public static bool Bit_747
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 748)]
    public static bool Bit_748
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 749)]
    public static bool Bit_749
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 750)]
    public static bool Bit_750
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 751)]
    public static bool Bit_751
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 752)]
    public static bool Bit_752
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 753)]
    public static bool Bit_753
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 754)]
    public static bool Bit_754
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 755)]
    public static bool Bit_755
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 756)]
    public static bool Bit_756
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 757)]
    public static bool Bit_757
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 758)]
    public static bool Bit_758
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 759)]
    public static bool Bit_759
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 760)]
    public static bool Bit_760
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 761)]
    public static bool Bit_761
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 763)]
    public static bool Bit_763
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 765)]
    public static bool Bit_765
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 766)]
    public static bool Bit_766
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 767)]
    public static bool Bit_767
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 768)]
    public static bool Bit_768
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 768)]
    public static int Int24_768
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 769)]
    public static bool Bit_769
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 770)]
    public static bool Bit_770
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 771)]
    public static bool Bit_771
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 771)]
    public static int Int24_771
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 772)]
    public static bool Bit_772
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 773)]
    public static bool Bit_773
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 774)]
    public static int Int24_774
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 775)]
    public static bool Bit_775
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 776)]
    public static bool Bit_776
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 777)]
    public static bool Bit_777
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 777)]
    public static int Int24_777
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 780)]
    public static bool Bit_780
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 780)]
    public static int Byte_780
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 781)]
    public static bool Bit_781
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 781)]
    public static int Byte_781
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 782)]
    public static bool Bit_782
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 782)]
    public static int Byte_782
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 783)]
    public static bool Bit_783
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 783)]
    public static int Byte_783
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.SByte, 784)]
    public static int SByte_784
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 785)]
    public static bool Bit_785
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 785)]
    public static int Byte_785
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 786)]
    public static int Byte_786
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 787)]
    public static int Byte_787
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 788)]
    public static bool Bit_788
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 788)]
    public static int Byte_788
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 789)]
    public static bool Bit_789
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 789)]
    public static int Byte_789
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 790)]
    public static int Byte_790
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 791)]
    public static int Byte_791
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 792)]
    public static int Byte_792
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 793)]
    public static int Byte_793
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 794)]
    public static int Byte_794
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 795)]
    public static bool Bit_795
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 795)]
    public static int Byte_795
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 796)]
    public static int Byte_796
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 797)]
    public static int Byte_797
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 798)]
    public static int Byte_798
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 799)]
    public static int Byte_799
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 800)]
    public static int Byte_800
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 801)]
    public static int Byte_801
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 802)]
    public static int Byte_802
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 803)]
    public static int UInt16_803
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 805)]
    public static int Byte_805
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 806)]
    public static int Byte_806
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 807)]
    public static int Byte_807
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 808)]
    public static bool Bit_808
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 808)]
    public static int Byte_808
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 809)]
    public static bool Bit_809
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 809)]
    public static int Byte_809
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 810)]
    public static bool Bit_810
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 810)]
    public static int Byte_810
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 811)]
    public static int Byte_811
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 812)]
    public static int Byte_812
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 813)]
    public static int Byte_813
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 814)]
    public static int Byte_814
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 815)]
    public static int Byte_815
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 816)]
    public static int Byte_816
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 817)]
    public static int Byte_817
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 818)]
    public static int Byte_818
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 819)]
    public static int Byte_819
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 820)]
    public static int Byte_820
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 848)]
    public static bool Bit_848
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 849)]
    public static bool Bit_849
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 852)]
    public static bool Bit_852
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 853)]
    public static bool Bit_853
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 888)]
    public static bool Bit_888
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 889)]
    public static bool Bit_889
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 890)]
    public static bool Bit_890
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 891)]
    public static bool Bit_891
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 892)]
    public static bool Bit_892
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 893)]
    public static bool Bit_893
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 894)]
    public static bool Bit_894
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 895)]
    public static bool Bit_895
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 896)]
    public static int Byte_896
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 897)]
    public static int Byte_897
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 898)]
    public static int Byte_898
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 899)]
    public static int Byte_899
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 900)]
    public static int Byte_900
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 901)]
    public static int Byte_901
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 902)]
    public static int Byte_902
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 903)]
    public static int Byte_903
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 904)]
    public static int Byte_904
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 905)]
    public static int Byte_905
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 906)]
    public static int Byte_906
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 907)]
    public static int Byte_907
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 908)]
    public static int Byte_908
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 909)]
    public static int Byte_909
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 910)]
    public static int Byte_910
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 911)]
    public static int Byte_911
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 912)]
    public static int Byte_912
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 913)]
    public static int Byte_913
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 914)]
    public static int Byte_914
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 915)]
    public static int Byte_915
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 916)]
    public static int Byte_916
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 917)]
    public static int Byte_917
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 918)]
    public static int Byte_918
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 919)]
    public static int Byte_919
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 920)]
    public static int Byte_920
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 921)]
    public static int Byte_921
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 922)]
    public static int Byte_922
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 923)]
    public static int Byte_923
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 924)]
    public static int Byte_924
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 925)]
    public static int Byte_925
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 926)]
    public static int Byte_926
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 927)]
    public static int Byte_927
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 928)]
    public static int Byte_928
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 929)]
    public static int Byte_929
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 930)]
    public static int Byte_930
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 931)]
    public static int Byte_931
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 932)]
    public static int Byte_932
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 933)]
    public static int Byte_933
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 934)]
    public static int Byte_934
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 935)]
    public static int Byte_935
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 936)]
    public static int Byte_936
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 937)]
    public static int Byte_937
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 938)]
    public static int Byte_938
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 939)]
    public static int Byte_939
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 940)]
    public static int Byte_940
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 941)]
    public static int Byte_941
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 942)]
    public static int Byte_942
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 943)]
    public static int Byte_943
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 944)]
    public static int Byte_944
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 945)]
    public static int Byte_945
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 946)]
    public static int Byte_946
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 947)]
    public static int Byte_947
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 948)]
    public static int Byte_948
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 949)]
    public static int Byte_949
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 950)]
    public static int Byte_950
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 951)]
    public static int Byte_951
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 952)]
    public static int Byte_952
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 953)]
    public static int Byte_953
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 954)]
    public static int Byte_954
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 955)]
    public static int Byte_955
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 956)]
    public static int Byte_956
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 957)]
    public static int Byte_957
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 958)]
    public static int Byte_958
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 959)]
    public static int Byte_959
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 960)]
    public static int Byte_960
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 966)]
    public static int Byte_966
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 967)]
    public static int Byte_967
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 968)]
    public static int Byte_968
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 969)]
    public static int Byte_969
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 970)]
    public static int Byte_970
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 971)]
    public static int Byte_971
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 972)]
    public static int Byte_972
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 973)]
    public static int Byte_973
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 974)]
    public static int Byte_974
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 975)]
    public static int Byte_975
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1024)]
    public static int Byte_1024
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1032)]
    public static int Byte_1032
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1033)]
    public static int Byte_1033
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1034)]
    public static int Byte_1034
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1035)]
    public static int Byte_1035
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1036)]
    public static int Byte_1036
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1037)]
    public static int Byte_1037
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1038)]
    public static int Byte_1038
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1039)]
    public static int Byte_1039
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1040)]
    public static bool Bit_1040
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1040)]
    public static int Byte_1040
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1041)]
    public static bool Bit_1041
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1041)]
    public static int Byte_1041
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1042)]
    public static bool Bit_1042
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1042)]
    public static int Byte_1042
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1043)]
    public static bool Bit_1043
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1043)]
    public static int Byte_1043
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1044)]
    public static bool Bit_1044
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1044)]
    public static int Byte_1044
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1045)]
    public static bool Bit_1045
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1045)]
    public static int Byte_1045
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1046)]
    public static bool Bit_1046
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1047)]
    public static bool Bit_1047
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1064)]
    public static bool Bit_1064
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1064)]
    public static int Byte_1064
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1065)]
    public static bool Bit_1065
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1065)]
    public static int Byte_1065
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1066)]
    public static bool Bit_1066
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1066)]
    public static int Byte_1066
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1067)]
    public static bool Bit_1067
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1067)]
    public static int Byte_1067
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1068)]
    public static bool Bit_1068
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1068)]
    public static int Byte_1068
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1069)]
    public static bool Bit_1069
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1069)]
    public static int Byte_1069
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1070)]
    public static bool Bit_1070
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1070)]
    public static int Byte_1070
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1071)]
    public static bool Bit_1071
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1071)]
    public static int Byte_1071
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1072)]
    public static int Byte_1072
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1073)]
    public static int Byte_1073
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1079)]
    public static int Byte_1079
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1080)]
    public static bool Bit_1080
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1080)]
    public static int Byte_1080
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1081)]
    public static bool Bit_1081
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1081)]
    public static int Byte_1081
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1082)]
    public static bool Bit_1082
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1082)]
    public static int Byte_1082
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1083)]
    public static bool Bit_1083
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1083)]
    public static int Byte_1083
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1084)]
    public static bool Bit_1084
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1084)]
    public static int Byte_1084
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1085)]
    public static int Byte_1085
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1086)]
    public static bool Bit_1086
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1086)]
    public static int Byte_1086
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1087)]
    public static bool Bit_1087
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1087)]
    public static int Byte_1087
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1088)]
    public static int Byte_1088
    {
        get => ScriptMemory.ReadView(typeof(Globals));
        set => ScriptMemory.WriteView(typeof(Globals), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1156)]
    public static bool Bit_1156
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1157)]
    public static bool Bit_1157
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1158)]
    public static bool Bit_1158
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1159)]
    public static bool Bit_1159
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1416)]
    public static bool Bit_1416
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1417)]
    public static bool Bit_1417
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1419)]
    public static bool Bit_1419
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1421)]
    public static bool Bit_1421
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1422)]
    public static bool Bit_1422
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1423)]
    public static bool Bit_1423
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1463)]
    public static bool Bit_1463
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1584)]
    public static bool Bit_1584
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1816)]
    public static bool Bit_1816
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2048)]
    public static bool Bit_2048
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2049)]
    public static bool Bit_2049
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2050)]
    public static bool Bit_2050
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2051)]
    public static bool Bit_2051
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2052)]
    public static bool Bit_2052
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2053)]
    public static bool Bit_2053
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2054)]
    public static bool Bit_2054
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2055)]
    public static bool Bit_2055
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2056)]
    public static bool Bit_2056
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2057)]
    public static bool Bit_2057
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2058)]
    public static bool Bit_2058
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2059)]
    public static bool Bit_2059
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2060)]
    public static bool Bit_2060
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2061)]
    public static bool Bit_2061
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2062)]
    public static bool Bit_2062
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2063)]
    public static bool Bit_2063
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2064)]
    public static bool Bit_2064
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2065)]
    public static bool Bit_2065
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2066)]
    public static bool Bit_2066
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2067)]
    public static bool Bit_2067
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2068)]
    public static bool Bit_2068
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2069)]
    public static bool Bit_2069
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2070)]
    public static bool Bit_2070
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2071)]
    public static bool Bit_2071
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2072)]
    public static bool Bit_2072
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2073)]
    public static bool Bit_2073
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2074)]
    public static bool Bit_2074
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2075)]
    public static bool Bit_2075
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2076)]
    public static bool Bit_2076
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2077)]
    public static bool Bit_2077
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2078)]
    public static bool Bit_2078
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2079)]
    public static bool Bit_2079
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2080)]
    public static bool Bit_2080
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2081)]
    public static bool Bit_2081
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2082)]
    public static bool Bit_2082
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2083)]
    public static bool Bit_2083
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2084)]
    public static bool Bit_2084
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2085)]
    public static bool Bit_2085
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2086)]
    public static bool Bit_2086
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2087)]
    public static bool Bit_2087
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2088)]
    public static bool Bit_2088
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2090)]
    public static bool Bit_2090
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2091)]
    public static bool Bit_2091
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2092)]
    public static bool Bit_2092
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2093)]
    public static bool Bit_2093
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2095)]
    public static bool Bit_2095
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2096)]
    public static bool Bit_2096
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2097)]
    public static bool Bit_2097
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2098)]
    public static bool Bit_2098
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2099)]
    public static bool Bit_2099
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2100)]
    public static bool Bit_2100
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2101)]
    public static bool Bit_2101
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2102)]
    public static bool Bit_2102
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2103)]
    public static bool Bit_2103
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2128)]
    public static bool Bit_2128
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2418)]
    public static bool Bit_2418
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2419)]
    public static bool Bit_2419
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2420)]
    public static bool Bit_2420
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2421)]
    public static bool Bit_2421
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2422)]
    public static bool Bit_2422
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2423)]
    public static bool Bit_2423
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2432)]
    public static bool Bit_2432
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2435)]
    public static bool Bit_2435
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2436)]
    public static bool Bit_2436
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2437)]
    public static bool Bit_2437
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2438)]
    public static bool Bit_2438
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2439)]
    public static bool Bit_2439
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2440)]
    public static bool Bit_2440
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2441)]
    public static bool Bit_2441
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2442)]
    public static bool Bit_2442
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2444)]
    public static bool Bit_2444
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2445)]
    public static bool Bit_2445
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2446)]
    public static bool Bit_2446
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2447)]
    public static bool Bit_2447
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2448)]
    public static bool Bit_2448
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2455)]
    public static bool Bit_2455
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2456)]
    public static bool Bit_2456
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2458)]
    public static bool Bit_2458
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2466)]
    public static bool Bit_2466
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2471)]
    public static bool Bit_2471
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2474)]
    public static bool Bit_2474
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2476)]
    public static bool Bit_2476
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2477)]
    public static bool Bit_2477
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2478)]
    public static bool Bit_2478
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2479)]
    public static bool Bit_2479
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2480)]
    public static bool Bit_2480
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2481)]
    public static bool Bit_2481
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2486)]
    public static bool Bit_2486
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2487)]
    public static bool Bit_2487
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2488)]
    public static bool Bit_2488
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2491)]
    public static bool Bit_2491
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2492)]
    public static bool Bit_2492
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2493)]
    public static bool Bit_2493
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2494)]
    public static bool Bit_2494
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2495)]
    public static bool Bit_2495
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2592)]
    public static bool Bit_2592
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2593)]
    public static bool Bit_2593
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2594)]
    public static bool Bit_2594
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2595)]
    public static bool Bit_2595
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2596)]
    public static bool Bit_2596
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2597)]
    public static bool Bit_2597
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2598)]
    public static bool Bit_2598
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2599)]
    public static bool Bit_2599
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2600)]
    public static bool Bit_2600
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2601)]
    public static bool Bit_2601
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2602)]
    public static bool Bit_2602
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2603)]
    public static bool Bit_2603
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2604)]
    public static bool Bit_2604
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2605)]
    public static bool Bit_2605
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2606)]
    public static bool Bit_2606
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2607)]
    public static bool Bit_2607
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2609)]
    public static bool Bit_2609
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2611)]
    public static bool Bit_2611
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2612)]
    public static bool Bit_2612
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2615)]
    public static bool Bit_2615
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2616)]
    public static bool Bit_2616
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2617)]
    public static bool Bit_2617
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2618)]
    public static bool Bit_2618
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2619)]
    public static bool Bit_2619
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2620)]
    public static bool Bit_2620
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2621)]
    public static bool Bit_2621
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2622)]
    public static bool Bit_2622
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2623)]
    public static bool Bit_2623
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2625)]
    public static bool Bit_2625
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2626)]
    public static bool Bit_2626
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2627)]
    public static bool Bit_2627
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2628)]
    public static bool Bit_2628
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2630)]
    public static bool Bit_2630
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2631)]
    public static bool Bit_2631
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2641)]
    public static bool Bit_2641
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2642)]
    public static bool Bit_2642
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2643)]
    public static bool Bit_2643
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2645)]
    public static bool Bit_2645
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2646)]
    public static bool Bit_2646
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2647)]
    public static bool Bit_2647
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2648)]
    public static bool Bit_2648
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2649)]
    public static bool Bit_2649
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2650)]
    public static bool Bit_2650
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2651)]
    public static bool Bit_2651
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2652)]
    public static bool Bit_2652
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2653)]
    public static bool Bit_2653
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2654)]
    public static bool Bit_2654
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2655)]
    public static bool Bit_2655
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2660)]
    public static bool Bit_2660
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2661)]
    public static bool Bit_2661
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2662)]
    public static bool Bit_2662
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2663)]
    public static bool Bit_2663
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2816)]
    public static bool Bit_2816
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2817)]
    public static bool Bit_2817
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2818)]
    public static bool Bit_2818
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2819)]
    public static bool Bit_2819
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2820)]
    public static bool Bit_2820
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2821)]
    public static bool Bit_2821
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2822)]
    public static bool Bit_2822
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2823)]
    public static bool Bit_2823
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2824)]
    public static bool Bit_2824
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2825)]
    public static bool Bit_2825
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2826)]
    public static bool Bit_2826
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2827)]
    public static bool Bit_2827
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2828)]
    public static bool Bit_2828
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2829)]
    public static bool Bit_2829
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2830)]
    public static bool Bit_2830
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2831)]
    public static bool Bit_2831
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2836)]
    public static bool Bit_2836
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2837)]
    public static bool Bit_2837
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2838)]
    public static bool Bit_2838
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2839)]
    public static bool Bit_2839
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2856)]
    public static bool Bit_2856
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2857)]
    public static bool Bit_2857
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2858)]
    public static bool Bit_2858
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2859)]
    public static bool Bit_2859
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2860)]
    public static bool Bit_2860
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2861)]
    public static bool Bit_2861
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2862)]
    public static bool Bit_2862
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2863)]
    public static bool Bit_2863
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2864)]
    public static bool Bit_2864
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2865)]
    public static bool Bit_2865
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2866)]
    public static bool Bit_2866
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2867)]
    public static bool Bit_2867
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2872)]
    public static bool Bit_2872
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2873)]
    public static bool Bit_2873
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2874)]
    public static bool Bit_2874
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2875)]
    public static bool Bit_2875
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2876)]
    public static bool Bit_2876
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2877)]
    public static bool Bit_2877
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2878)]
    public static bool Bit_2878
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2879)]
    public static bool Bit_2879
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2884)]
    public static bool Bit_2884
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2885)]
    public static bool Bit_2885
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2886)]
    public static bool Bit_2886
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2887)]
    public static bool Bit_2887
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2906)]
    public static bool Bit_2906
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2907)]
    public static bool Bit_2907
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2908)]
    public static bool Bit_2908
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2909)]
    public static bool Bit_2909
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2910)]
    public static bool Bit_2910
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2911)]
    public static bool Bit_2911
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2918)]
    public static bool Bit_2918
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2919)]
    public static bool Bit_2919
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2921)]
    public static bool Bit_2921
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2922)]
    public static bool Bit_2922
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2923)]
    public static bool Bit_2923
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2924)]
    public static bool Bit_2924
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2925)]
    public static bool Bit_2925
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2926)]
    public static bool Bit_2926
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2927)]
    public static bool Bit_2927
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2930)]
    public static bool Bit_2930
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2931)]
    public static bool Bit_2931
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2932)]
    public static bool Bit_2932
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2933)]
    public static bool Bit_2933
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2934)]
    public static bool Bit_2934
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2935)]
    public static bool Bit_2935
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2938)]
    public static bool Bit_2938
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2939)]
    public static bool Bit_2939
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2940)]
    public static bool Bit_2940
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2941)]
    public static bool Bit_2941
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2942)]
    public static bool Bit_2942
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2943)]
    public static bool Bit_2943
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2948)]
    public static bool Bit_2948
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2949)]
    public static bool Bit_2949
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2950)]
    public static bool Bit_2950
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2951)]
    public static bool Bit_2951
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2969)]
    public static bool Bit_2969
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2970)]
    public static bool Bit_2970
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2971)]
    public static bool Bit_2971
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2972)]
    public static bool Bit_2972
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2973)]
    public static bool Bit_2973
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2974)]
    public static bool Bit_2974
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2975)]
    public static bool Bit_2975
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2976)]
    public static bool Bit_2976
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2977)]
    public static bool Bit_2977
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2978)]
    public static bool Bit_2978
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2979)]
    public static bool Bit_2979
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2980)]
    public static bool Bit_2980
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2981)]
    public static bool Bit_2981
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2982)]
    public static bool Bit_2982
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2983)]
    public static bool Bit_2983
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3228)]
    public static bool Bit_3228
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3229)]
    public static bool Bit_3229
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3230)]
    public static bool Bit_3230
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3231)]
    public static bool Bit_3231
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3232)]
    public static bool Bit_3232
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3233)]
    public static bool Bit_3233
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3234)]
    public static bool Bit_3234
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3235)]
    public static bool Bit_3235
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3236)]
    public static bool Bit_3236
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3237)]
    public static bool Bit_3237
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3238)]
    public static bool Bit_3238
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3239)]
    public static bool Bit_3239
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3240)]
    public static bool Bit_3240
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3242)]
    public static bool Bit_3242
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3243)]
    public static bool Bit_3243
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3244)]
    public static bool Bit_3244
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3245)]
    public static bool Bit_3245
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3246)]
    public static bool Bit_3246
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3247)]
    public static bool Bit_3247
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3248)]
    public static bool Bit_3248
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3249)]
    public static bool Bit_3249
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3250)]
    public static bool Bit_3250
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3251)]
    public static bool Bit_3251
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3252)]
    public static bool Bit_3252
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3253)]
    public static bool Bit_3253
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3254)]
    public static bool Bit_3254
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3255)]
    public static bool Bit_3255
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3256)]
    public static bool Bit_3256
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3257)]
    public static bool Bit_3257
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3258)]
    public static bool Bit_3258
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3259)]
    public static bool Bit_3259
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3260)]
    public static bool Bit_3260
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3261)]
    public static bool Bit_3261
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3262)]
    public static bool Bit_3262
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3263)]
    public static bool Bit_3263
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3456)]
    public static bool Bit_3456
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3457)]
    public static bool Bit_3457
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3458)]
    public static bool Bit_3458
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3459)]
    public static bool Bit_3459
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3460)]
    public static bool Bit_3460
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3461)]
    public static bool Bit_3461
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3462)]
    public static bool Bit_3462
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3463)]
    public static bool Bit_3463
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3464)]
    public static bool Bit_3464
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3465)]
    public static bool Bit_3465
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3466)]
    public static bool Bit_3466
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3468)]
    public static bool Bit_3468
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3469)]
    public static bool Bit_3469
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3470)]
    public static bool Bit_3470
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3471)]
    public static bool Bit_3471
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3536)]
    public static bool Bit_3536
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3537)]
    public static bool Bit_3537
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3538)]
    public static bool Bit_3538
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3539)]
    public static bool Bit_3539
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3540)]
    public static bool Bit_3540
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3541)]
    public static bool Bit_3541
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3542)]
    public static bool Bit_3542
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3543)]
    public static bool Bit_3543
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3544)]
    public static bool Bit_3544
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3545)]
    public static bool Bit_3545
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3546)]
    public static bool Bit_3546
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3547)]
    public static bool Bit_3547
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3550)]
    public static bool Bit_3550
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3551)]
    public static bool Bit_3551
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3552)]
    public static bool Bit_3552
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3553)]
    public static bool Bit_3553
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3554)]
    public static bool Bit_3554
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3555)]
    public static bool Bit_3555
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3556)]
    public static bool Bit_3556
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3557)]
    public static bool Bit_3557
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3558)]
    public static bool Bit_3558
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3559)]
    public static bool Bit_3559
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3560)]
    public static bool Bit_3560
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3561)]
    public static bool Bit_3561
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3562)]
    public static bool Bit_3562
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3563)]
    public static bool Bit_3563
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3564)]
    public static bool Bit_3564
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3565)]
    public static bool Bit_3565
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3566)]
    public static bool Bit_3566
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3567)]
    public static bool Bit_3567
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3568)]
    public static bool Bit_3568
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3569)]
    public static bool Bit_3569
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3570)]
    public static bool Bit_3570
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3571)]
    public static bool Bit_3571
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3572)]
    public static bool Bit_3572
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3573)]
    public static bool Bit_3573
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3574)]
    public static bool Bit_3574
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3575)]
    public static bool Bit_3575
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3576)]
    public static bool Bit_3576
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3577)]
    public static bool Bit_3577
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3578)]
    public static bool Bit_3578
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3579)]
    public static bool Bit_3579
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3580)]
    public static bool Bit_3580
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3581)]
    public static bool Bit_3581
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3582)]
    public static bool Bit_3582
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3583)]
    public static bool Bit_3583
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3584)]
    public static bool Bit_3584
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3585)]
    public static bool Bit_3585
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3586)]
    public static bool Bit_3586
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3587)]
    public static bool Bit_3587
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3588)]
    public static bool Bit_3588
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3589)]
    public static bool Bit_3589
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3590)]
    public static bool Bit_3590
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3591)]
    public static bool Bit_3591
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3593)]
    public static bool Bit_3593
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3594)]
    public static bool Bit_3594
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3595)]
    public static bool Bit_3595
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3596)]
    public static bool Bit_3596
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3597)]
    public static bool Bit_3597
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3599)]
    public static bool Bit_3599
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3600)]
    public static bool Bit_3600
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3602)]
    public static bool Bit_3602
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3603)]
    public static bool Bit_3603
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3604)]
    public static bool Bit_3604
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3605)]
    public static bool Bit_3605
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3606)]
    public static bool Bit_3606
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3607)]
    public static bool Bit_3607
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3608)]
    public static bool Bit_3608
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3609)]
    public static bool Bit_3609
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3610)]
    public static bool Bit_3610
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3611)]
    public static bool Bit_3611
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3612)]
    public static bool Bit_3612
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3613)]
    public static bool Bit_3613
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3614)]
    public static bool Bit_3614
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3615)]
    public static bool Bit_3615
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3616)]
    public static bool Bit_3616
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3617)]
    public static bool Bit_3617
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3618)]
    public static bool Bit_3618
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3619)]
    public static bool Bit_3619
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3620)]
    public static bool Bit_3620
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3621)]
    public static bool Bit_3621
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3622)]
    public static bool Bit_3622
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3623)]
    public static bool Bit_3623
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3630)]
    public static bool Bit_3630
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3631)]
    public static bool Bit_3631
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3632)]
    public static bool Bit_3632
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3664)]
    public static bool Bit_3664
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3665)]
    public static bool Bit_3665
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3666)]
    public static bool Bit_3666
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3667)]
    public static bool Bit_3667
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3668)]
    public static bool Bit_3668
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3669)]
    public static bool Bit_3669
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3670)]
    public static bool Bit_3670
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3671)]
    public static bool Bit_3671
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3712)]
    public static bool Bit_3712
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3713)]
    public static bool Bit_3713
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3714)]
    public static bool Bit_3714
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3715)]
    public static bool Bit_3715
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3716)]
    public static bool Bit_3716
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3717)]
    public static bool Bit_3717
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3718)]
    public static bool Bit_3718
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3719)]
    public static bool Bit_3719
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3784)]
    public static bool Bit_3784
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3787)]
    public static bool Bit_3787
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3788)]
    public static bool Bit_3788
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3789)]
    public static bool Bit_3789
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3790)]
    public static bool Bit_3790
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3791)]
    public static bool Bit_3791
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3792)]
    public static bool Bit_3792
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3793)]
    public static bool Bit_3793
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3794)]
    public static bool Bit_3794
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3795)]
    public static bool Bit_3795
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3796)]
    public static bool Bit_3796
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3797)]
    public static bool Bit_3797
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3798)]
    public static bool Bit_3798
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3799)]
    public static bool Bit_3799
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3811)]
    public static bool Bit_3811
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3815)]
    public static bool Bit_3815
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3816)]
    public static bool Bit_3816
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3817)]
    public static bool Bit_3817
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3818)]
    public static bool Bit_3818
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3819)]
    public static bool Bit_3819
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3820)]
    public static bool Bit_3820
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3821)]
    public static bool Bit_3821
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3822)]
    public static bool Bit_3822
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3823)]
    public static bool Bit_3823
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3824)]
    public static bool Bit_3824
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3825)]
    public static bool Bit_3825
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3826)]
    public static bool Bit_3826
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3827)]
    public static bool Bit_3827
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3828)]
    public static bool Bit_3828
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3829)]
    public static bool Bit_3829
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3830)]
    public static bool Bit_3830
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3831)]
    public static bool Bit_3831
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3840)]
    public static bool Bit_3840
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3843)]
    public static bool Bit_3843
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3846)]
    public static bool Bit_3846
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3847)]
    public static bool Bit_3847
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3848)]
    public static bool Bit_3848
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3849)]
    public static bool Bit_3849
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3850)]
    public static bool Bit_3850
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3851)]
    public static bool Bit_3851
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3852)]
    public static bool Bit_3852
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3853)]
    public static bool Bit_3853
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3854)]
    public static bool Bit_3854
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3855)]
    public static bool Bit_3855
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3856)]
    public static bool Bit_3856
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3857)]
    public static bool Bit_3857
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3860)]
    public static bool Bit_3860
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3861)]
    public static bool Bit_3861
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3862)]
    public static bool Bit_3862
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3863)]
    public static bool Bit_3863
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3881)]
    public static bool Bit_3881
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3882)]
    public static bool Bit_3882
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3883)]
    public static bool Bit_3883
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3884)]
    public static bool Bit_3884
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3885)]
    public static bool Bit_3885
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3886)]
    public static bool Bit_3886
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3887)]
    public static bool Bit_3887
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3888)]
    public static bool Bit_3888
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3889)]
    public static bool Bit_3889
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3890)]
    public static bool Bit_3890
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3891)]
    public static bool Bit_3891
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3895)]
    public static bool Bit_3895
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3896)]
    public static bool Bit_3896
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3897)]
    public static bool Bit_3897
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3898)]
    public static bool Bit_3898
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3899)]
    public static bool Bit_3899
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3900)]
    public static bool Bit_3900
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3901)]
    public static bool Bit_3901
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3902)]
    public static bool Bit_3902
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3903)]
    public static bool Bit_3903
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3904)]
    public static bool Bit_3904
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3905)]
    public static bool Bit_3905
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3944)]
    public static bool Bit_3944
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3945)]
    public static bool Bit_3945
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3946)]
    public static bool Bit_3946
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3947)]
    public static bool Bit_3947
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3948)]
    public static bool Bit_3948
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3949)]
    public static bool Bit_3949
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3950)]
    public static bool Bit_3950
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3951)]
    public static bool Bit_3951
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3952)]
    public static bool Bit_3952
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3966)]
    public static bool Bit_3966
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3967)]
    public static bool Bit_3967
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 4046)]
    public static bool Bit_4046
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 4047)]
    public static bool Bit_4047
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7171)]
    public static bool Bit_7171
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7172)]
    public static bool Bit_7172
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7174)]
    public static bool Bit_7174
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7175)]
    public static bool Bit_7175
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7184)]
    public static bool Bit_7184
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7185)]
    public static bool Bit_7185
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7186)]
    public static bool Bit_7186
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7187)]
    public static bool Bit_7187
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7188)]
    public static bool Bit_7188
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7189)]
    public static bool Bit_7189
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7190)]
    public static bool Bit_7190
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7191)]
    public static bool Bit_7191
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7192)]
    public static bool Bit_7192
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7193)]
    public static bool Bit_7193
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7194)]
    public static bool Bit_7194
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7195)]
    public static bool Bit_7195
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7196)]
    public static bool Bit_7196
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7197)]
    public static bool Bit_7197
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7198)]
    public static bool Bit_7198
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7199)]
    public static bool Bit_7199
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7200)]
    public static bool Bit_7200
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7201)]
    public static bool Bit_7201
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7202)]
    public static bool Bit_7202
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7203)]
    public static bool Bit_7203
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7204)]
    public static bool Bit_7204
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7205)]
    public static bool Bit_7205
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7206)]
    public static bool Bit_7206
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7207)]
    public static bool Bit_7207
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7211)]
    public static bool Bit_7211
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7212)]
    public static bool Bit_7212
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7213)]
    public static bool Bit_7213
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7214)]
    public static bool Bit_7214
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7215)]
    public static bool Bit_7215
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7223)]
    public static bool Bit_7223
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7230)]
    public static bool Bit_7230
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7231)]
    public static bool Bit_7231
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7240)]
    public static bool Bit_7240
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7241)]
    public static bool Bit_7241
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7242)]
    public static bool Bit_7242
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7243)]
    public static bool Bit_7243
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7244)]
    public static bool Bit_7244
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7245)]
    public static bool Bit_7245
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7246)]
    public static bool Bit_7246
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7247)]
    public static bool Bit_7247
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7264)]
    public static bool Bit_7264
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7265)]
    public static bool Bit_7265
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7266)]
    public static bool Bit_7266
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7267)]
    public static bool Bit_7267
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7268)]
    public static bool Bit_7268
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7269)]
    public static bool Bit_7269
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7270)]
    public static bool Bit_7270
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7271)]
    public static bool Bit_7271
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7272)]
    public static bool Bit_7272
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7273)]
    public static bool Bit_7273
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7274)]
    public static bool Bit_7274
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7275)]
    public static bool Bit_7275
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7276)]
    public static bool Bit_7276
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7277)]
    public static bool Bit_7277
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7278)]
    public static bool Bit_7278
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7279)]
    public static bool Bit_7279
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7280)]
    public static bool Bit_7280
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7281)]
    public static bool Bit_7281
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7282)]
    public static bool Bit_7282
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7283)]
    public static bool Bit_7283
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7284)]
    public static bool Bit_7284
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7285)]
    public static bool Bit_7285
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7286)]
    public static bool Bit_7286
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7287)]
    public static bool Bit_7287
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7292)]
    public static bool Bit_7292
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7294)]
    public static bool Bit_7294
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7295)]
    public static bool Bit_7295
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7296)]
    public static bool Bit_7296
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7297)]
    public static bool Bit_7297
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7298)]
    public static bool Bit_7298
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7299)]
    public static bool Bit_7299
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7300)]
    public static bool Bit_7300
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7301)]
    public static bool Bit_7301
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7302)]
    public static bool Bit_7302
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7303)]
    public static bool Bit_7303
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7304)]
    public static bool Bit_7304
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7309)]
    public static bool Bit_7309
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7310)]
    public static bool Bit_7310
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7311)]
    public static bool Bit_7311
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7312)]
    public static bool Bit_7312
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7313)]
    public static bool Bit_7313
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7314)]
    public static bool Bit_7314
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7315)]
    public static bool Bit_7315
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7316)]
    public static bool Bit_7316
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7317)]
    public static bool Bit_7317
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7318)]
    public static bool Bit_7318
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7319)]
    public static bool Bit_7319
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7320)]
    public static bool Bit_7320
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7321)]
    public static bool Bit_7321
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7322)]
    public static bool Bit_7322
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7323)]
    public static bool Bit_7323
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7324)]
    public static bool Bit_7324
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7325)]
    public static bool Bit_7325
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7326)]
    public static bool Bit_7326
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7327)]
    public static bool Bit_7327
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7328)]
    public static bool Bit_7328
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7329)]
    public static bool Bit_7329
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7330)]
    public static bool Bit_7330
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7331)]
    public static bool Bit_7331
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7332)]
    public static bool Bit_7332
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7333)]
    public static bool Bit_7333
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7334)]
    public static bool Bit_7334
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7335)]
    public static bool Bit_7335
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7336)]
    public static bool Bit_7336
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7337)]
    public static bool Bit_7337
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7338)]
    public static bool Bit_7338
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7339)]
    public static bool Bit_7339
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7340)]
    public static bool Bit_7340
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7341)]
    public static bool Bit_7341
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7342)]
    public static bool Bit_7342
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7343)]
    public static bool Bit_7343
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7344)]
    public static bool Bit_7344
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7345)]
    public static bool Bit_7345
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7346)]
    public static bool Bit_7346
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7347)]
    public static bool Bit_7347
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7348)]
    public static bool Bit_7348
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7349)]
    public static bool Bit_7349
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7350)]
    public static bool Bit_7350
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7351)]
    public static bool Bit_7351
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7352)]
    public static bool Bit_7352
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7353)]
    public static bool Bit_7353
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7354)]
    public static bool Bit_7354
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7355)]
    public static bool Bit_7355
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7356)]
    public static bool Bit_7356
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7357)]
    public static bool Bit_7357
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7358)]
    public static bool Bit_7358
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7359)]
    public static bool Bit_7359
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7360)]
    public static bool Bit_7360
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7361)]
    public static bool Bit_7361
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7362)]
    public static bool Bit_7362
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7363)]
    public static bool Bit_7363
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7364)]
    public static bool Bit_7364
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7365)]
    public static bool Bit_7365
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7366)]
    public static bool Bit_7366
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7367)]
    public static bool Bit_7367
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7371)]
    public static bool Bit_7371
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7372)]
    public static bool Bit_7372
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7373)]
    public static bool Bit_7373
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7374)]
    public static bool Bit_7374
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7375)]
    public static bool Bit_7375
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7380)]
    public static bool Bit_7380
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7381)]
    public static bool Bit_7381
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7382)]
    public static bool Bit_7382
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7383)]
    public static bool Bit_7383
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7384)]
    public static bool Bit_7384
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7385)]
    public static bool Bit_7385
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7386)]
    public static bool Bit_7386
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7387)]
    public static bool Bit_7387
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7388)]
    public static bool Bit_7388
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7389)]
    public static bool Bit_7389
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7390)]
    public static bool Bit_7390
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7391)]
    public static bool Bit_7391
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7395)]
    public static bool Bit_7395
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7396)]
    public static bool Bit_7396
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7397)]
    public static bool Bit_7397
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7398)]
    public static bool Bit_7398
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7399)]
    public static bool Bit_7399
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7402)]
    public static bool Bit_7402
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7403)]
    public static bool Bit_7403
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7404)]
    public static bool Bit_7404
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7405)]
    public static bool Bit_7405
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7406)]
    public static bool Bit_7406
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7407)]
    public static bool Bit_7407
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7409)]
    public static bool Bit_7409
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7410)]
    public static bool Bit_7410
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7411)]
    public static bool Bit_7411
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7412)]
    public static bool Bit_7412
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7413)]
    public static bool Bit_7413
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7414)]
    public static bool Bit_7414
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7415)]
    public static bool Bit_7415
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7418)]
    public static bool Bit_7418
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7419)]
    public static bool Bit_7419
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7420)]
    public static bool Bit_7420
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7421)]
    public static bool Bit_7421
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7422)]
    public static bool Bit_7422
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7423)]
    public static bool Bit_7423
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7424)]
    public static bool Bit_7424
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7425)]
    public static bool Bit_7425
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7426)]
    public static bool Bit_7426
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7427)]
    public static bool Bit_7427
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7428)]
    public static bool Bit_7428
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7429)]
    public static bool Bit_7429
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7430)]
    public static bool Bit_7430
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7431)]
    public static bool Bit_7431
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7432)]
    public static bool Bit_7432
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7439)]
    public static bool Bit_7439
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7440)]
    public static bool Bit_7440
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7441)]
    public static bool Bit_7441
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7442)]
    public static bool Bit_7442
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7443)]
    public static bool Bit_7443
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7444)]
    public static bool Bit_7444
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7445)]
    public static bool Bit_7445
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7446)]
    public static bool Bit_7446
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7447)]
    public static bool Bit_7447
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7448)]
    public static bool Bit_7448
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7449)]
    public static bool Bit_7449
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7450)]
    public static bool Bit_7450
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7451)]
    public static bool Bit_7451
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7452)]
    public static bool Bit_7452
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7453)]
    public static bool Bit_7453
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7454)]
    public static bool Bit_7454
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7455)]
    public static bool Bit_7455
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7456)]
    public static bool Bit_7456
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7457)]
    public static bool Bit_7457
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7458)]
    public static bool Bit_7458
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7459)]
    public static bool Bit_7459
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7460)]
    public static bool Bit_7460
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7461)]
    public static bool Bit_7461
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7462)]
    public static bool Bit_7462
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7463)]
    public static bool Bit_7463
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7464)]
    public static bool Bit_7464
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7467)]
    public static bool Bit_7467
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7468)]
    public static bool Bit_7468
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7469)]
    public static bool Bit_7469
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7470)]
    public static bool Bit_7470
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7471)]
    public static bool Bit_7471
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7472)]
    public static bool Bit_7472
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7473)]
    public static bool Bit_7473
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7474)]
    public static bool Bit_7474
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7475)]
    public static bool Bit_7475
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7476)]
    public static bool Bit_7476
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7477)]
    public static bool Bit_7477
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7478)]
    public static bool Bit_7478
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7479)]
    public static bool Bit_7479
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7488)]
    public static bool Bit_7488
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7489)]
    public static bool Bit_7489
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7490)]
    public static bool Bit_7490
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7491)]
    public static bool Bit_7491
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7492)]
    public static bool Bit_7492
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7493)]
    public static bool Bit_7493
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7494)]
    public static bool Bit_7494
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7495)]
    public static bool Bit_7495
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7496)]
    public static bool Bit_7496
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7497)]
    public static bool Bit_7497
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7498)]
    public static bool Bit_7498
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7499)]
    public static bool Bit_7499
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7500)]
    public static bool Bit_7500
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7501)]
    public static bool Bit_7501
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7502)]
    public static bool Bit_7502
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7503)]
    public static bool Bit_7503
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7507)]
    public static bool Bit_7507
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7508)]
    public static bool Bit_7508
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7509)]
    public static bool Bit_7509
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7510)]
    public static bool Bit_7510
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7511)]
    public static bool Bit_7511
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7512)]
    public static bool Bit_7512
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7513)]
    public static bool Bit_7513
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7514)]
    public static bool Bit_7514
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7515)]
    public static bool Bit_7515
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7516)]
    public static bool Bit_7516
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7517)]
    public static bool Bit_7517
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7518)]
    public static bool Bit_7518
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7519)]
    public static bool Bit_7519
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7520)]
    public static bool Bit_7520
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7521)]
    public static bool Bit_7521
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7522)]
    public static bool Bit_7522
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7523)]
    public static bool Bit_7523
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7524)]
    public static bool Bit_7524
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7525)]
    public static bool Bit_7525
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7526)]
    public static bool Bit_7526
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7527)]
    public static bool Bit_7527
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7534)]
    public static bool Bit_7534
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7535)]
    public static bool Bit_7535
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7539)]
    public static bool Bit_7539
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7540)]
    public static bool Bit_7540
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7541)]
    public static bool Bit_7541
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7542)]
    public static bool Bit_7542
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7543)]
    public static bool Bit_7543
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7544)]
    public static bool Bit_7544
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7545)]
    public static bool Bit_7545
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7546)]
    public static bool Bit_7546
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7547)]
    public static bool Bit_7547
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7548)]
    public static bool Bit_7548
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7549)]
    public static bool Bit_7549
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7550)]
    public static bool Bit_7550
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7551)]
    public static bool Bit_7551
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7552)]
    public static bool Bit_7552
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7553)]
    public static bool Bit_7553
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7554)]
    public static bool Bit_7554
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7555)]
    public static bool Bit_7555
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7556)]
    public static bool Bit_7556
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7557)]
    public static bool Bit_7557
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7558)]
    public static bool Bit_7558
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7559)]
    public static bool Bit_7559
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7563)]
    public static bool Bit_7563
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7564)]
    public static bool Bit_7564
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7565)]
    public static bool Bit_7565
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7566)]
    public static bool Bit_7566
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7567)]
    public static bool Bit_7567
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7568)]
    public static bool Bit_7568
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7570)]
    public static bool Bit_7570
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7571)]
    public static bool Bit_7571
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7572)]
    public static bool Bit_7572
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7573)]
    public static bool Bit_7573
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7574)]
    public static bool Bit_7574
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7575)]
    public static bool Bit_7575
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7576)]
    public static bool Bit_7576
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7577)]
    public static bool Bit_7577
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7582)]
    public static bool Bit_7582
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7583)]
    public static bool Bit_7583
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7584)]
    public static bool Bit_7584
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7585)]
    public static bool Bit_7585
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7586)]
    public static bool Bit_7586
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7587)]
    public static bool Bit_7587
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7588)]
    public static bool Bit_7588
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7589)]
    public static bool Bit_7589
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7590)]
    public static bool Bit_7590
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7591)]
    public static bool Bit_7591
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7592)]
    public static bool Bit_7592
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7593)]
    public static bool Bit_7593
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7594)]
    public static bool Bit_7594
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7595)]
    public static bool Bit_7595
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7596)]
    public static bool Bit_7596
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7597)]
    public static bool Bit_7597
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7598)]
    public static bool Bit_7598
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7599)]
    public static bool Bit_7599
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7603)]
    public static bool Bit_7603
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7604)]
    public static bool Bit_7604
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7605)]
    public static bool Bit_7605
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7606)]
    public static bool Bit_7606
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7607)]
    public static bool Bit_7607
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7616)]
    public static bool Bit_7616
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7617)]
    public static bool Bit_7617
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7618)]
    public static bool Bit_7618
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7619)]
    public static bool Bit_7619
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7620)]
    public static bool Bit_7620
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7621)]
    public static bool Bit_7621
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7622)]
    public static bool Bit_7622
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7623)]
    public static bool Bit_7623
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7627)]
    public static bool Bit_7627
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7628)]
    public static bool Bit_7628
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7629)]
    public static bool Bit_7629
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7630)]
    public static bool Bit_7630
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7631)]
    public static bool Bit_7631
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7648)]
    public static bool Bit_7648
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7649)]
    public static bool Bit_7649
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7650)]
    public static bool Bit_7650
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7651)]
    public static bool Bit_7651
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7652)]
    public static bool Bit_7652
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7653)]
    public static bool Bit_7653
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7654)]
    public static bool Bit_7654
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7655)]
    public static bool Bit_7655
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7657)]
    public static bool Bit_7657
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7658)]
    public static bool Bit_7658
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7659)]
    public static bool Bit_7659
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7660)]
    public static bool Bit_7660
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7661)]
    public static bool Bit_7661
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7662)]
    public static bool Bit_7662
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7663)]
    public static bool Bit_7663
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7681)]
    public static bool Bit_7681
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7682)]
    public static bool Bit_7682
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7683)]
    public static bool Bit_7683
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7684)]
    public static bool Bit_7684
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7685)]
    public static bool Bit_7685
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7686)]
    public static bool Bit_7686
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7687)]
    public static bool Bit_7687
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7688)]
    public static bool Bit_7688
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7689)]
    public static bool Bit_7689
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7690)]
    public static bool Bit_7690
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7691)]
    public static bool Bit_7691
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7692)]
    public static bool Bit_7692
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7693)]
    public static bool Bit_7693
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7694)]
    public static bool Bit_7694
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7695)]
    public static bool Bit_7695
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7696)]
    public static bool Bit_7696
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7697)]
    public static bool Bit_7697
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7698)]
    public static bool Bit_7698
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7699)]
    public static bool Bit_7699
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7700)]
    public static bool Bit_7700
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7701)]
    public static bool Bit_7701
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7702)]
    public static bool Bit_7702
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7703)]
    public static bool Bit_7703
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7704)]
    public static bool Bit_7704
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7705)]
    public static bool Bit_7705
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7706)]
    public static bool Bit_7706
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7707)]
    public static bool Bit_7707
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7708)]
    public static bool Bit_7708
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7709)]
    public static bool Bit_7709
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7710)]
    public static bool Bit_7710
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7711)]
    public static bool Bit_7711
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7712)]
    public static bool Bit_7712
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7713)]
    public static bool Bit_7713
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7714)]
    public static bool Bit_7714
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7715)]
    public static bool Bit_7715
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7716)]
    public static bool Bit_7716
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7717)]
    public static bool Bit_7717
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7718)]
    public static bool Bit_7718
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7719)]
    public static bool Bit_7719
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7727)]
    public static bool Bit_7727
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7730)]
    public static bool Bit_7730
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7731)]
    public static bool Bit_7731
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7732)]
    public static bool Bit_7732
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7733)]
    public static bool Bit_7733
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7734)]
    public static bool Bit_7734
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7735)]
    public static bool Bit_7735
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7742)]
    public static bool Bit_7742
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7743)]
    public static bool Bit_7743
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7744)]
    public static bool Bit_7744
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7745)]
    public static bool Bit_7745
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7746)]
    public static bool Bit_7746
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7747)]
    public static bool Bit_7747
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7748)]
    public static bool Bit_7748
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7749)]
    public static bool Bit_7749
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7750)]
    public static bool Bit_7750
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7751)]
    public static bool Bit_7751
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7759)]
    public static bool Bit_7759
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7767)]
    public static bool Bit_7767
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7775)]
    public static bool Bit_7775
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8376)]
    public static bool Bit_8376
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8377)]
    public static bool Bit_8377
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8378)]
    public static bool Bit_8378
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8379)]
    public static bool Bit_8379
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8380)]
    public static bool Bit_8380
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8381)]
    public static bool Bit_8381
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8382)]
    public static bool Bit_8382
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8383)]
    public static bool Bit_8383
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8384)]
    public static bool Bit_8384
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8385)]
    public static bool Bit_8385
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8386)]
    public static bool Bit_8386
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8387)]
    public static bool Bit_8387
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8388)]
    public static bool Bit_8388
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8389)]
    public static bool Bit_8389
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8390)]
    public static bool Bit_8390
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8391)]
    public static bool Bit_8391
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8392)]
    public static bool Bit_8392
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8393)]
    public static bool Bit_8393
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8394)]
    public static bool Bit_8394
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8395)]
    public static bool Bit_8395
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8396)]
    public static bool Bit_8396
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8397)]
    public static bool Bit_8397
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8398)]
    public static bool Bit_8398
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8399)]
    public static bool Bit_8399
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8400)]
    public static bool Bit_8400
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8401)]
    public static bool Bit_8401
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8402)]
    public static bool Bit_8402
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8403)]
    public static bool Bit_8403
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8404)]
    public static bool Bit_8404
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8405)]
    public static bool Bit_8405
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8406)]
    public static bool Bit_8406
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8407)]
    public static bool Bit_8407
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8408)]
    public static bool Bit_8408
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8409)]
    public static bool Bit_8409
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8410)]
    public static bool Bit_8410
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8411)]
    public static bool Bit_8411
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8412)]
    public static bool Bit_8412
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8413)]
    public static bool Bit_8413
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8414)]
    public static bool Bit_8414
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8415)]
    public static bool Bit_8415
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8416)]
    public static bool Bit_8416
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8417)]
    public static bool Bit_8417
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8418)]
    public static bool Bit_8418
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8419)]
    public static bool Bit_8419
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8420)]
    public static bool Bit_8420
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8421)]
    public static bool Bit_8421
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8422)]
    public static bool Bit_8422
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8423)]
    public static bool Bit_8423
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8424)]
    public static bool Bit_8424
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8425)]
    public static bool Bit_8425
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8426)]
    public static bool Bit_8426
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8427)]
    public static bool Bit_8427
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8428)]
    public static bool Bit_8428
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8429)]
    public static bool Bit_8429
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8430)]
    public static bool Bit_8430
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8431)]
    public static bool Bit_8431
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8432)]
    public static bool Bit_8432
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8433)]
    public static bool Bit_8433
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8434)]
    public static bool Bit_8434
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8435)]
    public static bool Bit_8435
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8436)]
    public static bool Bit_8436
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8437)]
    public static bool Bit_8437
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8438)]
    public static bool Bit_8438
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8439)]
    public static bool Bit_8439
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8440)]
    public static bool Bit_8440
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8441)]
    public static bool Bit_8441
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8442)]
    public static bool Bit_8442
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8443)]
    public static bool Bit_8443
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8444)]
    public static bool Bit_8444
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8445)]
    public static bool Bit_8445
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8446)]
    public static bool Bit_8446
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8447)]
    public static bool Bit_8447
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8448)]
    public static bool Bit_8448
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8449)]
    public static bool Bit_8449
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8450)]
    public static bool Bit_8450
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8451)]
    public static bool Bit_8451
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8452)]
    public static bool Bit_8452
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8453)]
    public static bool Bit_8453
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8454)]
    public static bool Bit_8454
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8455)]
    public static bool Bit_8455
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8456)]
    public static bool Bit_8456
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8457)]
    public static bool Bit_8457
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8458)]
    public static bool Bit_8458
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8459)]
    public static bool Bit_8459
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8460)]
    public static bool Bit_8460
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8461)]
    public static bool Bit_8461
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8462)]
    public static bool Bit_8462
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8463)]
    public static bool Bit_8463
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8464)]
    public static bool Bit_8464
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8465)]
    public static bool Bit_8465
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8466)]
    public static bool Bit_8466
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8467)]
    public static bool Bit_8467
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8468)]
    public static bool Bit_8468
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8469)]
    public static bool Bit_8469
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8470)]
    public static bool Bit_8470
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8471)]
    public static bool Bit_8471
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8472)]
    public static bool Bit_8472
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8473)]
    public static bool Bit_8473
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8474)]
    public static bool Bit_8474
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8475)]
    public static bool Bit_8475
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8476)]
    public static bool Bit_8476
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8477)]
    public static bool Bit_8477
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8478)]
    public static bool Bit_8478
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8479)]
    public static bool Bit_8479
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8480)]
    public static bool Bit_8480
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8481)]
    public static bool Bit_8481
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8482)]
    public static bool Bit_8482
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8483)]
    public static bool Bit_8483
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8484)]
    public static bool Bit_8484
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8485)]
    public static bool Bit_8485
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8486)]
    public static bool Bit_8486
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8487)]
    public static bool Bit_8487
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8488)]
    public static bool Bit_8488
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8489)]
    public static bool Bit_8489
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8490)]
    public static bool Bit_8490
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8491)]
    public static bool Bit_8491
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8492)]
    public static bool Bit_8492
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8493)]
    public static bool Bit_8493
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8494)]
    public static bool Bit_8494
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8495)]
    public static bool Bit_8495
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8496)]
    public static bool Bit_8496
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8497)]
    public static bool Bit_8497
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8498)]
    public static bool Bit_8498
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8499)]
    public static bool Bit_8499
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8500)]
    public static bool Bit_8500
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8501)]
    public static bool Bit_8501
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8502)]
    public static bool Bit_8502
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8503)]
    public static bool Bit_8503
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8510)]
    public static bool Bit_8510
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8511)]
    public static bool Bit_8511
    {
        get => ScriptMemory.ReadView(typeof(Globals)) != 0;
        set => ScriptMemory.WriteView(typeof(Globals), value ? 1 : 0);
    }

}

public sealed class GlobalState
{
    [StorageView(StorageScope.Global, StorageType.UInt16, 0)]
    public int ScenarioCounter
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 2)]
    public int FieldEntrance
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 4)]
    public int Byte_4
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 5)]
    public int Byte_5
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 6)]
    public int Byte_6
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 7)]
    public int Byte_7
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 8)]
    public int Byte_8
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 9)]
    public int EnvironmentSoundSlot0Id
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 11)]
    public int EnvironmentSoundSlot1Id
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 13)]
    public int EnvironmentSoundSlot0State
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 14)]
    public int EnvironmentSoundSlot1State
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 16)]
    public int TranceGaugeFlag
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 17)]
    public int GarnetDepressionFlag
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 18)]
    public int Byte_18
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 19)]
    public int UInt16_19
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 21)]
    public int UInt16_21
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.SByte, 28)]
    public int SByte_28
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.SByte, 60)]
    public int SByte_60
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.SByte, 61)]
    public int SByte_61
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.SByte, 62)]
    public int SByte_62
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.SByte, 63)]
    public int SByte_63
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 64)]
    public int Int24_64
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 67)]
    public int Int16_67
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 69)]
    public int Int24_69
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 72)]
    public int Byte_72
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 74)]
    public int Int24_74
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 77)]
    public int Int16_77
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 79)]
    public int Int24_79
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 82)]
    public int Byte_82
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 83)]
    public int Int24_83
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 86)]
    public int Int16_86
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 88)]
    public int Int24_88
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 91)]
    public int Byte_91
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 104)]
    public int Byte_104
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 112)]
    public int Int24_112
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 115)]
    public int Int24_115
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 118)]
    public int Int16_118
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 129)]
    public int Byte_129
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 131)]
    public int Byte_131
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 132)]
    public int Byte_132
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 134)]
    public int Byte_134
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 136)]
    public int UInt16_136
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 138)]
    public int Byte_138
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 139)]
    public int Byte_139
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 140)]
    public int Int16_140
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 142)]
    public int UInt16_142
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 146)]
    public int UInt16_146
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 148)]
    public int UInt16_148
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 150)]
    public int UInt16_150
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 152)]
    public int UInt16_152
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 154)]
    public int UInt16_154
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 156)]
    public int UInt16_156
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 158)]
    public int Byte_158
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 159)]
    public int Byte_159
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 160)]
    public int Byte_160
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 161)]
    public int Byte_161
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 162)]
    public int Byte_162
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 163)]
    public int Byte_163
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 164)]
    public int Byte_164
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 165)]
    public int Byte_165
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 166)]
    public int Byte_166
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 167)]
    public int Byte_167
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 168)]
    public int Byte_168
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 171)]
    public int Byte_171
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 172)]
    public int Int24_172
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 175)]
    public int Int16_175
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 178)]
    public int UInt16_178
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 182)]
    public int UInt16_182
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 184)]
    public bool Bit_184
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 184)]
    public int Int24_184
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 187)]
    public int Int24_187
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 189)]
    public bool Bit_189
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 190)]
    public int Byte_190
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 191)]
    public bool Bit_191
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 191)]
    public int Byte_191
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 196)]
    public bool Bit_196
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 197)]
    public bool Bit_197
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 198)]
    public bool Bit_198
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 198)]
    public int Byte_198
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 199)]
    public bool Bit_199
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 200)]
    public int Byte_200
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 208)]
    public int Byte_208
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 209)]
    public int Byte_209
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 210)]
    public int Byte_210
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 211)]
    public int Byte_211
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 212)]
    public int Byte_212
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 213)]
    public int Byte_213
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 214)]
    public int Int16_214
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 216)]
    public int Int16_216
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 218)]
    public int Int16_218
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 220)]
    public int Int16_220
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 222)]
    public int Int16_222
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 224)]
    public int Int16_224
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 226)]
    public int Byte_226
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 228)]
    public int Int16_228
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 234)]
    public int UInt16_234
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 236)]
    public int UInt16_236
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.SByte, 238)]
    public int SByte_238
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 239)]
    public int Int16_239
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 241)]
    public int Int16_241
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 243)]
    public int Int16_243
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 245)]
    public int Int16_245
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 247)]
    public int Int16_247
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 249)]
    public int Byte_249
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 250)]
    public int Byte_250
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 251)]
    public int UInt16_251
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 256)]
    public int Byte_256
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 257)]
    public int Byte_257
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 258)]
    public int Byte_258
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 259)]
    public int Byte_259
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 260)]
    public int Byte_260
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 263)]
    public int Byte_263
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 264)]
    public int Byte_264
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 265)]
    public int Byte_265
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 266)]
    public int Byte_266
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 267)]
    public int Int16_267
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 269)]
    public int Int16_269
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 271)]
    public int Int16_271
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 273)]
    public int Int16_273
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 275)]
    public int Int16_275
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 277)]
    public int Int16_277
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 279)]
    public int Byte_279
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 280)]
    public int Byte_280
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 281)]
    public int Byte_281
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 282)]
    public int Byte_282
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 283)]
    public int Byte_283
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 284)]
    public int Byte_284
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 285)]
    public int Byte_285
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 286)]
    public int Byte_286
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 287)]
    public int Byte_287
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 288)]
    public int Byte_288
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 289)]
    public int Byte_289
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 290)]
    public int Byte_290
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 291)]
    public int Byte_291
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 292)]
    public int Byte_292
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.SByte, 293)]
    public int SByte_293
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 294)]
    public int UInt16_294
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.SByte, 296)]
    public int SByte_296
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 297)]
    public int UInt16_297
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 299)]
    public int Byte_299
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 303)]
    public int Byte_303
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 312)]
    public int Byte_312
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 313)]
    public int Byte_313
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 314)]
    public int UInt16_314
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 316)]
    public int UInt16_316
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 318)]
    public int UInt16_318
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 320)]
    public int UInt16_320
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 322)]
    public int UInt16_322
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 329)]
    public int Byte_329
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 333)]
    public int UInt16_333
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 335)]
    public int UInt16_335
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 355)]
    public int UInt16_355
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 358)]
    public int Byte_358
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 361)]
    public int Byte_361
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 362)]
    public int Byte_362
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 363)]
    public int Byte_363
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 369)]
    public int Byte_369
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 370)]
    public int Byte_370
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 396)]
    public int UInt16_396
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 398)]
    public int Byte_398
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 399)]
    public int Byte_399
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 400)]
    public int Byte_400
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 401)]
    public int Byte_401
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 408)]
    public int Byte_408
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 409)]
    public int Byte_409
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 418)]
    public int Int16_418
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 420)]
    public int Int16_420
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 426)]
    public int Int24_426
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 429)]
    public int Int24_429
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 433)]
    public int Byte_433
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 434)]
    public int Byte_434
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 435)]
    public int Int16_435
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 437)]
    public int Int16_437
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 439)]
    public int Int16_439
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 441)]
    public int Byte_441
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 442)]
    public int Byte_442
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 444)]
    public int Byte_444
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 445)]
    public int Int24_445
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 450)]
    public int Byte_450
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 455)]
    public int Byte_455
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 456)]
    public int Int16_456
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 458)]
    public int Byte_458
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 465)]
    public int Byte_465
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 467)]
    public int Byte_467
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 468)]
    public int Byte_468
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 469)]
    public int Int16_469
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 471)]
    public int Byte_471
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 472)]
    public int Byte_472
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 475)]
    public int Byte_475
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 479)]
    public int Byte_479
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 483)]
    public int Byte_483
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 484)]
    public int Byte_484
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 489)]
    public int Byte_489
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 490)]
    public int Byte_490
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 491)]
    public int Byte_491
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 492)]
    public int Byte_492
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 496)]
    public int Int24_496
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 499)]
    public int Int24_499
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 502)]
    public int Int24_502
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 506)]
    public int UInt16_506
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 508)]
    public int Byte_508
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 509)]
    public int Byte_509
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 736)]
    public bool Bit_736
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 737)]
    public bool Bit_737
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 738)]
    public bool Bit_738
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 739)]
    public bool Bit_739
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 740)]
    public bool Bit_740
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 741)]
    public bool Bit_741
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 742)]
    public bool Bit_742
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 743)]
    public bool Bit_743
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 744)]
    public bool Bit_744
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 745)]
    public bool Bit_745
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 746)]
    public bool Bit_746
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 747)]
    public bool Bit_747
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 748)]
    public bool Bit_748
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 749)]
    public bool Bit_749
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 750)]
    public bool Bit_750
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 751)]
    public bool Bit_751
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 752)]
    public bool Bit_752
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 753)]
    public bool Bit_753
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 754)]
    public bool Bit_754
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 755)]
    public bool Bit_755
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 756)]
    public bool Bit_756
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 757)]
    public bool Bit_757
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 758)]
    public bool Bit_758
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 759)]
    public bool Bit_759
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 760)]
    public bool Bit_760
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 761)]
    public bool Bit_761
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 763)]
    public bool Bit_763
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 765)]
    public bool Bit_765
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 766)]
    public bool Bit_766
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 767)]
    public bool Bit_767
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 768)]
    public bool Bit_768
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 768)]
    public int Int24_768
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 769)]
    public bool Bit_769
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 770)]
    public bool Bit_770
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 771)]
    public bool Bit_771
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 771)]
    public int Int24_771
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 772)]
    public bool Bit_772
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 773)]
    public bool Bit_773
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 774)]
    public int Int24_774
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 775)]
    public bool Bit_775
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 776)]
    public bool Bit_776
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 777)]
    public bool Bit_777
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 777)]
    public int Int24_777
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 780)]
    public bool Bit_780
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 780)]
    public int Byte_780
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 781)]
    public bool Bit_781
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 781)]
    public int Byte_781
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 782)]
    public bool Bit_782
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 782)]
    public int Byte_782
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 783)]
    public bool Bit_783
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 783)]
    public int Byte_783
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.SByte, 784)]
    public int SByte_784
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 785)]
    public bool Bit_785
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 785)]
    public int Byte_785
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 786)]
    public int Byte_786
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 787)]
    public int Byte_787
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 788)]
    public bool Bit_788
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 788)]
    public int Byte_788
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 789)]
    public bool Bit_789
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 789)]
    public int Byte_789
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 790)]
    public int Byte_790
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 791)]
    public int Byte_791
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 792)]
    public int Byte_792
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 793)]
    public int Byte_793
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 794)]
    public int Byte_794
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 795)]
    public bool Bit_795
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 795)]
    public int Byte_795
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 796)]
    public int Byte_796
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 797)]
    public int Byte_797
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 798)]
    public int Byte_798
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 799)]
    public int Byte_799
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 800)]
    public int Byte_800
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 801)]
    public int Byte_801
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 802)]
    public int Byte_802
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 803)]
    public int UInt16_803
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 805)]
    public int Byte_805
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 806)]
    public int Byte_806
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 807)]
    public int Byte_807
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 808)]
    public bool Bit_808
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 808)]
    public int Byte_808
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 809)]
    public bool Bit_809
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 809)]
    public int Byte_809
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 810)]
    public bool Bit_810
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 810)]
    public int Byte_810
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 811)]
    public int Byte_811
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 812)]
    public int Byte_812
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 813)]
    public int Byte_813
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 814)]
    public int Byte_814
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 815)]
    public int Byte_815
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 816)]
    public int Byte_816
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 817)]
    public int Byte_817
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 818)]
    public int Byte_818
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 819)]
    public int Byte_819
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 820)]
    public int Byte_820
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 848)]
    public bool Bit_848
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 849)]
    public bool Bit_849
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 852)]
    public bool Bit_852
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 853)]
    public bool Bit_853
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 888)]
    public bool Bit_888
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 889)]
    public bool Bit_889
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 890)]
    public bool Bit_890
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 891)]
    public bool Bit_891
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 892)]
    public bool Bit_892
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 893)]
    public bool Bit_893
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 894)]
    public bool Bit_894
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 895)]
    public bool Bit_895
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 896)]
    public int Byte_896
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 897)]
    public int Byte_897
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 898)]
    public int Byte_898
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 899)]
    public int Byte_899
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 900)]
    public int Byte_900
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 901)]
    public int Byte_901
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 902)]
    public int Byte_902
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 903)]
    public int Byte_903
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 904)]
    public int Byte_904
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 905)]
    public int Byte_905
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 906)]
    public int Byte_906
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 907)]
    public int Byte_907
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 908)]
    public int Byte_908
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 909)]
    public int Byte_909
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 910)]
    public int Byte_910
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 911)]
    public int Byte_911
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 912)]
    public int Byte_912
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 913)]
    public int Byte_913
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 914)]
    public int Byte_914
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 915)]
    public int Byte_915
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 916)]
    public int Byte_916
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 917)]
    public int Byte_917
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 918)]
    public int Byte_918
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 919)]
    public int Byte_919
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 920)]
    public int Byte_920
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 921)]
    public int Byte_921
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 922)]
    public int Byte_922
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 923)]
    public int Byte_923
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 924)]
    public int Byte_924
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 925)]
    public int Byte_925
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 926)]
    public int Byte_926
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 927)]
    public int Byte_927
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 928)]
    public int Byte_928
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 929)]
    public int Byte_929
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 930)]
    public int Byte_930
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 931)]
    public int Byte_931
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 932)]
    public int Byte_932
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 933)]
    public int Byte_933
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 934)]
    public int Byte_934
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 935)]
    public int Byte_935
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 936)]
    public int Byte_936
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 937)]
    public int Byte_937
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 938)]
    public int Byte_938
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 939)]
    public int Byte_939
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 940)]
    public int Byte_940
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 941)]
    public int Byte_941
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 942)]
    public int Byte_942
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 943)]
    public int Byte_943
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 944)]
    public int Byte_944
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 945)]
    public int Byte_945
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 946)]
    public int Byte_946
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 947)]
    public int Byte_947
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 948)]
    public int Byte_948
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 949)]
    public int Byte_949
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 950)]
    public int Byte_950
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 951)]
    public int Byte_951
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 952)]
    public int Byte_952
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 953)]
    public int Byte_953
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 954)]
    public int Byte_954
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 955)]
    public int Byte_955
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 956)]
    public int Byte_956
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 957)]
    public int Byte_957
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 958)]
    public int Byte_958
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 959)]
    public int Byte_959
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 960)]
    public int Byte_960
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 966)]
    public int Byte_966
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 967)]
    public int Byte_967
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 968)]
    public int Byte_968
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 969)]
    public int Byte_969
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 970)]
    public int Byte_970
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 971)]
    public int Byte_971
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 972)]
    public int Byte_972
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 973)]
    public int Byte_973
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 974)]
    public int Byte_974
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 975)]
    public int Byte_975
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1024)]
    public int Byte_1024
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1032)]
    public int Byte_1032
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1033)]
    public int Byte_1033
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1034)]
    public int Byte_1034
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1035)]
    public int Byte_1035
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1036)]
    public int Byte_1036
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1037)]
    public int Byte_1037
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1038)]
    public int Byte_1038
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1039)]
    public int Byte_1039
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1040)]
    public bool Bit_1040
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1040)]
    public int Byte_1040
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1041)]
    public bool Bit_1041
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1041)]
    public int Byte_1041
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1042)]
    public bool Bit_1042
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1042)]
    public int Byte_1042
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1043)]
    public bool Bit_1043
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1043)]
    public int Byte_1043
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1044)]
    public bool Bit_1044
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1044)]
    public int Byte_1044
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1045)]
    public bool Bit_1045
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1045)]
    public int Byte_1045
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1046)]
    public bool Bit_1046
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1047)]
    public bool Bit_1047
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1064)]
    public bool Bit_1064
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1064)]
    public int Byte_1064
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1065)]
    public bool Bit_1065
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1065)]
    public int Byte_1065
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1066)]
    public bool Bit_1066
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1066)]
    public int Byte_1066
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1067)]
    public bool Bit_1067
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1067)]
    public int Byte_1067
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1068)]
    public bool Bit_1068
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1068)]
    public int Byte_1068
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1069)]
    public bool Bit_1069
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1069)]
    public int Byte_1069
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1070)]
    public bool Bit_1070
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1070)]
    public int Byte_1070
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1071)]
    public bool Bit_1071
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1071)]
    public int Byte_1071
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1072)]
    public int Byte_1072
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1073)]
    public int Byte_1073
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1079)]
    public int Byte_1079
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1080)]
    public bool Bit_1080
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1080)]
    public int Byte_1080
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1081)]
    public bool Bit_1081
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1081)]
    public int Byte_1081
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1082)]
    public bool Bit_1082
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1082)]
    public int Byte_1082
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1083)]
    public bool Bit_1083
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1083)]
    public int Byte_1083
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1084)]
    public bool Bit_1084
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1084)]
    public int Byte_1084
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1085)]
    public int Byte_1085
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1086)]
    public bool Bit_1086
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1086)]
    public int Byte_1086
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1087)]
    public bool Bit_1087
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1087)]
    public int Byte_1087
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1088)]
    public int Byte_1088
    {
        get => ScriptMemory.ReadView(typeof(GlobalState));
        set => ScriptMemory.WriteView(typeof(GlobalState), value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1156)]
    public bool Bit_1156
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1157)]
    public bool Bit_1157
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1158)]
    public bool Bit_1158
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1159)]
    public bool Bit_1159
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1416)]
    public bool Bit_1416
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1417)]
    public bool Bit_1417
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1419)]
    public bool Bit_1419
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1421)]
    public bool Bit_1421
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1422)]
    public bool Bit_1422
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1423)]
    public bool Bit_1423
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1463)]
    public bool Bit_1463
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1584)]
    public bool Bit_1584
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1816)]
    public bool Bit_1816
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2048)]
    public bool Bit_2048
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2049)]
    public bool Bit_2049
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2050)]
    public bool Bit_2050
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2051)]
    public bool Bit_2051
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2052)]
    public bool Bit_2052
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2053)]
    public bool Bit_2053
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2054)]
    public bool Bit_2054
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2055)]
    public bool Bit_2055
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2056)]
    public bool Bit_2056
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2057)]
    public bool Bit_2057
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2058)]
    public bool Bit_2058
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2059)]
    public bool Bit_2059
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2060)]
    public bool Bit_2060
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2061)]
    public bool Bit_2061
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2062)]
    public bool Bit_2062
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2063)]
    public bool Bit_2063
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2064)]
    public bool Bit_2064
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2065)]
    public bool Bit_2065
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2066)]
    public bool Bit_2066
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2067)]
    public bool Bit_2067
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2068)]
    public bool Bit_2068
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2069)]
    public bool Bit_2069
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2070)]
    public bool Bit_2070
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2071)]
    public bool Bit_2071
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2072)]
    public bool Bit_2072
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2073)]
    public bool Bit_2073
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2074)]
    public bool Bit_2074
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2075)]
    public bool Bit_2075
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2076)]
    public bool Bit_2076
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2077)]
    public bool Bit_2077
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2078)]
    public bool Bit_2078
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2079)]
    public bool Bit_2079
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2080)]
    public bool Bit_2080
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2081)]
    public bool Bit_2081
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2082)]
    public bool Bit_2082
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2083)]
    public bool Bit_2083
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2084)]
    public bool Bit_2084
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2085)]
    public bool Bit_2085
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2086)]
    public bool Bit_2086
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2087)]
    public bool Bit_2087
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2088)]
    public bool Bit_2088
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2090)]
    public bool Bit_2090
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2091)]
    public bool Bit_2091
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2092)]
    public bool Bit_2092
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2093)]
    public bool Bit_2093
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2095)]
    public bool Bit_2095
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2096)]
    public bool Bit_2096
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2097)]
    public bool Bit_2097
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2098)]
    public bool Bit_2098
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2099)]
    public bool Bit_2099
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2100)]
    public bool Bit_2100
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2101)]
    public bool Bit_2101
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2102)]
    public bool Bit_2102
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2103)]
    public bool Bit_2103
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2128)]
    public bool Bit_2128
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2418)]
    public bool Bit_2418
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2419)]
    public bool Bit_2419
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2420)]
    public bool Bit_2420
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2421)]
    public bool Bit_2421
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2422)]
    public bool Bit_2422
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2423)]
    public bool Bit_2423
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2432)]
    public bool Bit_2432
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2435)]
    public bool Bit_2435
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2436)]
    public bool Bit_2436
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2437)]
    public bool Bit_2437
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2438)]
    public bool Bit_2438
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2439)]
    public bool Bit_2439
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2440)]
    public bool Bit_2440
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2441)]
    public bool Bit_2441
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2442)]
    public bool Bit_2442
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2444)]
    public bool Bit_2444
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2445)]
    public bool Bit_2445
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2446)]
    public bool Bit_2446
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2447)]
    public bool Bit_2447
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2448)]
    public bool Bit_2448
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2455)]
    public bool Bit_2455
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2456)]
    public bool Bit_2456
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2458)]
    public bool Bit_2458
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2466)]
    public bool Bit_2466
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2471)]
    public bool Bit_2471
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2474)]
    public bool Bit_2474
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2476)]
    public bool Bit_2476
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2477)]
    public bool Bit_2477
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2478)]
    public bool Bit_2478
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2479)]
    public bool Bit_2479
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2480)]
    public bool Bit_2480
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2481)]
    public bool Bit_2481
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2486)]
    public bool Bit_2486
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2487)]
    public bool Bit_2487
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2488)]
    public bool Bit_2488
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2491)]
    public bool Bit_2491
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2492)]
    public bool Bit_2492
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2493)]
    public bool Bit_2493
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2494)]
    public bool Bit_2494
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2495)]
    public bool Bit_2495
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2592)]
    public bool Bit_2592
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2593)]
    public bool Bit_2593
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2594)]
    public bool Bit_2594
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2595)]
    public bool Bit_2595
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2596)]
    public bool Bit_2596
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2597)]
    public bool Bit_2597
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2598)]
    public bool Bit_2598
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2599)]
    public bool Bit_2599
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2600)]
    public bool Bit_2600
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2601)]
    public bool Bit_2601
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2602)]
    public bool Bit_2602
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2603)]
    public bool Bit_2603
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2604)]
    public bool Bit_2604
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2605)]
    public bool Bit_2605
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2606)]
    public bool Bit_2606
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2607)]
    public bool Bit_2607
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2609)]
    public bool Bit_2609
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2611)]
    public bool Bit_2611
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2612)]
    public bool Bit_2612
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2615)]
    public bool Bit_2615
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2616)]
    public bool Bit_2616
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2617)]
    public bool Bit_2617
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2618)]
    public bool Bit_2618
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2619)]
    public bool Bit_2619
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2620)]
    public bool Bit_2620
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2621)]
    public bool Bit_2621
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2622)]
    public bool Bit_2622
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2623)]
    public bool Bit_2623
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2625)]
    public bool Bit_2625
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2626)]
    public bool Bit_2626
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2627)]
    public bool Bit_2627
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2628)]
    public bool Bit_2628
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2630)]
    public bool Bit_2630
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2631)]
    public bool Bit_2631
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2641)]
    public bool Bit_2641
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2642)]
    public bool Bit_2642
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2643)]
    public bool Bit_2643
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2645)]
    public bool Bit_2645
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2646)]
    public bool Bit_2646
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2647)]
    public bool Bit_2647
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2648)]
    public bool Bit_2648
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2649)]
    public bool Bit_2649
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2650)]
    public bool Bit_2650
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2651)]
    public bool Bit_2651
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2652)]
    public bool Bit_2652
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2653)]
    public bool Bit_2653
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2654)]
    public bool Bit_2654
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2655)]
    public bool Bit_2655
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2660)]
    public bool Bit_2660
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2661)]
    public bool Bit_2661
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2662)]
    public bool Bit_2662
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2663)]
    public bool Bit_2663
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2816)]
    public bool Bit_2816
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2817)]
    public bool Bit_2817
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2818)]
    public bool Bit_2818
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2819)]
    public bool Bit_2819
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2820)]
    public bool Bit_2820
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2821)]
    public bool Bit_2821
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2822)]
    public bool Bit_2822
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2823)]
    public bool Bit_2823
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2824)]
    public bool Bit_2824
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2825)]
    public bool Bit_2825
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2826)]
    public bool Bit_2826
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2827)]
    public bool Bit_2827
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2828)]
    public bool Bit_2828
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2829)]
    public bool Bit_2829
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2830)]
    public bool Bit_2830
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2831)]
    public bool Bit_2831
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2836)]
    public bool Bit_2836
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2837)]
    public bool Bit_2837
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2838)]
    public bool Bit_2838
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2839)]
    public bool Bit_2839
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2856)]
    public bool Bit_2856
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2857)]
    public bool Bit_2857
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2858)]
    public bool Bit_2858
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2859)]
    public bool Bit_2859
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2860)]
    public bool Bit_2860
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2861)]
    public bool Bit_2861
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2862)]
    public bool Bit_2862
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2863)]
    public bool Bit_2863
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2864)]
    public bool Bit_2864
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2865)]
    public bool Bit_2865
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2866)]
    public bool Bit_2866
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2867)]
    public bool Bit_2867
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2872)]
    public bool Bit_2872
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2873)]
    public bool Bit_2873
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2874)]
    public bool Bit_2874
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2875)]
    public bool Bit_2875
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2876)]
    public bool Bit_2876
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2877)]
    public bool Bit_2877
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2878)]
    public bool Bit_2878
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2879)]
    public bool Bit_2879
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2884)]
    public bool Bit_2884
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2885)]
    public bool Bit_2885
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2886)]
    public bool Bit_2886
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2887)]
    public bool Bit_2887
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2906)]
    public bool Bit_2906
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2907)]
    public bool Bit_2907
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2908)]
    public bool Bit_2908
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2909)]
    public bool Bit_2909
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2910)]
    public bool Bit_2910
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2911)]
    public bool Bit_2911
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2918)]
    public bool Bit_2918
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2919)]
    public bool Bit_2919
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2921)]
    public bool Bit_2921
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2922)]
    public bool Bit_2922
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2923)]
    public bool Bit_2923
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2924)]
    public bool Bit_2924
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2925)]
    public bool Bit_2925
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2926)]
    public bool Bit_2926
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2927)]
    public bool Bit_2927
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2930)]
    public bool Bit_2930
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2931)]
    public bool Bit_2931
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2932)]
    public bool Bit_2932
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2933)]
    public bool Bit_2933
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2934)]
    public bool Bit_2934
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2935)]
    public bool Bit_2935
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2938)]
    public bool Bit_2938
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2939)]
    public bool Bit_2939
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2940)]
    public bool Bit_2940
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2941)]
    public bool Bit_2941
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2942)]
    public bool Bit_2942
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2943)]
    public bool Bit_2943
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2948)]
    public bool Bit_2948
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2949)]
    public bool Bit_2949
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2950)]
    public bool Bit_2950
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2951)]
    public bool Bit_2951
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2969)]
    public bool Bit_2969
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2970)]
    public bool Bit_2970
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2971)]
    public bool Bit_2971
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2972)]
    public bool Bit_2972
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2973)]
    public bool Bit_2973
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2974)]
    public bool Bit_2974
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2975)]
    public bool Bit_2975
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2976)]
    public bool Bit_2976
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2977)]
    public bool Bit_2977
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2978)]
    public bool Bit_2978
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2979)]
    public bool Bit_2979
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2980)]
    public bool Bit_2980
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2981)]
    public bool Bit_2981
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2982)]
    public bool Bit_2982
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2983)]
    public bool Bit_2983
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3228)]
    public bool Bit_3228
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3229)]
    public bool Bit_3229
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3230)]
    public bool Bit_3230
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3231)]
    public bool Bit_3231
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3232)]
    public bool Bit_3232
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3233)]
    public bool Bit_3233
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3234)]
    public bool Bit_3234
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3235)]
    public bool Bit_3235
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3236)]
    public bool Bit_3236
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3237)]
    public bool Bit_3237
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3238)]
    public bool Bit_3238
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3239)]
    public bool Bit_3239
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3240)]
    public bool Bit_3240
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3242)]
    public bool Bit_3242
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3243)]
    public bool Bit_3243
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3244)]
    public bool Bit_3244
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3245)]
    public bool Bit_3245
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3246)]
    public bool Bit_3246
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3247)]
    public bool Bit_3247
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3248)]
    public bool Bit_3248
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3249)]
    public bool Bit_3249
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3250)]
    public bool Bit_3250
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3251)]
    public bool Bit_3251
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3252)]
    public bool Bit_3252
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3253)]
    public bool Bit_3253
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3254)]
    public bool Bit_3254
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3255)]
    public bool Bit_3255
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3256)]
    public bool Bit_3256
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3257)]
    public bool Bit_3257
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3258)]
    public bool Bit_3258
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3259)]
    public bool Bit_3259
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3260)]
    public bool Bit_3260
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3261)]
    public bool Bit_3261
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3262)]
    public bool Bit_3262
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3263)]
    public bool Bit_3263
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3456)]
    public bool Bit_3456
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3457)]
    public bool Bit_3457
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3458)]
    public bool Bit_3458
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3459)]
    public bool Bit_3459
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3460)]
    public bool Bit_3460
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3461)]
    public bool Bit_3461
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3462)]
    public bool Bit_3462
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3463)]
    public bool Bit_3463
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3464)]
    public bool Bit_3464
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3465)]
    public bool Bit_3465
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3466)]
    public bool Bit_3466
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3468)]
    public bool Bit_3468
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3469)]
    public bool Bit_3469
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3470)]
    public bool Bit_3470
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3471)]
    public bool Bit_3471
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3536)]
    public bool Bit_3536
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3537)]
    public bool Bit_3537
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3538)]
    public bool Bit_3538
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3539)]
    public bool Bit_3539
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3540)]
    public bool Bit_3540
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3541)]
    public bool Bit_3541
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3542)]
    public bool Bit_3542
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3543)]
    public bool Bit_3543
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3544)]
    public bool Bit_3544
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3545)]
    public bool Bit_3545
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3546)]
    public bool Bit_3546
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3547)]
    public bool Bit_3547
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3550)]
    public bool Bit_3550
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3551)]
    public bool Bit_3551
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3552)]
    public bool Bit_3552
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3553)]
    public bool Bit_3553
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3554)]
    public bool Bit_3554
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3555)]
    public bool Bit_3555
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3556)]
    public bool Bit_3556
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3557)]
    public bool Bit_3557
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3558)]
    public bool Bit_3558
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3559)]
    public bool Bit_3559
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3560)]
    public bool Bit_3560
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3561)]
    public bool Bit_3561
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3562)]
    public bool Bit_3562
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3563)]
    public bool Bit_3563
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3564)]
    public bool Bit_3564
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3565)]
    public bool Bit_3565
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3566)]
    public bool Bit_3566
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3567)]
    public bool Bit_3567
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3568)]
    public bool Bit_3568
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3569)]
    public bool Bit_3569
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3570)]
    public bool Bit_3570
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3571)]
    public bool Bit_3571
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3572)]
    public bool Bit_3572
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3573)]
    public bool Bit_3573
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3574)]
    public bool Bit_3574
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3575)]
    public bool Bit_3575
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3576)]
    public bool Bit_3576
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3577)]
    public bool Bit_3577
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3578)]
    public bool Bit_3578
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3579)]
    public bool Bit_3579
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3580)]
    public bool Bit_3580
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3581)]
    public bool Bit_3581
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3582)]
    public bool Bit_3582
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3583)]
    public bool Bit_3583
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3584)]
    public bool Bit_3584
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3585)]
    public bool Bit_3585
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3586)]
    public bool Bit_3586
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3587)]
    public bool Bit_3587
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3588)]
    public bool Bit_3588
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3589)]
    public bool Bit_3589
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3590)]
    public bool Bit_3590
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3591)]
    public bool Bit_3591
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3593)]
    public bool Bit_3593
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3594)]
    public bool Bit_3594
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3595)]
    public bool Bit_3595
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3596)]
    public bool Bit_3596
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3597)]
    public bool Bit_3597
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3599)]
    public bool Bit_3599
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3600)]
    public bool Bit_3600
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3602)]
    public bool Bit_3602
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3603)]
    public bool Bit_3603
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3604)]
    public bool Bit_3604
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3605)]
    public bool Bit_3605
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3606)]
    public bool Bit_3606
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3607)]
    public bool Bit_3607
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3608)]
    public bool Bit_3608
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3609)]
    public bool Bit_3609
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3610)]
    public bool Bit_3610
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3611)]
    public bool Bit_3611
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3612)]
    public bool Bit_3612
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3613)]
    public bool Bit_3613
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3614)]
    public bool Bit_3614
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3615)]
    public bool Bit_3615
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3616)]
    public bool Bit_3616
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3617)]
    public bool Bit_3617
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3618)]
    public bool Bit_3618
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3619)]
    public bool Bit_3619
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3620)]
    public bool Bit_3620
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3621)]
    public bool Bit_3621
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3622)]
    public bool Bit_3622
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3623)]
    public bool Bit_3623
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3630)]
    public bool Bit_3630
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3631)]
    public bool Bit_3631
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3632)]
    public bool Bit_3632
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3664)]
    public bool Bit_3664
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3665)]
    public bool Bit_3665
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3666)]
    public bool Bit_3666
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3667)]
    public bool Bit_3667
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3668)]
    public bool Bit_3668
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3669)]
    public bool Bit_3669
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3670)]
    public bool Bit_3670
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3671)]
    public bool Bit_3671
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3712)]
    public bool Bit_3712
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3713)]
    public bool Bit_3713
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3714)]
    public bool Bit_3714
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3715)]
    public bool Bit_3715
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3716)]
    public bool Bit_3716
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3717)]
    public bool Bit_3717
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3718)]
    public bool Bit_3718
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3719)]
    public bool Bit_3719
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3784)]
    public bool Bit_3784
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3787)]
    public bool Bit_3787
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3788)]
    public bool Bit_3788
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3789)]
    public bool Bit_3789
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3790)]
    public bool Bit_3790
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3791)]
    public bool Bit_3791
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3792)]
    public bool Bit_3792
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3793)]
    public bool Bit_3793
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3794)]
    public bool Bit_3794
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3795)]
    public bool Bit_3795
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3796)]
    public bool Bit_3796
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3797)]
    public bool Bit_3797
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3798)]
    public bool Bit_3798
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3799)]
    public bool Bit_3799
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3811)]
    public bool Bit_3811
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3815)]
    public bool Bit_3815
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3816)]
    public bool Bit_3816
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3817)]
    public bool Bit_3817
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3818)]
    public bool Bit_3818
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3819)]
    public bool Bit_3819
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3820)]
    public bool Bit_3820
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3821)]
    public bool Bit_3821
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3822)]
    public bool Bit_3822
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3823)]
    public bool Bit_3823
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3824)]
    public bool Bit_3824
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3825)]
    public bool Bit_3825
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3826)]
    public bool Bit_3826
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3827)]
    public bool Bit_3827
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3828)]
    public bool Bit_3828
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3829)]
    public bool Bit_3829
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3830)]
    public bool Bit_3830
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3831)]
    public bool Bit_3831
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3840)]
    public bool Bit_3840
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3843)]
    public bool Bit_3843
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3846)]
    public bool Bit_3846
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3847)]
    public bool Bit_3847
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3848)]
    public bool Bit_3848
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3849)]
    public bool Bit_3849
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3850)]
    public bool Bit_3850
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3851)]
    public bool Bit_3851
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3852)]
    public bool Bit_3852
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3853)]
    public bool Bit_3853
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3854)]
    public bool Bit_3854
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3855)]
    public bool Bit_3855
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3856)]
    public bool Bit_3856
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3857)]
    public bool Bit_3857
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3860)]
    public bool Bit_3860
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3861)]
    public bool Bit_3861
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3862)]
    public bool Bit_3862
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3863)]
    public bool Bit_3863
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3881)]
    public bool Bit_3881
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3882)]
    public bool Bit_3882
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3883)]
    public bool Bit_3883
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3884)]
    public bool Bit_3884
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3885)]
    public bool Bit_3885
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3886)]
    public bool Bit_3886
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3887)]
    public bool Bit_3887
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3888)]
    public bool Bit_3888
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3889)]
    public bool Bit_3889
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3890)]
    public bool Bit_3890
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3891)]
    public bool Bit_3891
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3895)]
    public bool Bit_3895
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3896)]
    public bool Bit_3896
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3897)]
    public bool Bit_3897
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3898)]
    public bool Bit_3898
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3899)]
    public bool Bit_3899
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3900)]
    public bool Bit_3900
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3901)]
    public bool Bit_3901
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3902)]
    public bool Bit_3902
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3903)]
    public bool Bit_3903
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3904)]
    public bool Bit_3904
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3905)]
    public bool Bit_3905
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3944)]
    public bool Bit_3944
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3945)]
    public bool Bit_3945
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3946)]
    public bool Bit_3946
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3947)]
    public bool Bit_3947
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3948)]
    public bool Bit_3948
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3949)]
    public bool Bit_3949
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3950)]
    public bool Bit_3950
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3951)]
    public bool Bit_3951
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3952)]
    public bool Bit_3952
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3966)]
    public bool Bit_3966
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3967)]
    public bool Bit_3967
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 4046)]
    public bool Bit_4046
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 4047)]
    public bool Bit_4047
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7171)]
    public bool Bit_7171
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7172)]
    public bool Bit_7172
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7174)]
    public bool Bit_7174
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7175)]
    public bool Bit_7175
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7184)]
    public bool Bit_7184
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7185)]
    public bool Bit_7185
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7186)]
    public bool Bit_7186
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7187)]
    public bool Bit_7187
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7188)]
    public bool Bit_7188
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7189)]
    public bool Bit_7189
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7190)]
    public bool Bit_7190
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7191)]
    public bool Bit_7191
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7192)]
    public bool Bit_7192
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7193)]
    public bool Bit_7193
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7194)]
    public bool Bit_7194
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7195)]
    public bool Bit_7195
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7196)]
    public bool Bit_7196
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7197)]
    public bool Bit_7197
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7198)]
    public bool Bit_7198
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7199)]
    public bool Bit_7199
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7200)]
    public bool Bit_7200
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7201)]
    public bool Bit_7201
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7202)]
    public bool Bit_7202
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7203)]
    public bool Bit_7203
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7204)]
    public bool Bit_7204
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7205)]
    public bool Bit_7205
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7206)]
    public bool Bit_7206
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7207)]
    public bool Bit_7207
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7211)]
    public bool Bit_7211
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7212)]
    public bool Bit_7212
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7213)]
    public bool Bit_7213
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7214)]
    public bool Bit_7214
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7215)]
    public bool Bit_7215
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7223)]
    public bool Bit_7223
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7230)]
    public bool Bit_7230
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7231)]
    public bool Bit_7231
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7240)]
    public bool Bit_7240
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7241)]
    public bool Bit_7241
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7242)]
    public bool Bit_7242
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7243)]
    public bool Bit_7243
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7244)]
    public bool Bit_7244
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7245)]
    public bool Bit_7245
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7246)]
    public bool Bit_7246
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7247)]
    public bool Bit_7247
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7264)]
    public bool Bit_7264
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7265)]
    public bool Bit_7265
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7266)]
    public bool Bit_7266
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7267)]
    public bool Bit_7267
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7268)]
    public bool Bit_7268
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7269)]
    public bool Bit_7269
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7270)]
    public bool Bit_7270
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7271)]
    public bool Bit_7271
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7272)]
    public bool Bit_7272
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7273)]
    public bool Bit_7273
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7274)]
    public bool Bit_7274
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7275)]
    public bool Bit_7275
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7276)]
    public bool Bit_7276
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7277)]
    public bool Bit_7277
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7278)]
    public bool Bit_7278
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7279)]
    public bool Bit_7279
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7280)]
    public bool Bit_7280
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7281)]
    public bool Bit_7281
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7282)]
    public bool Bit_7282
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7283)]
    public bool Bit_7283
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7284)]
    public bool Bit_7284
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7285)]
    public bool Bit_7285
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7286)]
    public bool Bit_7286
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7287)]
    public bool Bit_7287
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7292)]
    public bool Bit_7292
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7294)]
    public bool Bit_7294
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7295)]
    public bool Bit_7295
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7296)]
    public bool Bit_7296
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7297)]
    public bool Bit_7297
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7298)]
    public bool Bit_7298
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7299)]
    public bool Bit_7299
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7300)]
    public bool Bit_7300
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7301)]
    public bool Bit_7301
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7302)]
    public bool Bit_7302
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7303)]
    public bool Bit_7303
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7304)]
    public bool Bit_7304
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7309)]
    public bool Bit_7309
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7310)]
    public bool Bit_7310
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7311)]
    public bool Bit_7311
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7312)]
    public bool Bit_7312
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7313)]
    public bool Bit_7313
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7314)]
    public bool Bit_7314
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7315)]
    public bool Bit_7315
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7316)]
    public bool Bit_7316
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7317)]
    public bool Bit_7317
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7318)]
    public bool Bit_7318
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7319)]
    public bool Bit_7319
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7320)]
    public bool Bit_7320
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7321)]
    public bool Bit_7321
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7322)]
    public bool Bit_7322
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7323)]
    public bool Bit_7323
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7324)]
    public bool Bit_7324
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7325)]
    public bool Bit_7325
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7326)]
    public bool Bit_7326
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7327)]
    public bool Bit_7327
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7328)]
    public bool Bit_7328
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7329)]
    public bool Bit_7329
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7330)]
    public bool Bit_7330
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7331)]
    public bool Bit_7331
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7332)]
    public bool Bit_7332
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7333)]
    public bool Bit_7333
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7334)]
    public bool Bit_7334
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7335)]
    public bool Bit_7335
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7336)]
    public bool Bit_7336
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7337)]
    public bool Bit_7337
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7338)]
    public bool Bit_7338
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7339)]
    public bool Bit_7339
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7340)]
    public bool Bit_7340
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7341)]
    public bool Bit_7341
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7342)]
    public bool Bit_7342
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7343)]
    public bool Bit_7343
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7344)]
    public bool Bit_7344
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7345)]
    public bool Bit_7345
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7346)]
    public bool Bit_7346
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7347)]
    public bool Bit_7347
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7348)]
    public bool Bit_7348
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7349)]
    public bool Bit_7349
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7350)]
    public bool Bit_7350
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7351)]
    public bool Bit_7351
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7352)]
    public bool Bit_7352
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7353)]
    public bool Bit_7353
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7354)]
    public bool Bit_7354
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7355)]
    public bool Bit_7355
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7356)]
    public bool Bit_7356
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7357)]
    public bool Bit_7357
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7358)]
    public bool Bit_7358
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7359)]
    public bool Bit_7359
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7360)]
    public bool Bit_7360
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7361)]
    public bool Bit_7361
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7362)]
    public bool Bit_7362
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7363)]
    public bool Bit_7363
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7364)]
    public bool Bit_7364
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7365)]
    public bool Bit_7365
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7366)]
    public bool Bit_7366
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7367)]
    public bool Bit_7367
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7371)]
    public bool Bit_7371
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7372)]
    public bool Bit_7372
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7373)]
    public bool Bit_7373
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7374)]
    public bool Bit_7374
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7375)]
    public bool Bit_7375
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7380)]
    public bool Bit_7380
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7381)]
    public bool Bit_7381
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7382)]
    public bool Bit_7382
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7383)]
    public bool Bit_7383
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7384)]
    public bool Bit_7384
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7385)]
    public bool Bit_7385
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7386)]
    public bool Bit_7386
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7387)]
    public bool Bit_7387
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7388)]
    public bool Bit_7388
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7389)]
    public bool Bit_7389
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7390)]
    public bool Bit_7390
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7391)]
    public bool Bit_7391
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7395)]
    public bool Bit_7395
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7396)]
    public bool Bit_7396
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7397)]
    public bool Bit_7397
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7398)]
    public bool Bit_7398
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7399)]
    public bool Bit_7399
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7402)]
    public bool Bit_7402
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7403)]
    public bool Bit_7403
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7404)]
    public bool Bit_7404
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7405)]
    public bool Bit_7405
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7406)]
    public bool Bit_7406
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7407)]
    public bool Bit_7407
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7409)]
    public bool Bit_7409
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7410)]
    public bool Bit_7410
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7411)]
    public bool Bit_7411
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7412)]
    public bool Bit_7412
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7413)]
    public bool Bit_7413
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7414)]
    public bool Bit_7414
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7415)]
    public bool Bit_7415
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7418)]
    public bool Bit_7418
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7419)]
    public bool Bit_7419
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7420)]
    public bool Bit_7420
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7421)]
    public bool Bit_7421
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7422)]
    public bool Bit_7422
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7423)]
    public bool Bit_7423
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7424)]
    public bool Bit_7424
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7425)]
    public bool Bit_7425
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7426)]
    public bool Bit_7426
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7427)]
    public bool Bit_7427
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7428)]
    public bool Bit_7428
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7429)]
    public bool Bit_7429
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7430)]
    public bool Bit_7430
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7431)]
    public bool Bit_7431
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7432)]
    public bool Bit_7432
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7439)]
    public bool Bit_7439
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7440)]
    public bool Bit_7440
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7441)]
    public bool Bit_7441
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7442)]
    public bool Bit_7442
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7443)]
    public bool Bit_7443
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7444)]
    public bool Bit_7444
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7445)]
    public bool Bit_7445
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7446)]
    public bool Bit_7446
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7447)]
    public bool Bit_7447
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7448)]
    public bool Bit_7448
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7449)]
    public bool Bit_7449
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7450)]
    public bool Bit_7450
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7451)]
    public bool Bit_7451
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7452)]
    public bool Bit_7452
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7453)]
    public bool Bit_7453
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7454)]
    public bool Bit_7454
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7455)]
    public bool Bit_7455
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7456)]
    public bool Bit_7456
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7457)]
    public bool Bit_7457
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7458)]
    public bool Bit_7458
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7459)]
    public bool Bit_7459
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7460)]
    public bool Bit_7460
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7461)]
    public bool Bit_7461
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7462)]
    public bool Bit_7462
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7463)]
    public bool Bit_7463
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7464)]
    public bool Bit_7464
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7467)]
    public bool Bit_7467
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7468)]
    public bool Bit_7468
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7469)]
    public bool Bit_7469
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7470)]
    public bool Bit_7470
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7471)]
    public bool Bit_7471
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7472)]
    public bool Bit_7472
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7473)]
    public bool Bit_7473
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7474)]
    public bool Bit_7474
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7475)]
    public bool Bit_7475
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7476)]
    public bool Bit_7476
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7477)]
    public bool Bit_7477
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7478)]
    public bool Bit_7478
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7479)]
    public bool Bit_7479
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7488)]
    public bool Bit_7488
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7489)]
    public bool Bit_7489
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7490)]
    public bool Bit_7490
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7491)]
    public bool Bit_7491
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7492)]
    public bool Bit_7492
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7493)]
    public bool Bit_7493
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7494)]
    public bool Bit_7494
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7495)]
    public bool Bit_7495
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7496)]
    public bool Bit_7496
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7497)]
    public bool Bit_7497
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7498)]
    public bool Bit_7498
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7499)]
    public bool Bit_7499
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7500)]
    public bool Bit_7500
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7501)]
    public bool Bit_7501
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7502)]
    public bool Bit_7502
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7503)]
    public bool Bit_7503
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7507)]
    public bool Bit_7507
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7508)]
    public bool Bit_7508
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7509)]
    public bool Bit_7509
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7510)]
    public bool Bit_7510
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7511)]
    public bool Bit_7511
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7512)]
    public bool Bit_7512
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7513)]
    public bool Bit_7513
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7514)]
    public bool Bit_7514
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7515)]
    public bool Bit_7515
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7516)]
    public bool Bit_7516
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7517)]
    public bool Bit_7517
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7518)]
    public bool Bit_7518
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7519)]
    public bool Bit_7519
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7520)]
    public bool Bit_7520
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7521)]
    public bool Bit_7521
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7522)]
    public bool Bit_7522
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7523)]
    public bool Bit_7523
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7524)]
    public bool Bit_7524
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7525)]
    public bool Bit_7525
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7526)]
    public bool Bit_7526
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7527)]
    public bool Bit_7527
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7534)]
    public bool Bit_7534
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7535)]
    public bool Bit_7535
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7539)]
    public bool Bit_7539
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7540)]
    public bool Bit_7540
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7541)]
    public bool Bit_7541
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7542)]
    public bool Bit_7542
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7543)]
    public bool Bit_7543
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7544)]
    public bool Bit_7544
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7545)]
    public bool Bit_7545
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7546)]
    public bool Bit_7546
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7547)]
    public bool Bit_7547
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7548)]
    public bool Bit_7548
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7549)]
    public bool Bit_7549
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7550)]
    public bool Bit_7550
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7551)]
    public bool Bit_7551
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7552)]
    public bool Bit_7552
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7553)]
    public bool Bit_7553
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7554)]
    public bool Bit_7554
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7555)]
    public bool Bit_7555
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7556)]
    public bool Bit_7556
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7557)]
    public bool Bit_7557
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7558)]
    public bool Bit_7558
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7559)]
    public bool Bit_7559
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7563)]
    public bool Bit_7563
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7564)]
    public bool Bit_7564
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7565)]
    public bool Bit_7565
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7566)]
    public bool Bit_7566
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7567)]
    public bool Bit_7567
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7568)]
    public bool Bit_7568
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7570)]
    public bool Bit_7570
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7571)]
    public bool Bit_7571
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7572)]
    public bool Bit_7572
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7573)]
    public bool Bit_7573
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7574)]
    public bool Bit_7574
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7575)]
    public bool Bit_7575
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7576)]
    public bool Bit_7576
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7577)]
    public bool Bit_7577
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7582)]
    public bool Bit_7582
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7583)]
    public bool Bit_7583
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7584)]
    public bool Bit_7584
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7585)]
    public bool Bit_7585
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7586)]
    public bool Bit_7586
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7587)]
    public bool Bit_7587
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7588)]
    public bool Bit_7588
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7589)]
    public bool Bit_7589
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7590)]
    public bool Bit_7590
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7591)]
    public bool Bit_7591
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7592)]
    public bool Bit_7592
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7593)]
    public bool Bit_7593
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7594)]
    public bool Bit_7594
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7595)]
    public bool Bit_7595
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7596)]
    public bool Bit_7596
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7597)]
    public bool Bit_7597
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7598)]
    public bool Bit_7598
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7599)]
    public bool Bit_7599
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7603)]
    public bool Bit_7603
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7604)]
    public bool Bit_7604
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7605)]
    public bool Bit_7605
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7606)]
    public bool Bit_7606
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7607)]
    public bool Bit_7607
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7616)]
    public bool Bit_7616
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7617)]
    public bool Bit_7617
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7618)]
    public bool Bit_7618
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7619)]
    public bool Bit_7619
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7620)]
    public bool Bit_7620
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7621)]
    public bool Bit_7621
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7622)]
    public bool Bit_7622
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7623)]
    public bool Bit_7623
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7627)]
    public bool Bit_7627
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7628)]
    public bool Bit_7628
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7629)]
    public bool Bit_7629
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7630)]
    public bool Bit_7630
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7631)]
    public bool Bit_7631
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7648)]
    public bool Bit_7648
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7649)]
    public bool Bit_7649
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7650)]
    public bool Bit_7650
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7651)]
    public bool Bit_7651
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7652)]
    public bool Bit_7652
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7653)]
    public bool Bit_7653
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7654)]
    public bool Bit_7654
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7655)]
    public bool Bit_7655
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7657)]
    public bool Bit_7657
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7658)]
    public bool Bit_7658
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7659)]
    public bool Bit_7659
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7660)]
    public bool Bit_7660
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7661)]
    public bool Bit_7661
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7662)]
    public bool Bit_7662
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7663)]
    public bool Bit_7663
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7681)]
    public bool Bit_7681
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7682)]
    public bool Bit_7682
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7683)]
    public bool Bit_7683
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7684)]
    public bool Bit_7684
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7685)]
    public bool Bit_7685
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7686)]
    public bool Bit_7686
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7687)]
    public bool Bit_7687
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7688)]
    public bool Bit_7688
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7689)]
    public bool Bit_7689
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7690)]
    public bool Bit_7690
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7691)]
    public bool Bit_7691
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7692)]
    public bool Bit_7692
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7693)]
    public bool Bit_7693
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7694)]
    public bool Bit_7694
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7695)]
    public bool Bit_7695
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7696)]
    public bool Bit_7696
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7697)]
    public bool Bit_7697
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7698)]
    public bool Bit_7698
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7699)]
    public bool Bit_7699
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7700)]
    public bool Bit_7700
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7701)]
    public bool Bit_7701
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7702)]
    public bool Bit_7702
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7703)]
    public bool Bit_7703
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7704)]
    public bool Bit_7704
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7705)]
    public bool Bit_7705
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7706)]
    public bool Bit_7706
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7707)]
    public bool Bit_7707
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7708)]
    public bool Bit_7708
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7709)]
    public bool Bit_7709
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7710)]
    public bool Bit_7710
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7711)]
    public bool Bit_7711
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7712)]
    public bool Bit_7712
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7713)]
    public bool Bit_7713
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7714)]
    public bool Bit_7714
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7715)]
    public bool Bit_7715
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7716)]
    public bool Bit_7716
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7717)]
    public bool Bit_7717
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7718)]
    public bool Bit_7718
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7719)]
    public bool Bit_7719
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7727)]
    public bool Bit_7727
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7730)]
    public bool Bit_7730
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7731)]
    public bool Bit_7731
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7732)]
    public bool Bit_7732
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7733)]
    public bool Bit_7733
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7734)]
    public bool Bit_7734
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7735)]
    public bool Bit_7735
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7742)]
    public bool Bit_7742
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7743)]
    public bool Bit_7743
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7744)]
    public bool Bit_7744
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7745)]
    public bool Bit_7745
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7746)]
    public bool Bit_7746
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7747)]
    public bool Bit_7747
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7748)]
    public bool Bit_7748
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7749)]
    public bool Bit_7749
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7750)]
    public bool Bit_7750
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7751)]
    public bool Bit_7751
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7759)]
    public bool Bit_7759
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7767)]
    public bool Bit_7767
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7775)]
    public bool Bit_7775
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8376)]
    public bool Bit_8376
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8377)]
    public bool Bit_8377
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8378)]
    public bool Bit_8378
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8379)]
    public bool Bit_8379
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8380)]
    public bool Bit_8380
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8381)]
    public bool Bit_8381
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8382)]
    public bool Bit_8382
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8383)]
    public bool Bit_8383
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8384)]
    public bool Bit_8384
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8385)]
    public bool Bit_8385
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8386)]
    public bool Bit_8386
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8387)]
    public bool Bit_8387
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8388)]
    public bool Bit_8388
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8389)]
    public bool Bit_8389
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8390)]
    public bool Bit_8390
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8391)]
    public bool Bit_8391
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8392)]
    public bool Bit_8392
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8393)]
    public bool Bit_8393
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8394)]
    public bool Bit_8394
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8395)]
    public bool Bit_8395
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8396)]
    public bool Bit_8396
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8397)]
    public bool Bit_8397
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8398)]
    public bool Bit_8398
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8399)]
    public bool Bit_8399
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8400)]
    public bool Bit_8400
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8401)]
    public bool Bit_8401
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8402)]
    public bool Bit_8402
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8403)]
    public bool Bit_8403
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8404)]
    public bool Bit_8404
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8405)]
    public bool Bit_8405
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8406)]
    public bool Bit_8406
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8407)]
    public bool Bit_8407
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8408)]
    public bool Bit_8408
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8409)]
    public bool Bit_8409
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8410)]
    public bool Bit_8410
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8411)]
    public bool Bit_8411
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8412)]
    public bool Bit_8412
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8413)]
    public bool Bit_8413
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8414)]
    public bool Bit_8414
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8415)]
    public bool Bit_8415
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8416)]
    public bool Bit_8416
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8417)]
    public bool Bit_8417
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8418)]
    public bool Bit_8418
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8419)]
    public bool Bit_8419
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8420)]
    public bool Bit_8420
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8421)]
    public bool Bit_8421
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8422)]
    public bool Bit_8422
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8423)]
    public bool Bit_8423
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8424)]
    public bool Bit_8424
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8425)]
    public bool Bit_8425
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8426)]
    public bool Bit_8426
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8427)]
    public bool Bit_8427
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8428)]
    public bool Bit_8428
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8429)]
    public bool Bit_8429
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8430)]
    public bool Bit_8430
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8431)]
    public bool Bit_8431
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8432)]
    public bool Bit_8432
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8433)]
    public bool Bit_8433
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8434)]
    public bool Bit_8434
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8435)]
    public bool Bit_8435
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8436)]
    public bool Bit_8436
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8437)]
    public bool Bit_8437
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8438)]
    public bool Bit_8438
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8439)]
    public bool Bit_8439
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8440)]
    public bool Bit_8440
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8441)]
    public bool Bit_8441
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8442)]
    public bool Bit_8442
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8443)]
    public bool Bit_8443
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8444)]
    public bool Bit_8444
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8445)]
    public bool Bit_8445
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8446)]
    public bool Bit_8446
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8447)]
    public bool Bit_8447
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8448)]
    public bool Bit_8448
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8449)]
    public bool Bit_8449
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8450)]
    public bool Bit_8450
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8451)]
    public bool Bit_8451
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8452)]
    public bool Bit_8452
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8453)]
    public bool Bit_8453
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8454)]
    public bool Bit_8454
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8455)]
    public bool Bit_8455
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8456)]
    public bool Bit_8456
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8457)]
    public bool Bit_8457
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8458)]
    public bool Bit_8458
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8459)]
    public bool Bit_8459
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8460)]
    public bool Bit_8460
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8461)]
    public bool Bit_8461
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8462)]
    public bool Bit_8462
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8463)]
    public bool Bit_8463
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8464)]
    public bool Bit_8464
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8465)]
    public bool Bit_8465
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8466)]
    public bool Bit_8466
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8467)]
    public bool Bit_8467
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8468)]
    public bool Bit_8468
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8469)]
    public bool Bit_8469
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8470)]
    public bool Bit_8470
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8471)]
    public bool Bit_8471
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8472)]
    public bool Bit_8472
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8473)]
    public bool Bit_8473
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8474)]
    public bool Bit_8474
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8475)]
    public bool Bit_8475
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8476)]
    public bool Bit_8476
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8477)]
    public bool Bit_8477
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8478)]
    public bool Bit_8478
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8479)]
    public bool Bit_8479
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8480)]
    public bool Bit_8480
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8481)]
    public bool Bit_8481
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8482)]
    public bool Bit_8482
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8483)]
    public bool Bit_8483
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8484)]
    public bool Bit_8484
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8485)]
    public bool Bit_8485
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8486)]
    public bool Bit_8486
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8487)]
    public bool Bit_8487
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8488)]
    public bool Bit_8488
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8489)]
    public bool Bit_8489
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8490)]
    public bool Bit_8490
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8491)]
    public bool Bit_8491
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8492)]
    public bool Bit_8492
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8493)]
    public bool Bit_8493
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8494)]
    public bool Bit_8494
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8495)]
    public bool Bit_8495
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8496)]
    public bool Bit_8496
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8497)]
    public bool Bit_8497
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8498)]
    public bool Bit_8498
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8499)]
    public bool Bit_8499
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8500)]
    public bool Bit_8500
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8501)]
    public bool Bit_8501
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8502)]
    public bool Bit_8502
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8503)]
    public bool Bit_8503
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8510)]
    public bool Bit_8510
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8511)]
    public bool Bit_8511
    {
        get => ScriptMemory.ReadView(typeof(GlobalState)) != 0;
        set => ScriptMemory.WriteView(typeof(GlobalState), value ? 1 : 0);
    }

}

