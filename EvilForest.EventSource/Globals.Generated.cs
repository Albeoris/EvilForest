namespace EvilForest.EventSource;

public static class Globals
{
    [StorageView(StorageScope.Global, StorageType.UInt16, 0)]
    public static int ScenarioCounter
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.UInt16, 0);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.UInt16, 0, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 2)]
    public static int FieldEntrance
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int16, 2);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int16, 2, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 4)]
    public static int Byte_4
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 4);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 4, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 5)]
    public static int Byte_5
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 5);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 5, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 6)]
    public static int Byte_6
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 6);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 6, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 7)]
    public static int Byte_7
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 7);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 7, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 8)]
    public static int Byte_8
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 8);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 8, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 9)]
    public static int EnvironmentSoundSlot0Id
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int16, 9);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int16, 9, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 11)]
    public static int EnvironmentSoundSlot1Id
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int16, 11);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int16, 11, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 13)]
    public static int EnvironmentSoundSlot0State
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 13);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 13, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 14)]
    public static int EnvironmentSoundSlot1State
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 14);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 14, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 16)]
    public static int TranceGaugeFlag
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 16);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 16, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 17)]
    public static int GarnetDepressionFlag
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 17);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 17, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 18)]
    public static int Byte_18
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 18);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 18, value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 19)]
    public static int UInt16_19
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.UInt16, 19);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.UInt16, 19, value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 21)]
    public static int UInt16_21
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.UInt16, 21);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.UInt16, 21, value);
    }

    [StorageView(StorageScope.Global, StorageType.SByte, 28)]
    public static int SByte_28
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.SByte, 28);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.SByte, 28, value);
    }

    [StorageView(StorageScope.Global, StorageType.SByte, 60)]
    public static int SByte_60
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.SByte, 60);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.SByte, 60, value);
    }

    [StorageView(StorageScope.Global, StorageType.SByte, 61)]
    public static int SByte_61
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.SByte, 61);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.SByte, 61, value);
    }

    [StorageView(StorageScope.Global, StorageType.SByte, 62)]
    public static int SByte_62
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.SByte, 62);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.SByte, 62, value);
    }

    [StorageView(StorageScope.Global, StorageType.SByte, 63)]
    public static int SByte_63
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.SByte, 63);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.SByte, 63, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 64)]
    public static int Int24_64
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int24, 64);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int24, 64, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 67)]
    public static int Int16_67
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int16, 67);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int16, 67, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 69)]
    public static int Int24_69
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int24, 69);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int24, 69, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 72)]
    public static int Byte_72
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 72);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 72, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 74)]
    public static int Int24_74
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int24, 74);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int24, 74, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 77)]
    public static int Int16_77
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int16, 77);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int16, 77, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 79)]
    public static int Int24_79
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int24, 79);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int24, 79, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 82)]
    public static int Byte_82
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 82);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 82, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 83)]
    public static int Int24_83
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int24, 83);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int24, 83, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 86)]
    public static int Int16_86
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int16, 86);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int16, 86, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 88)]
    public static int Int24_88
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int24, 88);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int24, 88, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 91)]
    public static int Byte_91
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 91);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 91, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 104)]
    public static int Byte_104
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 104);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 104, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 112)]
    public static int Int24_112
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int24, 112);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int24, 112, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 115)]
    public static int Int24_115
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int24, 115);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int24, 115, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 118)]
    public static int Int16_118
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int16, 118);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int16, 118, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 129)]
    public static int Byte_129
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 129);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 129, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 131)]
    public static int Byte_131
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 131);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 131, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 132)]
    public static int Byte_132
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 132);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 132, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 134)]
    public static int Byte_134
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 134);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 134, value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 136)]
    public static int UInt16_136
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.UInt16, 136);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.UInt16, 136, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 138)]
    public static int Byte_138
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 138);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 138, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 139)]
    public static int Byte_139
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 139);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 139, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 140)]
    public static int Int16_140
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int16, 140);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int16, 140, value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 142)]
    public static int UInt16_142
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.UInt16, 142);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.UInt16, 142, value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 146)]
    public static int UInt16_146
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.UInt16, 146);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.UInt16, 146, value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 148)]
    public static int UInt16_148
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.UInt16, 148);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.UInt16, 148, value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 150)]
    public static int UInt16_150
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.UInt16, 150);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.UInt16, 150, value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 152)]
    public static int UInt16_152
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.UInt16, 152);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.UInt16, 152, value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 154)]
    public static int UInt16_154
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.UInt16, 154);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.UInt16, 154, value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 156)]
    public static int UInt16_156
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.UInt16, 156);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.UInt16, 156, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 158)]
    public static int Byte_158
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 158);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 158, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 159)]
    public static int Byte_159
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 159);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 159, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 160)]
    public static int Byte_160
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 160);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 160, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 161)]
    public static int Byte_161
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 161);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 161, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 162)]
    public static int Byte_162
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 162);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 162, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 163)]
    public static int Byte_163
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 163);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 163, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 164)]
    public static int Byte_164
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 164);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 164, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 165)]
    public static int Byte_165
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 165);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 165, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 166)]
    public static int Byte_166
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 166);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 166, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 167)]
    public static int Byte_167
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 167);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 167, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 168)]
    public static int Byte_168
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 168);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 168, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 171)]
    public static int Byte_171
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 171);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 171, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 172)]
    public static int Int24_172
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int24, 172);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int24, 172, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 175)]
    public static int Int16_175
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int16, 175);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int16, 175, value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 178)]
    public static int UInt16_178
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.UInt16, 178);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.UInt16, 178, value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 182)]
    public static int UInt16_182
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.UInt16, 182);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.UInt16, 182, value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 184)]
    public static bool Bit_184
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 184) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 184, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 184)]
    public static int Int24_184
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int24, 184);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int24, 184, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 187)]
    public static int Int24_187
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int24, 187);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int24, 187, value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 189)]
    public static bool Bit_189
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 189) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 189, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 190)]
    public static int Byte_190
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 190);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 190, value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 191)]
    public static bool Bit_191
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 191) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 191, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 191)]
    public static int Byte_191
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 191);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 191, value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 196)]
    public static bool Bit_196
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 196) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 196, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 197)]
    public static bool Bit_197
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 197) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 197, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 198)]
    public static bool Bit_198
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 198) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 198, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 198)]
    public static int Byte_198
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 198);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 198, value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 199)]
    public static bool Bit_199
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 199) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 199, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 200)]
    public static int Byte_200
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 200);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 200, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 208)]
    public static int Byte_208
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 208);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 208, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 209)]
    public static int Byte_209
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 209);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 209, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 210)]
    public static int Byte_210
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 210);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 210, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 211)]
    public static int Byte_211
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 211);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 211, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 212)]
    public static int Byte_212
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 212);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 212, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 213)]
    public static int Byte_213
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 213);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 213, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 214)]
    public static int Int16_214
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int16, 214);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int16, 214, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 216)]
    public static int Int16_216
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int16, 216);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int16, 216, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 218)]
    public static int Int16_218
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int16, 218);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int16, 218, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 220)]
    public static int Int16_220
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int16, 220);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int16, 220, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 222)]
    public static int Int16_222
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int16, 222);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int16, 222, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 224)]
    public static int Int16_224
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int16, 224);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int16, 224, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 226)]
    public static int Byte_226
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 226);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 226, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 228)]
    public static int Int16_228
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int16, 228);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int16, 228, value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 234)]
    public static int UInt16_234
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.UInt16, 234);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.UInt16, 234, value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 236)]
    public static int UInt16_236
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.UInt16, 236);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.UInt16, 236, value);
    }

    [StorageView(StorageScope.Global, StorageType.SByte, 238)]
    public static int SByte_238
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.SByte, 238);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.SByte, 238, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 239)]
    public static int Int16_239
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int16, 239);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int16, 239, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 241)]
    public static int Int16_241
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int16, 241);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int16, 241, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 243)]
    public static int Int16_243
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int16, 243);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int16, 243, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 245)]
    public static int Int16_245
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int16, 245);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int16, 245, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 247)]
    public static int Int16_247
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int16, 247);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int16, 247, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 249)]
    public static int Byte_249
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 249);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 249, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 250)]
    public static int Byte_250
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 250);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 250, value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 251)]
    public static int UInt16_251
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.UInt16, 251);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.UInt16, 251, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 256)]
    public static int Byte_256
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 256);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 256, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 257)]
    public static int Byte_257
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 257);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 257, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 258)]
    public static int Byte_258
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 258);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 258, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 259)]
    public static int Byte_259
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 259);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 259, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 260)]
    public static int Byte_260
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 260);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 260, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 263)]
    public static int Byte_263
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 263);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 263, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 264)]
    public static int Byte_264
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 264);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 264, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 265)]
    public static int Byte_265
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 265);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 265, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 266)]
    public static int Byte_266
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 266);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 266, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 267)]
    public static int Int16_267
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int16, 267);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int16, 267, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 269)]
    public static int Int16_269
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int16, 269);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int16, 269, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 271)]
    public static int Int16_271
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int16, 271);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int16, 271, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 273)]
    public static int Int16_273
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int16, 273);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int16, 273, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 275)]
    public static int Int16_275
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int16, 275);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int16, 275, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 277)]
    public static int Int16_277
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int16, 277);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int16, 277, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 279)]
    public static int Byte_279
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 279);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 279, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 280)]
    public static int Byte_280
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 280);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 280, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 281)]
    public static int Byte_281
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 281);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 281, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 282)]
    public static int Byte_282
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 282);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 282, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 283)]
    public static int Byte_283
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 283);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 283, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 284)]
    public static int Byte_284
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 284);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 284, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 285)]
    public static int Byte_285
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 285);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 285, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 286)]
    public static int Byte_286
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 286);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 286, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 287)]
    public static int Byte_287
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 287);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 287, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 288)]
    public static int Byte_288
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 288);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 288, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 289)]
    public static int Byte_289
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 289);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 289, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 290)]
    public static int Byte_290
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 290);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 290, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 291)]
    public static int Byte_291
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 291);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 291, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 292)]
    public static int Byte_292
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 292);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 292, value);
    }

    [StorageView(StorageScope.Global, StorageType.SByte, 293)]
    public static int SByte_293
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.SByte, 293);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.SByte, 293, value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 294)]
    public static int UInt16_294
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.UInt16, 294);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.UInt16, 294, value);
    }

    [StorageView(StorageScope.Global, StorageType.SByte, 296)]
    public static int SByte_296
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.SByte, 296);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.SByte, 296, value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 297)]
    public static int UInt16_297
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.UInt16, 297);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.UInt16, 297, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 299)]
    public static int Byte_299
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 299);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 299, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 303)]
    public static int Byte_303
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 303);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 303, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 312)]
    public static int Byte_312
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 312);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 312, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 313)]
    public static int Byte_313
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 313);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 313, value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 314)]
    public static int UInt16_314
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.UInt16, 314);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.UInt16, 314, value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 316)]
    public static int UInt16_316
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.UInt16, 316);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.UInt16, 316, value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 318)]
    public static int UInt16_318
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.UInt16, 318);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.UInt16, 318, value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 320)]
    public static int UInt16_320
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.UInt16, 320);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.UInt16, 320, value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 322)]
    public static int UInt16_322
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.UInt16, 322);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.UInt16, 322, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 329)]
    public static int Byte_329
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 329);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 329, value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 333)]
    public static int UInt16_333
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.UInt16, 333);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.UInt16, 333, value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 335)]
    public static int UInt16_335
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.UInt16, 335);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.UInt16, 335, value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 355)]
    public static int UInt16_355
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.UInt16, 355);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.UInt16, 355, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 358)]
    public static int Byte_358
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 358);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 358, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 361)]
    public static int Byte_361
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 361);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 361, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 362)]
    public static int Byte_362
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 362);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 362, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 363)]
    public static int Byte_363
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 363);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 363, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 369)]
    public static int Byte_369
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 369);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 369, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 370)]
    public static int Byte_370
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 370);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 370, value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 396)]
    public static int UInt16_396
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.UInt16, 396);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.UInt16, 396, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 398)]
    public static int Byte_398
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 398);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 398, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 399)]
    public static int Byte_399
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 399);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 399, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 400)]
    public static int Byte_400
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 400);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 400, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 401)]
    public static int Byte_401
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 401);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 401, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 408)]
    public static int Byte_408
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 408);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 408, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 409)]
    public static int Byte_409
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 409);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 409, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 418)]
    public static int Int16_418
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int16, 418);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int16, 418, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 420)]
    public static int Int16_420
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int16, 420);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int16, 420, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 426)]
    public static int Int24_426
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int24, 426);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int24, 426, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 429)]
    public static int Int24_429
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int24, 429);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int24, 429, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 433)]
    public static int Byte_433
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 433);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 433, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 434)]
    public static int Byte_434
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 434);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 434, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 435)]
    public static int Int16_435
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int16, 435);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int16, 435, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 437)]
    public static int Int16_437
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int16, 437);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int16, 437, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 439)]
    public static int Int16_439
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int16, 439);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int16, 439, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 441)]
    public static int Byte_441
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 441);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 441, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 442)]
    public static int Byte_442
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 442);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 442, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 444)]
    public static int Byte_444
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 444);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 444, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 445)]
    public static int Int24_445
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int24, 445);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int24, 445, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 450)]
    public static int Byte_450
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 450);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 450, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 455)]
    public static int Byte_455
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 455);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 455, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 456)]
    public static int Int16_456
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int16, 456);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int16, 456, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 458)]
    public static int Byte_458
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 458);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 458, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 465)]
    public static int Byte_465
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 465);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 465, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 467)]
    public static int Byte_467
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 467);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 467, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 468)]
    public static int Byte_468
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 468);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 468, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int16, 469)]
    public static int Int16_469
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int16, 469);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int16, 469, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 471)]
    public static int Byte_471
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 471);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 471, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 472)]
    public static int Byte_472
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 472);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 472, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 475)]
    public static int Byte_475
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 475);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 475, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 479)]
    public static int Byte_479
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 479);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 479, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 483)]
    public static int Byte_483
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 483);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 483, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 484)]
    public static int Byte_484
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 484);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 484, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 489)]
    public static int Byte_489
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 489);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 489, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 490)]
    public static int Byte_490
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 490);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 490, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 491)]
    public static int Byte_491
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 491);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 491, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 492)]
    public static int Byte_492
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 492);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 492, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 496)]
    public static int Int24_496
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int24, 496);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int24, 496, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 499)]
    public static int Int24_499
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int24, 499);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int24, 499, value);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 502)]
    public static int Int24_502
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int24, 502);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int24, 502, value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 506)]
    public static int UInt16_506
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.UInt16, 506);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.UInt16, 506, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 508)]
    public static int Byte_508
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 508);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 508, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 509)]
    public static int Byte_509
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 509);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 509, value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 736)]
    public static bool Bit_736
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 736) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 736, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 737)]
    public static bool Bit_737
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 737) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 737, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 738)]
    public static bool Bit_738
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 738) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 738, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 739)]
    public static bool Bit_739
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 739) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 739, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 740)]
    public static bool Bit_740
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 740) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 740, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 741)]
    public static bool Bit_741
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 741) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 741, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 742)]
    public static bool Bit_742
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 742) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 742, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 743)]
    public static bool Bit_743
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 743) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 743, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 744)]
    public static bool Bit_744
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 744) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 744, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 745)]
    public static bool Bit_745
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 745) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 745, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 746)]
    public static bool Bit_746
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 746) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 746, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 747)]
    public static bool Bit_747
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 747) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 747, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 748)]
    public static bool Bit_748
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 748) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 748, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 749)]
    public static bool Bit_749
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 749) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 749, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 750)]
    public static bool Bit_750
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 750) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 750, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 751)]
    public static bool Bit_751
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 751) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 751, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 752)]
    public static bool Bit_752
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 752) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 752, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 753)]
    public static bool Bit_753
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 753) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 753, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 754)]
    public static bool Bit_754
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 754) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 754, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 755)]
    public static bool Bit_755
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 755) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 755, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 756)]
    public static bool Bit_756
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 756) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 756, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 757)]
    public static bool Bit_757
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 757) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 757, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 758)]
    public static bool Bit_758
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 758) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 758, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 759)]
    public static bool Bit_759
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 759) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 759, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 760)]
    public static bool Bit_760
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 760) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 760, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 761)]
    public static bool Bit_761
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 761) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 761, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 763)]
    public static bool Bit_763
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 763) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 763, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 765)]
    public static bool Bit_765
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 765) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 765, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 766)]
    public static bool Bit_766
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 766) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 766, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 767)]
    public static bool Bit_767
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 767) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 767, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 768)]
    public static bool Bit_768
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 768) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 768, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 768)]
    public static int Int24_768
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int24, 768);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int24, 768, value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 769)]
    public static bool Bit_769
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 769) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 769, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 770)]
    public static bool Bit_770
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 770) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 770, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 771)]
    public static bool Bit_771
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 771) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 771, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 771)]
    public static int Int24_771
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int24, 771);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int24, 771, value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 772)]
    public static bool Bit_772
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 772) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 772, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 773)]
    public static bool Bit_773
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 773) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 773, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 774)]
    public static int Int24_774
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int24, 774);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int24, 774, value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 775)]
    public static bool Bit_775
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 775) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 775, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 776)]
    public static bool Bit_776
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 776) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 776, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 777)]
    public static bool Bit_777
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 777) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 777, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Int24, 777)]
    public static int Int24_777
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Int24, 777);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Int24, 777, value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 780)]
    public static bool Bit_780
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 780) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 780, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 780)]
    public static int Byte_780
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 780);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 780, value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 781)]
    public static bool Bit_781
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 781) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 781, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 781)]
    public static int Byte_781
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 781);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 781, value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 782)]
    public static bool Bit_782
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 782) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 782, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 782)]
    public static int Byte_782
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 782);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 782, value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 783)]
    public static bool Bit_783
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 783) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 783, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 783)]
    public static int Byte_783
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 783);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 783, value);
    }

    [StorageView(StorageScope.Global, StorageType.SByte, 784)]
    public static int SByte_784
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.SByte, 784);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.SByte, 784, value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 785)]
    public static bool Bit_785
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 785) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 785, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 785)]
    public static int Byte_785
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 785);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 785, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 786)]
    public static int Byte_786
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 786);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 786, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 787)]
    public static int Byte_787
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 787);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 787, value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 788)]
    public static bool Bit_788
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 788) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 788, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 788)]
    public static int Byte_788
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 788);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 788, value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 789)]
    public static bool Bit_789
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 789) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 789, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 789)]
    public static int Byte_789
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 789);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 789, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 790)]
    public static int Byte_790
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 790);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 790, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 791)]
    public static int Byte_791
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 791);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 791, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 792)]
    public static int Byte_792
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 792);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 792, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 793)]
    public static int Byte_793
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 793);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 793, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 794)]
    public static int Byte_794
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 794);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 794, value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 795)]
    public static bool Bit_795
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 795) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 795, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 795)]
    public static int Byte_795
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 795);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 795, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 796)]
    public static int Byte_796
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 796);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 796, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 797)]
    public static int Byte_797
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 797);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 797, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 798)]
    public static int Byte_798
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 798);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 798, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 799)]
    public static int Byte_799
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 799);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 799, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 800)]
    public static int Byte_800
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 800);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 800, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 801)]
    public static int Byte_801
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 801);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 801, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 802)]
    public static int Byte_802
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 802);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 802, value);
    }

    [StorageView(StorageScope.Global, StorageType.UInt16, 803)]
    public static int UInt16_803
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.UInt16, 803);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.UInt16, 803, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 805)]
    public static int Byte_805
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 805);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 805, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 806)]
    public static int Byte_806
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 806);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 806, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 807)]
    public static int Byte_807
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 807);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 807, value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 808)]
    public static bool Bit_808
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 808) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 808, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 808)]
    public static int Byte_808
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 808);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 808, value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 809)]
    public static bool Bit_809
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 809) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 809, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 809)]
    public static int Byte_809
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 809);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 809, value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 810)]
    public static bool Bit_810
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 810) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 810, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 810)]
    public static int Byte_810
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 810);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 810, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 811)]
    public static int Byte_811
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 811);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 811, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 812)]
    public static int Byte_812
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 812);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 812, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 813)]
    public static int Byte_813
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 813);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 813, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 814)]
    public static int Byte_814
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 814);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 814, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 815)]
    public static int Byte_815
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 815);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 815, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 816)]
    public static int Byte_816
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 816);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 816, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 817)]
    public static int Byte_817
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 817);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 817, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 818)]
    public static int Byte_818
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 818);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 818, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 819)]
    public static int Byte_819
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 819);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 819, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 820)]
    public static int Byte_820
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 820);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 820, value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 848)]
    public static bool Bit_848
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 848) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 848, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 849)]
    public static bool Bit_849
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 849) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 849, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 852)]
    public static bool Bit_852
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 852) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 852, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 853)]
    public static bool Bit_853
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 853) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 853, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 888)]
    public static bool Bit_888
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 888) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 888, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 889)]
    public static bool Bit_889
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 889) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 889, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 890)]
    public static bool Bit_890
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 890) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 890, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 891)]
    public static bool Bit_891
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 891) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 891, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 892)]
    public static bool Bit_892
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 892) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 892, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 893)]
    public static bool Bit_893
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 893) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 893, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 894)]
    public static bool Bit_894
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 894) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 894, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 895)]
    public static bool Bit_895
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 895) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 895, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 896)]
    public static int Byte_896
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 896);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 896, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 897)]
    public static int Byte_897
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 897);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 897, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 898)]
    public static int Byte_898
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 898);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 898, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 899)]
    public static int Byte_899
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 899);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 899, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 900)]
    public static int Byte_900
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 900);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 900, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 901)]
    public static int Byte_901
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 901);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 901, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 902)]
    public static int Byte_902
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 902);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 902, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 903)]
    public static int Byte_903
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 903);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 903, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 904)]
    public static int Byte_904
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 904);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 904, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 905)]
    public static int Byte_905
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 905);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 905, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 906)]
    public static int Byte_906
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 906);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 906, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 907)]
    public static int Byte_907
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 907);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 907, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 908)]
    public static int Byte_908
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 908);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 908, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 909)]
    public static int Byte_909
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 909);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 909, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 910)]
    public static int Byte_910
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 910);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 910, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 911)]
    public static int Byte_911
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 911);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 911, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 912)]
    public static int Byte_912
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 912);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 912, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 913)]
    public static int Byte_913
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 913);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 913, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 914)]
    public static int Byte_914
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 914);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 914, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 915)]
    public static int Byte_915
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 915);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 915, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 916)]
    public static int Byte_916
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 916);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 916, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 917)]
    public static int Byte_917
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 917);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 917, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 918)]
    public static int Byte_918
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 918);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 918, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 919)]
    public static int Byte_919
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 919);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 919, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 920)]
    public static int Byte_920
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 920);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 920, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 921)]
    public static int Byte_921
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 921);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 921, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 922)]
    public static int Byte_922
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 922);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 922, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 923)]
    public static int Byte_923
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 923);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 923, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 924)]
    public static int Byte_924
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 924);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 924, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 925)]
    public static int Byte_925
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 925);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 925, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 926)]
    public static int Byte_926
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 926);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 926, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 927)]
    public static int Byte_927
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 927);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 927, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 928)]
    public static int Byte_928
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 928);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 928, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 929)]
    public static int Byte_929
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 929);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 929, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 930)]
    public static int Byte_930
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 930);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 930, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 931)]
    public static int Byte_931
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 931);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 931, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 932)]
    public static int Byte_932
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 932);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 932, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 933)]
    public static int Byte_933
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 933);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 933, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 934)]
    public static int Byte_934
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 934);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 934, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 935)]
    public static int Byte_935
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 935);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 935, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 936)]
    public static int Byte_936
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 936);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 936, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 937)]
    public static int Byte_937
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 937);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 937, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 938)]
    public static int Byte_938
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 938);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 938, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 939)]
    public static int Byte_939
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 939);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 939, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 940)]
    public static int Byte_940
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 940);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 940, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 941)]
    public static int Byte_941
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 941);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 941, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 942)]
    public static int Byte_942
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 942);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 942, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 943)]
    public static int Byte_943
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 943);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 943, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 944)]
    public static int Byte_944
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 944);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 944, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 945)]
    public static int Byte_945
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 945);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 945, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 946)]
    public static int Byte_946
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 946);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 946, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 947)]
    public static int Byte_947
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 947);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 947, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 948)]
    public static int Byte_948
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 948);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 948, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 949)]
    public static int Byte_949
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 949);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 949, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 950)]
    public static int Byte_950
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 950);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 950, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 951)]
    public static int Byte_951
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 951);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 951, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 952)]
    public static int Byte_952
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 952);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 952, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 953)]
    public static int Byte_953
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 953);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 953, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 954)]
    public static int Byte_954
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 954);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 954, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 955)]
    public static int Byte_955
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 955);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 955, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 956)]
    public static int Byte_956
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 956);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 956, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 957)]
    public static int Byte_957
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 957);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 957, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 958)]
    public static int Byte_958
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 958);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 958, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 959)]
    public static int Byte_959
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 959);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 959, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 960)]
    public static int Byte_960
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 960);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 960, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 966)]
    public static int Byte_966
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 966);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 966, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 967)]
    public static int Byte_967
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 967);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 967, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 968)]
    public static int Byte_968
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 968);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 968, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 969)]
    public static int Byte_969
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 969);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 969, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 970)]
    public static int Byte_970
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 970);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 970, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 971)]
    public static int Byte_971
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 971);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 971, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 972)]
    public static int Byte_972
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 972);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 972, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 973)]
    public static int Byte_973
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 973);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 973, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 974)]
    public static int Byte_974
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 974);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 974, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 975)]
    public static int Byte_975
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 975);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 975, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1024)]
    public static int Byte_1024
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 1024);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 1024, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1032)]
    public static int Byte_1032
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 1032);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 1032, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1033)]
    public static int Byte_1033
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 1033);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 1033, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1034)]
    public static int Byte_1034
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 1034);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 1034, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1035)]
    public static int Byte_1035
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 1035);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 1035, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1036)]
    public static int Byte_1036
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 1036);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 1036, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1037)]
    public static int Byte_1037
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 1037);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 1037, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1038)]
    public static int Byte_1038
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 1038);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 1038, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1039)]
    public static int Byte_1039
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 1039);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 1039, value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1040)]
    public static bool Bit_1040
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 1040) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 1040, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1040)]
    public static int Byte_1040
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 1040);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 1040, value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1041)]
    public static bool Bit_1041
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 1041) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 1041, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1041)]
    public static int Byte_1041
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 1041);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 1041, value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1042)]
    public static bool Bit_1042
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 1042) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 1042, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1042)]
    public static int Byte_1042
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 1042);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 1042, value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1043)]
    public static bool Bit_1043
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 1043) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 1043, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1043)]
    public static int Byte_1043
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 1043);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 1043, value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1044)]
    public static bool Bit_1044
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 1044) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 1044, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1044)]
    public static int Byte_1044
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 1044);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 1044, value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1045)]
    public static bool Bit_1045
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 1045) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 1045, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1045)]
    public static int Byte_1045
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 1045);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 1045, value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1046)]
    public static bool Bit_1046
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 1046) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 1046, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1047)]
    public static bool Bit_1047
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 1047) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 1047, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1064)]
    public static bool Bit_1064
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 1064) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 1064, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1064)]
    public static int Byte_1064
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 1064);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 1064, value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1065)]
    public static bool Bit_1065
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 1065) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 1065, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1065)]
    public static int Byte_1065
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 1065);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 1065, value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1066)]
    public static bool Bit_1066
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 1066) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 1066, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1066)]
    public static int Byte_1066
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 1066);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 1066, value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1067)]
    public static bool Bit_1067
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 1067) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 1067, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1067)]
    public static int Byte_1067
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 1067);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 1067, value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1068)]
    public static bool Bit_1068
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 1068) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 1068, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1068)]
    public static int Byte_1068
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 1068);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 1068, value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1069)]
    public static bool Bit_1069
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 1069) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 1069, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1069)]
    public static int Byte_1069
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 1069);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 1069, value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1070)]
    public static bool Bit_1070
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 1070) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 1070, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1070)]
    public static int Byte_1070
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 1070);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 1070, value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1071)]
    public static bool Bit_1071
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 1071) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 1071, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1071)]
    public static int Byte_1071
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 1071);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 1071, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1072)]
    public static int Byte_1072
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 1072);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 1072, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1073)]
    public static int Byte_1073
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 1073);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 1073, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1079)]
    public static int Byte_1079
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 1079);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 1079, value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1080)]
    public static bool Bit_1080
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 1080) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 1080, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1080)]
    public static int Byte_1080
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 1080);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 1080, value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1081)]
    public static bool Bit_1081
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 1081) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 1081, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1081)]
    public static int Byte_1081
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 1081);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 1081, value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1082)]
    public static bool Bit_1082
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 1082) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 1082, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1082)]
    public static int Byte_1082
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 1082);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 1082, value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1083)]
    public static bool Bit_1083
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 1083) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 1083, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1083)]
    public static int Byte_1083
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 1083);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 1083, value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1084)]
    public static bool Bit_1084
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 1084) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 1084, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1084)]
    public static int Byte_1084
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 1084);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 1084, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1085)]
    public static int Byte_1085
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 1085);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 1085, value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1086)]
    public static bool Bit_1086
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 1086) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 1086, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1086)]
    public static int Byte_1086
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 1086);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 1086, value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1087)]
    public static bool Bit_1087
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 1087) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 1087, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1087)]
    public static int Byte_1087
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 1087);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 1087, value);
    }

    [StorageView(StorageScope.Global, StorageType.Byte, 1088)]
    public static int Byte_1088
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Byte, 1088);
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Byte, 1088, value);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1156)]
    public static bool Bit_1156
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 1156) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 1156, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1157)]
    public static bool Bit_1157
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 1157) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 1157, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1158)]
    public static bool Bit_1158
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 1158) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 1158, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1159)]
    public static bool Bit_1159
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 1159) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 1159, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1416)]
    public static bool Bit_1416
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 1416) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 1416, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1417)]
    public static bool Bit_1417
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 1417) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 1417, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1419)]
    public static bool Bit_1419
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 1419) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 1419, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1421)]
    public static bool Bit_1421
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 1421) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 1421, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1422)]
    public static bool Bit_1422
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 1422) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 1422, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1423)]
    public static bool Bit_1423
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 1423) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 1423, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1463)]
    public static bool Bit_1463
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 1463) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 1463, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1584)]
    public static bool Bit_1584
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 1584) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 1584, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 1816)]
    public static bool Bit_1816
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 1816) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 1816, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2048)]
    public static bool Bit_2048
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2048) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2048, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2049)]
    public static bool Bit_2049
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2049) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2049, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2050)]
    public static bool Bit_2050
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2050) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2050, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2051)]
    public static bool Bit_2051
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2051) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2051, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2052)]
    public static bool Bit_2052
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2052) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2052, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2053)]
    public static bool Bit_2053
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2053) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2053, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2054)]
    public static bool Bit_2054
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2054) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2054, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2055)]
    public static bool Bit_2055
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2055) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2055, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2056)]
    public static bool Bit_2056
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2056) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2056, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2057)]
    public static bool Bit_2057
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2057) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2057, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2058)]
    public static bool Bit_2058
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2058) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2058, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2059)]
    public static bool Bit_2059
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2059) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2059, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2060)]
    public static bool Bit_2060
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2060) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2060, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2061)]
    public static bool Bit_2061
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2061) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2061, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2062)]
    public static bool Bit_2062
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2062) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2062, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2063)]
    public static bool Bit_2063
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2063) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2063, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2064)]
    public static bool Bit_2064
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2064) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2064, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2065)]
    public static bool Bit_2065
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2065) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2065, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2066)]
    public static bool Bit_2066
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2066) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2066, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2067)]
    public static bool Bit_2067
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2067) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2067, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2068)]
    public static bool Bit_2068
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2068) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2068, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2069)]
    public static bool Bit_2069
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2069) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2069, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2070)]
    public static bool Bit_2070
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2070) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2070, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2071)]
    public static bool Bit_2071
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2071) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2071, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2072)]
    public static bool Bit_2072
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2072) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2072, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2073)]
    public static bool Bit_2073
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2073) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2073, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2074)]
    public static bool Bit_2074
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2074) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2074, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2075)]
    public static bool Bit_2075
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2075) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2075, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2076)]
    public static bool Bit_2076
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2076) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2076, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2077)]
    public static bool Bit_2077
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2077) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2077, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2078)]
    public static bool Bit_2078
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2078) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2078, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2079)]
    public static bool Bit_2079
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2079) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2079, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2080)]
    public static bool Bit_2080
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2080) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2080, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2081)]
    public static bool Bit_2081
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2081) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2081, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2082)]
    public static bool Bit_2082
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2082) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2082, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2083)]
    public static bool Bit_2083
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2083) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2083, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2084)]
    public static bool Bit_2084
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2084) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2084, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2085)]
    public static bool Bit_2085
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2085) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2085, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2086)]
    public static bool Bit_2086
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2086) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2086, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2087)]
    public static bool Bit_2087
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2087) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2087, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2088)]
    public static bool Bit_2088
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2088) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2088, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2090)]
    public static bool Bit_2090
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2090) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2090, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2091)]
    public static bool Bit_2091
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2091) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2091, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2092)]
    public static bool Bit_2092
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2092) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2092, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2093)]
    public static bool Bit_2093
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2093) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2093, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2095)]
    public static bool Bit_2095
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2095) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2095, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2096)]
    public static bool Bit_2096
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2096) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2096, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2097)]
    public static bool Bit_2097
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2097) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2097, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2098)]
    public static bool Bit_2098
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2098) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2098, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2099)]
    public static bool Bit_2099
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2099) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2099, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2100)]
    public static bool Bit_2100
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2100) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2100, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2101)]
    public static bool Bit_2101
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2101) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2101, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2102)]
    public static bool Bit_2102
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2102) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2102, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2103)]
    public static bool Bit_2103
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2103) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2103, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2128)]
    public static bool Bit_2128
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2128) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2128, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2418)]
    public static bool Bit_2418
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2418) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2418, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2419)]
    public static bool Bit_2419
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2419) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2419, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2420)]
    public static bool Bit_2420
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2420) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2420, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2421)]
    public static bool Bit_2421
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2421) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2421, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2422)]
    public static bool Bit_2422
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2422) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2422, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2423)]
    public static bool Bit_2423
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2423) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2423, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2432)]
    public static bool Bit_2432
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2432) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2432, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2435)]
    public static bool Bit_2435
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2435) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2435, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2436)]
    public static bool Bit_2436
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2436) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2436, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2437)]
    public static bool Bit_2437
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2437) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2437, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2438)]
    public static bool Bit_2438
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2438) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2438, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2439)]
    public static bool Bit_2439
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2439) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2439, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2440)]
    public static bool Bit_2440
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2440) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2440, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2441)]
    public static bool Bit_2441
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2441) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2441, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2442)]
    public static bool Bit_2442
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2442) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2442, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2444)]
    public static bool Bit_2444
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2444) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2444, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2445)]
    public static bool Bit_2445
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2445) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2445, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2446)]
    public static bool Bit_2446
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2446) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2446, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2447)]
    public static bool Bit_2447
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2447) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2447, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2448)]
    public static bool Bit_2448
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2448) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2448, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2455)]
    public static bool Bit_2455
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2455) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2455, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2456)]
    public static bool Bit_2456
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2456) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2456, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2458)]
    public static bool Bit_2458
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2458) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2458, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2466)]
    public static bool Bit_2466
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2466) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2466, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2471)]
    public static bool Bit_2471
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2471) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2471, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2474)]
    public static bool Bit_2474
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2474) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2474, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2476)]
    public static bool Bit_2476
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2476) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2476, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2477)]
    public static bool Bit_2477
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2477) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2477, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2478)]
    public static bool Bit_2478
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2478) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2478, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2479)]
    public static bool Bit_2479
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2479) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2479, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2480)]
    public static bool Bit_2480
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2480) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2480, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2481)]
    public static bool Bit_2481
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2481) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2481, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2486)]
    public static bool Bit_2486
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2486) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2486, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2487)]
    public static bool Bit_2487
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2487) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2487, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2488)]
    public static bool Bit_2488
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2488) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2488, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2491)]
    public static bool Bit_2491
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2491) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2491, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2492)]
    public static bool Bit_2492
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2492) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2492, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2493)]
    public static bool Bit_2493
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2493) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2493, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2494)]
    public static bool Bit_2494
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2494) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2494, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2495)]
    public static bool Bit_2495
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2495) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2495, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2592)]
    public static bool Bit_2592
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2592) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2592, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2593)]
    public static bool Bit_2593
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2593) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2593, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2594)]
    public static bool Bit_2594
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2594) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2594, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2595)]
    public static bool Bit_2595
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2595) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2595, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2596)]
    public static bool Bit_2596
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2596) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2596, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2597)]
    public static bool Bit_2597
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2597) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2597, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2598)]
    public static bool Bit_2598
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2598) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2598, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2599)]
    public static bool Bit_2599
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2599) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2599, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2600)]
    public static bool Bit_2600
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2600) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2600, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2601)]
    public static bool Bit_2601
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2601) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2601, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2602)]
    public static bool Bit_2602
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2602) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2602, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2603)]
    public static bool Bit_2603
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2603) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2603, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2604)]
    public static bool Bit_2604
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2604) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2604, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2605)]
    public static bool Bit_2605
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2605) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2605, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2606)]
    public static bool Bit_2606
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2606) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2606, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2607)]
    public static bool Bit_2607
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2607) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2607, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2609)]
    public static bool Bit_2609
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2609) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2609, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2611)]
    public static bool Bit_2611
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2611) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2611, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2612)]
    public static bool Bit_2612
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2612) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2612, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2615)]
    public static bool Bit_2615
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2615) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2615, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2616)]
    public static bool Bit_2616
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2616) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2616, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2617)]
    public static bool Bit_2617
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2617) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2617, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2618)]
    public static bool Bit_2618
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2618) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2618, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2619)]
    public static bool Bit_2619
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2619) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2619, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2620)]
    public static bool Bit_2620
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2620) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2620, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2621)]
    public static bool Bit_2621
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2621) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2621, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2622)]
    public static bool Bit_2622
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2622) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2622, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2623)]
    public static bool Bit_2623
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2623) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2623, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2625)]
    public static bool Bit_2625
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2625) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2625, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2626)]
    public static bool Bit_2626
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2626) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2626, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2627)]
    public static bool Bit_2627
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2627) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2627, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2628)]
    public static bool Bit_2628
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2628) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2628, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2630)]
    public static bool Bit_2630
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2630) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2630, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2631)]
    public static bool Bit_2631
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2631) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2631, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2641)]
    public static bool Bit_2641
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2641) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2641, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2642)]
    public static bool Bit_2642
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2642) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2642, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2643)]
    public static bool Bit_2643
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2643) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2643, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2645)]
    public static bool Bit_2645
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2645) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2645, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2646)]
    public static bool Bit_2646
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2646) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2646, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2647)]
    public static bool Bit_2647
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2647) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2647, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2648)]
    public static bool Bit_2648
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2648) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2648, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2649)]
    public static bool Bit_2649
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2649) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2649, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2650)]
    public static bool Bit_2650
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2650) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2650, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2651)]
    public static bool Bit_2651
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2651) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2651, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2652)]
    public static bool Bit_2652
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2652) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2652, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2653)]
    public static bool Bit_2653
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2653) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2653, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2654)]
    public static bool Bit_2654
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2654) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2654, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2655)]
    public static bool Bit_2655
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2655) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2655, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2660)]
    public static bool Bit_2660
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2660) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2660, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2661)]
    public static bool Bit_2661
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2661) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2661, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2662)]
    public static bool Bit_2662
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2662) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2662, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2663)]
    public static bool Bit_2663
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2663) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2663, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2816)]
    public static bool Bit_2816
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2816) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2816, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2817)]
    public static bool Bit_2817
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2817) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2817, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2818)]
    public static bool Bit_2818
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2818) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2818, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2819)]
    public static bool Bit_2819
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2819) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2819, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2820)]
    public static bool Bit_2820
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2820) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2820, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2821)]
    public static bool Bit_2821
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2821) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2821, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2822)]
    public static bool Bit_2822
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2822) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2822, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2823)]
    public static bool Bit_2823
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2823) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2823, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2824)]
    public static bool Bit_2824
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2824) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2824, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2825)]
    public static bool Bit_2825
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2825) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2825, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2826)]
    public static bool Bit_2826
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2826) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2826, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2827)]
    public static bool Bit_2827
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2827) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2827, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2828)]
    public static bool Bit_2828
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2828) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2828, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2829)]
    public static bool Bit_2829
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2829) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2829, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2830)]
    public static bool Bit_2830
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2830) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2830, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2831)]
    public static bool Bit_2831
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2831) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2831, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2836)]
    public static bool Bit_2836
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2836) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2836, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2837)]
    public static bool Bit_2837
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2837) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2837, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2838)]
    public static bool Bit_2838
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2838) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2838, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2839)]
    public static bool Bit_2839
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2839) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2839, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2856)]
    public static bool Bit_2856
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2856) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2856, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2857)]
    public static bool Bit_2857
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2857) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2857, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2858)]
    public static bool Bit_2858
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2858) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2858, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2859)]
    public static bool Bit_2859
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2859) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2859, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2860)]
    public static bool Bit_2860
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2860) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2860, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2861)]
    public static bool Bit_2861
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2861) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2861, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2862)]
    public static bool Bit_2862
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2862) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2862, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2863)]
    public static bool Bit_2863
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2863) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2863, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2864)]
    public static bool Bit_2864
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2864) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2864, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2865)]
    public static bool Bit_2865
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2865) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2865, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2866)]
    public static bool Bit_2866
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2866) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2866, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2867)]
    public static bool Bit_2867
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2867) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2867, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2872)]
    public static bool Bit_2872
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2872) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2872, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2873)]
    public static bool Bit_2873
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2873) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2873, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2874)]
    public static bool Bit_2874
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2874) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2874, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2875)]
    public static bool Bit_2875
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2875) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2875, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2876)]
    public static bool Bit_2876
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2876) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2876, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2877)]
    public static bool Bit_2877
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2877) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2877, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2878)]
    public static bool Bit_2878
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2878) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2878, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2879)]
    public static bool Bit_2879
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2879) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2879, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2884)]
    public static bool Bit_2884
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2884) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2884, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2885)]
    public static bool Bit_2885
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2885) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2885, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2886)]
    public static bool Bit_2886
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2886) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2886, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2887)]
    public static bool Bit_2887
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2887) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2887, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2906)]
    public static bool Bit_2906
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2906) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2906, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2907)]
    public static bool Bit_2907
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2907) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2907, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2908)]
    public static bool Bit_2908
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2908) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2908, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2909)]
    public static bool Bit_2909
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2909) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2909, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2910)]
    public static bool Bit_2910
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2910) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2910, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2911)]
    public static bool Bit_2911
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2911) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2911, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2918)]
    public static bool Bit_2918
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2918) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2918, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2919)]
    public static bool Bit_2919
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2919) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2919, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2921)]
    public static bool Bit_2921
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2921) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2921, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2922)]
    public static bool Bit_2922
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2922) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2922, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2923)]
    public static bool Bit_2923
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2923) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2923, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2924)]
    public static bool Bit_2924
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2924) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2924, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2925)]
    public static bool Bit_2925
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2925) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2925, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2926)]
    public static bool Bit_2926
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2926) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2926, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2927)]
    public static bool Bit_2927
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2927) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2927, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2930)]
    public static bool Bit_2930
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2930) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2930, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2931)]
    public static bool Bit_2931
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2931) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2931, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2932)]
    public static bool Bit_2932
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2932) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2932, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2933)]
    public static bool Bit_2933
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2933) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2933, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2934)]
    public static bool Bit_2934
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2934) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2934, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2935)]
    public static bool Bit_2935
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2935) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2935, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2938)]
    public static bool Bit_2938
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2938) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2938, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2939)]
    public static bool Bit_2939
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2939) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2939, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2940)]
    public static bool Bit_2940
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2940) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2940, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2941)]
    public static bool Bit_2941
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2941) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2941, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2942)]
    public static bool Bit_2942
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2942) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2942, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2943)]
    public static bool Bit_2943
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2943) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2943, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2948)]
    public static bool Bit_2948
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2948) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2948, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2949)]
    public static bool Bit_2949
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2949) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2949, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2950)]
    public static bool Bit_2950
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2950) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2950, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2951)]
    public static bool Bit_2951
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2951) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2951, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2969)]
    public static bool Bit_2969
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2969) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2969, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2970)]
    public static bool Bit_2970
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2970) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2970, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2971)]
    public static bool Bit_2971
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2971) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2971, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2972)]
    public static bool Bit_2972
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2972) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2972, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2973)]
    public static bool Bit_2973
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2973) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2973, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2974)]
    public static bool Bit_2974
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2974) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2974, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2975)]
    public static bool Bit_2975
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2975) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2975, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2976)]
    public static bool Bit_2976
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2976) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2976, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2977)]
    public static bool Bit_2977
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2977) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2977, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2978)]
    public static bool Bit_2978
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2978) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2978, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2979)]
    public static bool Bit_2979
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2979) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2979, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2980)]
    public static bool Bit_2980
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2980) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2980, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2981)]
    public static bool Bit_2981
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2981) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2981, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2982)]
    public static bool Bit_2982
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2982) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2982, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 2983)]
    public static bool Bit_2983
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 2983) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 2983, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3228)]
    public static bool Bit_3228
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3228) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3228, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3229)]
    public static bool Bit_3229
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3229) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3229, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3230)]
    public static bool Bit_3230
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3230) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3230, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3231)]
    public static bool Bit_3231
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3231) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3231, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3232)]
    public static bool Bit_3232
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3232) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3232, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3233)]
    public static bool Bit_3233
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3233) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3233, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3234)]
    public static bool Bit_3234
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3234) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3234, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3235)]
    public static bool Bit_3235
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3235) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3235, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3236)]
    public static bool Bit_3236
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3236) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3236, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3237)]
    public static bool Bit_3237
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3237) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3237, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3238)]
    public static bool Bit_3238
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3238) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3238, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3239)]
    public static bool Bit_3239
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3239) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3239, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3240)]
    public static bool Bit_3240
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3240) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3240, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3242)]
    public static bool Bit_3242
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3242) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3242, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3243)]
    public static bool Bit_3243
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3243) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3243, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3244)]
    public static bool Bit_3244
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3244) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3244, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3245)]
    public static bool Bit_3245
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3245) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3245, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3246)]
    public static bool Bit_3246
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3246) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3246, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3247)]
    public static bool Bit_3247
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3247) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3247, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3248)]
    public static bool Bit_3248
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3248) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3248, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3249)]
    public static bool Bit_3249
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3249) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3249, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3250)]
    public static bool Bit_3250
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3250) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3250, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3251)]
    public static bool Bit_3251
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3251) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3251, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3252)]
    public static bool Bit_3252
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3252) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3252, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3253)]
    public static bool Bit_3253
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3253) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3253, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3254)]
    public static bool Bit_3254
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3254) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3254, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3255)]
    public static bool Bit_3255
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3255) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3255, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3256)]
    public static bool Bit_3256
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3256) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3256, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3257)]
    public static bool Bit_3257
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3257) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3257, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3258)]
    public static bool Bit_3258
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3258) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3258, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3259)]
    public static bool Bit_3259
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3259) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3259, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3260)]
    public static bool Bit_3260
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3260) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3260, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3261)]
    public static bool Bit_3261
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3261) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3261, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3262)]
    public static bool Bit_3262
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3262) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3262, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3263)]
    public static bool Bit_3263
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3263) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3263, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3456)]
    public static bool Bit_3456
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3456) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3456, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3457)]
    public static bool Bit_3457
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3457) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3457, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3458)]
    public static bool Bit_3458
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3458) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3458, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3459)]
    public static bool Bit_3459
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3459) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3459, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3460)]
    public static bool Bit_3460
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3460) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3460, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3461)]
    public static bool Bit_3461
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3461) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3461, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3462)]
    public static bool Bit_3462
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3462) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3462, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3463)]
    public static bool Bit_3463
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3463) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3463, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3464)]
    public static bool Bit_3464
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3464) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3464, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3465)]
    public static bool Bit_3465
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3465) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3465, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3466)]
    public static bool Bit_3466
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3466) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3466, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3468)]
    public static bool Bit_3468
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3468) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3468, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3469)]
    public static bool Bit_3469
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3469) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3469, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3470)]
    public static bool Bit_3470
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3470) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3470, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3471)]
    public static bool Bit_3471
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3471) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3471, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3536)]
    public static bool Bit_3536
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3536) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3536, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3537)]
    public static bool Bit_3537
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3537) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3537, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3538)]
    public static bool Bit_3538
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3538) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3538, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3539)]
    public static bool Bit_3539
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3539) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3539, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3540)]
    public static bool Bit_3540
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3540) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3540, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3541)]
    public static bool Bit_3541
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3541) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3541, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3542)]
    public static bool Bit_3542
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3542) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3542, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3543)]
    public static bool Bit_3543
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3543) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3543, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3544)]
    public static bool Bit_3544
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3544) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3544, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3545)]
    public static bool Bit_3545
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3545) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3545, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3546)]
    public static bool Bit_3546
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3546) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3546, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3547)]
    public static bool Bit_3547
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3547) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3547, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3550)]
    public static bool Bit_3550
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3550) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3550, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3551)]
    public static bool Bit_3551
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3551) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3551, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3552)]
    public static bool Bit_3552
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3552) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3552, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3553)]
    public static bool Bit_3553
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3553) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3553, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3554)]
    public static bool Bit_3554
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3554) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3554, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3555)]
    public static bool Bit_3555
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3555) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3555, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3556)]
    public static bool Bit_3556
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3556) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3556, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3557)]
    public static bool Bit_3557
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3557) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3557, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3558)]
    public static bool Bit_3558
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3558) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3558, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3559)]
    public static bool Bit_3559
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3559) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3559, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3560)]
    public static bool Bit_3560
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3560) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3560, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3561)]
    public static bool Bit_3561
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3561) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3561, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3562)]
    public static bool Bit_3562
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3562) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3562, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3563)]
    public static bool Bit_3563
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3563) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3563, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3564)]
    public static bool Bit_3564
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3564) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3564, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3565)]
    public static bool Bit_3565
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3565) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3565, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3566)]
    public static bool Bit_3566
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3566) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3566, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3567)]
    public static bool Bit_3567
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3567) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3567, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3568)]
    public static bool Bit_3568
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3568) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3568, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3569)]
    public static bool Bit_3569
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3569) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3569, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3570)]
    public static bool Bit_3570
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3570) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3570, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3571)]
    public static bool Bit_3571
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3571) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3571, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3572)]
    public static bool Bit_3572
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3572) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3572, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3573)]
    public static bool Bit_3573
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3573) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3573, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3574)]
    public static bool Bit_3574
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3574) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3574, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3575)]
    public static bool Bit_3575
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3575) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3575, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3576)]
    public static bool Bit_3576
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3576) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3576, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3577)]
    public static bool Bit_3577
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3577) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3577, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3578)]
    public static bool Bit_3578
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3578) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3578, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3579)]
    public static bool Bit_3579
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3579) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3579, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3580)]
    public static bool Bit_3580
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3580) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3580, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3581)]
    public static bool Bit_3581
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3581) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3581, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3582)]
    public static bool Bit_3582
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3582) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3582, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3583)]
    public static bool Bit_3583
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3583) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3583, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3584)]
    public static bool Bit_3584
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3584) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3584, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3585)]
    public static bool Bit_3585
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3585) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3585, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3586)]
    public static bool Bit_3586
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3586) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3586, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3587)]
    public static bool Bit_3587
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3587) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3587, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3588)]
    public static bool Bit_3588
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3588) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3588, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3589)]
    public static bool Bit_3589
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3589) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3589, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3590)]
    public static bool Bit_3590
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3590) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3590, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3591)]
    public static bool Bit_3591
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3591) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3591, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3593)]
    public static bool Bit_3593
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3593) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3593, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3594)]
    public static bool Bit_3594
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3594) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3594, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3595)]
    public static bool Bit_3595
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3595) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3595, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3596)]
    public static bool Bit_3596
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3596) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3596, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3597)]
    public static bool Bit_3597
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3597) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3597, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3599)]
    public static bool Bit_3599
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3599) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3599, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3600)]
    public static bool Bit_3600
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3600) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3600, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3602)]
    public static bool Bit_3602
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3602) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3602, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3603)]
    public static bool Bit_3603
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3603) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3603, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3604)]
    public static bool Bit_3604
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3604) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3604, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3605)]
    public static bool Bit_3605
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3605) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3605, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3606)]
    public static bool Bit_3606
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3606) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3606, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3607)]
    public static bool Bit_3607
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3607) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3607, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3608)]
    public static bool Bit_3608
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3608) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3608, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3609)]
    public static bool Bit_3609
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3609) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3609, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3610)]
    public static bool Bit_3610
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3610) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3610, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3611)]
    public static bool Bit_3611
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3611) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3611, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3612)]
    public static bool Bit_3612
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3612) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3612, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3613)]
    public static bool Bit_3613
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3613) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3613, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3614)]
    public static bool Bit_3614
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3614) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3614, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3615)]
    public static bool Bit_3615
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3615) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3615, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3616)]
    public static bool Bit_3616
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3616) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3616, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3617)]
    public static bool Bit_3617
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3617) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3617, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3618)]
    public static bool Bit_3618
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3618) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3618, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3619)]
    public static bool Bit_3619
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3619) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3619, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3620)]
    public static bool Bit_3620
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3620) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3620, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3621)]
    public static bool Bit_3621
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3621) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3621, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3622)]
    public static bool Bit_3622
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3622) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3622, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3623)]
    public static bool Bit_3623
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3623) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3623, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3630)]
    public static bool Bit_3630
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3630) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3630, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3631)]
    public static bool Bit_3631
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3631) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3631, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3632)]
    public static bool Bit_3632
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3632) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3632, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3664)]
    public static bool Bit_3664
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3664) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3664, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3665)]
    public static bool Bit_3665
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3665) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3665, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3666)]
    public static bool Bit_3666
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3666) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3666, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3667)]
    public static bool Bit_3667
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3667) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3667, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3668)]
    public static bool Bit_3668
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3668) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3668, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3669)]
    public static bool Bit_3669
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3669) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3669, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3670)]
    public static bool Bit_3670
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3670) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3670, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3671)]
    public static bool Bit_3671
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3671) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3671, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3712)]
    public static bool Bit_3712
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3712) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3712, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3713)]
    public static bool Bit_3713
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3713) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3713, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3714)]
    public static bool Bit_3714
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3714) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3714, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3715)]
    public static bool Bit_3715
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3715) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3715, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3716)]
    public static bool Bit_3716
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3716) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3716, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3717)]
    public static bool Bit_3717
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3717) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3717, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3718)]
    public static bool Bit_3718
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3718) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3718, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3719)]
    public static bool Bit_3719
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3719) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3719, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3784)]
    public static bool Bit_3784
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3784) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3784, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3787)]
    public static bool Bit_3787
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3787) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3787, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3788)]
    public static bool Bit_3788
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3788) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3788, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3789)]
    public static bool Bit_3789
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3789) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3789, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3790)]
    public static bool Bit_3790
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3790) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3790, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3791)]
    public static bool Bit_3791
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3791) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3791, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3792)]
    public static bool Bit_3792
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3792) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3792, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3793)]
    public static bool Bit_3793
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3793) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3793, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3794)]
    public static bool Bit_3794
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3794) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3794, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3795)]
    public static bool Bit_3795
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3795) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3795, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3796)]
    public static bool Bit_3796
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3796) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3796, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3797)]
    public static bool Bit_3797
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3797) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3797, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3798)]
    public static bool Bit_3798
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3798) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3798, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3799)]
    public static bool Bit_3799
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3799) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3799, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3811)]
    public static bool Bit_3811
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3811) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3811, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3815)]
    public static bool Bit_3815
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3815) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3815, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3816)]
    public static bool Bit_3816
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3816) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3816, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3817)]
    public static bool Bit_3817
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3817) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3817, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3818)]
    public static bool Bit_3818
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3818) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3818, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3819)]
    public static bool Bit_3819
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3819) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3819, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3820)]
    public static bool Bit_3820
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3820) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3820, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3821)]
    public static bool Bit_3821
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3821) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3821, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3822)]
    public static bool Bit_3822
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3822) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3822, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3823)]
    public static bool Bit_3823
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3823) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3823, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3824)]
    public static bool Bit_3824
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3824) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3824, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3825)]
    public static bool Bit_3825
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3825) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3825, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3826)]
    public static bool Bit_3826
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3826) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3826, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3827)]
    public static bool Bit_3827
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3827) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3827, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3828)]
    public static bool Bit_3828
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3828) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3828, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3829)]
    public static bool Bit_3829
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3829) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3829, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3830)]
    public static bool Bit_3830
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3830) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3830, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3831)]
    public static bool Bit_3831
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3831) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3831, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3840)]
    public static bool Bit_3840
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3840) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3840, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3843)]
    public static bool Bit_3843
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3843) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3843, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3846)]
    public static bool Bit_3846
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3846) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3846, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3847)]
    public static bool Bit_3847
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3847) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3847, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3848)]
    public static bool Bit_3848
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3848) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3848, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3849)]
    public static bool Bit_3849
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3849) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3849, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3850)]
    public static bool Bit_3850
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3850) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3850, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3851)]
    public static bool Bit_3851
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3851) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3851, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3852)]
    public static bool Bit_3852
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3852) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3852, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3853)]
    public static bool Bit_3853
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3853) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3853, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3854)]
    public static bool Bit_3854
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3854) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3854, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3855)]
    public static bool Bit_3855
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3855) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3855, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3856)]
    public static bool Bit_3856
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3856) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3856, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3857)]
    public static bool Bit_3857
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3857) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3857, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3860)]
    public static bool Bit_3860
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3860) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3860, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3861)]
    public static bool Bit_3861
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3861) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3861, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3862)]
    public static bool Bit_3862
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3862) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3862, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3863)]
    public static bool Bit_3863
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3863) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3863, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3881)]
    public static bool Bit_3881
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3881) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3881, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3882)]
    public static bool Bit_3882
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3882) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3882, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3883)]
    public static bool Bit_3883
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3883) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3883, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3884)]
    public static bool Bit_3884
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3884) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3884, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3885)]
    public static bool Bit_3885
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3885) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3885, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3886)]
    public static bool Bit_3886
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3886) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3886, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3887)]
    public static bool Bit_3887
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3887) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3887, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3888)]
    public static bool Bit_3888
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3888) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3888, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3889)]
    public static bool Bit_3889
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3889) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3889, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3890)]
    public static bool Bit_3890
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3890) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3890, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3891)]
    public static bool Bit_3891
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3891) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3891, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3895)]
    public static bool Bit_3895
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3895) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3895, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3896)]
    public static bool Bit_3896
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3896) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3896, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3897)]
    public static bool Bit_3897
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3897) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3897, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3898)]
    public static bool Bit_3898
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3898) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3898, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3899)]
    public static bool Bit_3899
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3899) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3899, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3900)]
    public static bool Bit_3900
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3900) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3900, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3901)]
    public static bool Bit_3901
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3901) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3901, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3902)]
    public static bool Bit_3902
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3902) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3902, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3903)]
    public static bool Bit_3903
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3903) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3903, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3904)]
    public static bool Bit_3904
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3904) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3904, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3905)]
    public static bool Bit_3905
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3905) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3905, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3944)]
    public static bool Bit_3944
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3944) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3944, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3945)]
    public static bool Bit_3945
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3945) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3945, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3946)]
    public static bool Bit_3946
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3946) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3946, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3947)]
    public static bool Bit_3947
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3947) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3947, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3948)]
    public static bool Bit_3948
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3948) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3948, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3949)]
    public static bool Bit_3949
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3949) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3949, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3950)]
    public static bool Bit_3950
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3950) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3950, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3951)]
    public static bool Bit_3951
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3951) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3951, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3952)]
    public static bool Bit_3952
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3952) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3952, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3966)]
    public static bool Bit_3966
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3966) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3966, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 3967)]
    public static bool Bit_3967
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 3967) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 3967, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 4046)]
    public static bool Bit_4046
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 4046) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 4046, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 4047)]
    public static bool Bit_4047
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 4047) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 4047, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7171)]
    public static bool Bit_7171
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7171) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7171, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7172)]
    public static bool Bit_7172
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7172) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7172, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7174)]
    public static bool Bit_7174
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7174) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7174, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7175)]
    public static bool Bit_7175
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7175) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7175, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7184)]
    public static bool Bit_7184
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7184) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7184, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7185)]
    public static bool Bit_7185
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7185) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7185, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7186)]
    public static bool Bit_7186
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7186) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7186, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7187)]
    public static bool Bit_7187
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7187) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7187, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7188)]
    public static bool Bit_7188
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7188) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7188, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7189)]
    public static bool Bit_7189
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7189) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7189, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7190)]
    public static bool Bit_7190
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7190) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7190, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7191)]
    public static bool Bit_7191
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7191) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7191, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7192)]
    public static bool Bit_7192
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7192) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7192, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7193)]
    public static bool Bit_7193
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7193) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7193, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7194)]
    public static bool Bit_7194
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7194) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7194, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7195)]
    public static bool Bit_7195
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7195) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7195, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7196)]
    public static bool Bit_7196
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7196) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7196, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7197)]
    public static bool Bit_7197
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7197) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7197, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7198)]
    public static bool Bit_7198
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7198) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7198, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7199)]
    public static bool Bit_7199
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7199) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7199, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7200)]
    public static bool Bit_7200
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7200) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7200, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7201)]
    public static bool Bit_7201
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7201) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7201, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7202)]
    public static bool Bit_7202
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7202) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7202, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7203)]
    public static bool Bit_7203
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7203) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7203, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7204)]
    public static bool Bit_7204
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7204) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7204, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7205)]
    public static bool Bit_7205
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7205) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7205, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7206)]
    public static bool Bit_7206
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7206) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7206, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7207)]
    public static bool Bit_7207
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7207) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7207, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7211)]
    public static bool Bit_7211
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7211) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7211, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7212)]
    public static bool Bit_7212
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7212) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7212, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7213)]
    public static bool Bit_7213
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7213) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7213, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7214)]
    public static bool Bit_7214
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7214) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7214, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7215)]
    public static bool Bit_7215
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7215) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7215, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7223)]
    public static bool Bit_7223
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7223) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7223, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7230)]
    public static bool Bit_7230
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7230) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7230, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7231)]
    public static bool Bit_7231
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7231) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7231, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7240)]
    public static bool Bit_7240
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7240) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7240, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7241)]
    public static bool Bit_7241
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7241) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7241, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7242)]
    public static bool Bit_7242
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7242) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7242, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7243)]
    public static bool Bit_7243
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7243) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7243, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7244)]
    public static bool Bit_7244
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7244) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7244, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7245)]
    public static bool Bit_7245
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7245) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7245, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7246)]
    public static bool Bit_7246
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7246) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7246, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7247)]
    public static bool Bit_7247
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7247) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7247, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7264)]
    public static bool Bit_7264
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7264) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7264, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7265)]
    public static bool Bit_7265
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7265) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7265, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7266)]
    public static bool Bit_7266
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7266) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7266, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7267)]
    public static bool Bit_7267
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7267) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7267, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7268)]
    public static bool Bit_7268
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7268) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7268, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7269)]
    public static bool Bit_7269
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7269) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7269, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7270)]
    public static bool Bit_7270
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7270) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7270, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7271)]
    public static bool Bit_7271
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7271) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7271, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7272)]
    public static bool Bit_7272
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7272) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7272, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7273)]
    public static bool Bit_7273
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7273) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7273, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7274)]
    public static bool Bit_7274
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7274) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7274, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7275)]
    public static bool Bit_7275
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7275) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7275, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7276)]
    public static bool Bit_7276
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7276) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7276, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7277)]
    public static bool Bit_7277
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7277) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7277, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7278)]
    public static bool Bit_7278
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7278) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7278, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7279)]
    public static bool Bit_7279
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7279) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7279, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7280)]
    public static bool Bit_7280
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7280) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7280, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7281)]
    public static bool Bit_7281
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7281) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7281, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7282)]
    public static bool Bit_7282
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7282) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7282, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7283)]
    public static bool Bit_7283
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7283) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7283, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7284)]
    public static bool Bit_7284
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7284) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7284, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7285)]
    public static bool Bit_7285
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7285) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7285, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7286)]
    public static bool Bit_7286
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7286) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7286, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7287)]
    public static bool Bit_7287
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7287) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7287, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7292)]
    public static bool Bit_7292
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7292) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7292, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7294)]
    public static bool Bit_7294
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7294) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7294, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7295)]
    public static bool Bit_7295
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7295) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7295, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7296)]
    public static bool Bit_7296
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7296) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7296, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7297)]
    public static bool Bit_7297
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7297) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7297, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7298)]
    public static bool Bit_7298
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7298) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7298, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7299)]
    public static bool Bit_7299
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7299) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7299, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7300)]
    public static bool Bit_7300
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7300) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7300, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7301)]
    public static bool Bit_7301
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7301) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7301, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7302)]
    public static bool Bit_7302
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7302) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7302, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7303)]
    public static bool Bit_7303
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7303) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7303, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7304)]
    public static bool Bit_7304
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7304) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7304, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7309)]
    public static bool Bit_7309
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7309) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7309, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7310)]
    public static bool Bit_7310
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7310) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7310, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7311)]
    public static bool Bit_7311
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7311) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7311, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7312)]
    public static bool Bit_7312
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7312) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7312, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7313)]
    public static bool Bit_7313
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7313) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7313, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7314)]
    public static bool Bit_7314
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7314) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7314, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7315)]
    public static bool Bit_7315
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7315) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7315, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7316)]
    public static bool Bit_7316
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7316) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7316, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7317)]
    public static bool Bit_7317
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7317) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7317, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7318)]
    public static bool Bit_7318
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7318) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7318, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7319)]
    public static bool Bit_7319
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7319) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7319, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7320)]
    public static bool Bit_7320
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7320) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7320, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7321)]
    public static bool Bit_7321
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7321) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7321, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7322)]
    public static bool Bit_7322
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7322) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7322, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7323)]
    public static bool Bit_7323
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7323) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7323, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7324)]
    public static bool Bit_7324
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7324) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7324, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7325)]
    public static bool Bit_7325
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7325) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7325, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7326)]
    public static bool Bit_7326
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7326) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7326, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7327)]
    public static bool Bit_7327
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7327) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7327, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7328)]
    public static bool Bit_7328
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7328) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7328, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7329)]
    public static bool Bit_7329
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7329) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7329, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7330)]
    public static bool Bit_7330
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7330) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7330, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7331)]
    public static bool Bit_7331
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7331) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7331, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7332)]
    public static bool Bit_7332
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7332) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7332, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7333)]
    public static bool Bit_7333
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7333) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7333, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7334)]
    public static bool Bit_7334
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7334) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7334, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7335)]
    public static bool Bit_7335
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7335) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7335, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7336)]
    public static bool Bit_7336
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7336) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7336, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7337)]
    public static bool Bit_7337
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7337) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7337, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7338)]
    public static bool Bit_7338
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7338) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7338, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7339)]
    public static bool Bit_7339
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7339) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7339, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7340)]
    public static bool Bit_7340
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7340) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7340, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7341)]
    public static bool Bit_7341
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7341) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7341, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7342)]
    public static bool Bit_7342
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7342) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7342, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7343)]
    public static bool Bit_7343
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7343) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7343, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7344)]
    public static bool Bit_7344
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7344) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7344, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7345)]
    public static bool Bit_7345
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7345) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7345, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7346)]
    public static bool Bit_7346
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7346) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7346, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7347)]
    public static bool Bit_7347
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7347) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7347, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7348)]
    public static bool Bit_7348
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7348) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7348, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7349)]
    public static bool Bit_7349
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7349) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7349, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7350)]
    public static bool Bit_7350
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7350) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7350, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7351)]
    public static bool Bit_7351
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7351) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7351, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7352)]
    public static bool Bit_7352
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7352) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7352, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7353)]
    public static bool Bit_7353
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7353) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7353, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7354)]
    public static bool Bit_7354
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7354) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7354, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7355)]
    public static bool Bit_7355
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7355) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7355, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7356)]
    public static bool Bit_7356
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7356) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7356, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7357)]
    public static bool Bit_7357
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7357) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7357, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7358)]
    public static bool Bit_7358
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7358) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7358, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7359)]
    public static bool Bit_7359
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7359) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7359, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7360)]
    public static bool Bit_7360
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7360) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7360, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7361)]
    public static bool Bit_7361
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7361) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7361, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7362)]
    public static bool Bit_7362
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7362) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7362, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7363)]
    public static bool Bit_7363
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7363) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7363, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7364)]
    public static bool Bit_7364
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7364) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7364, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7365)]
    public static bool Bit_7365
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7365) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7365, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7366)]
    public static bool Bit_7366
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7366) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7366, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7367)]
    public static bool Bit_7367
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7367) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7367, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7371)]
    public static bool Bit_7371
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7371) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7371, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7372)]
    public static bool Bit_7372
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7372) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7372, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7373)]
    public static bool Bit_7373
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7373) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7373, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7374)]
    public static bool Bit_7374
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7374) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7374, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7375)]
    public static bool Bit_7375
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7375) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7375, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7380)]
    public static bool Bit_7380
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7380) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7380, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7381)]
    public static bool Bit_7381
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7381) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7381, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7382)]
    public static bool Bit_7382
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7382) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7382, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7383)]
    public static bool Bit_7383
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7383) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7383, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7384)]
    public static bool Bit_7384
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7384) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7384, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7385)]
    public static bool Bit_7385
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7385) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7385, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7386)]
    public static bool Bit_7386
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7386) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7386, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7387)]
    public static bool Bit_7387
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7387) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7387, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7388)]
    public static bool Bit_7388
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7388) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7388, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7389)]
    public static bool Bit_7389
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7389) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7389, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7390)]
    public static bool Bit_7390
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7390) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7390, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7391)]
    public static bool Bit_7391
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7391) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7391, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7395)]
    public static bool Bit_7395
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7395) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7395, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7396)]
    public static bool Bit_7396
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7396) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7396, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7397)]
    public static bool Bit_7397
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7397) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7397, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7398)]
    public static bool Bit_7398
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7398) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7398, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7399)]
    public static bool Bit_7399
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7399) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7399, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7402)]
    public static bool Bit_7402
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7402) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7402, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7403)]
    public static bool Bit_7403
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7403) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7403, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7404)]
    public static bool Bit_7404
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7404) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7404, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7405)]
    public static bool Bit_7405
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7405) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7405, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7406)]
    public static bool Bit_7406
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7406) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7406, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7407)]
    public static bool Bit_7407
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7407) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7407, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7409)]
    public static bool Bit_7409
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7409) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7409, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7410)]
    public static bool Bit_7410
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7410) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7410, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7411)]
    public static bool Bit_7411
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7411) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7411, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7412)]
    public static bool Bit_7412
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7412) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7412, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7413)]
    public static bool Bit_7413
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7413) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7413, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7414)]
    public static bool Bit_7414
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7414) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7414, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7415)]
    public static bool Bit_7415
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7415) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7415, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7418)]
    public static bool Bit_7418
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7418) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7418, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7419)]
    public static bool Bit_7419
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7419) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7419, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7420)]
    public static bool Bit_7420
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7420) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7420, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7421)]
    public static bool Bit_7421
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7421) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7421, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7422)]
    public static bool Bit_7422
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7422) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7422, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7423)]
    public static bool Bit_7423
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7423) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7423, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7424)]
    public static bool Bit_7424
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7424) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7424, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7425)]
    public static bool Bit_7425
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7425) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7425, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7426)]
    public static bool Bit_7426
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7426) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7426, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7427)]
    public static bool Bit_7427
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7427) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7427, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7428)]
    public static bool Bit_7428
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7428) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7428, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7429)]
    public static bool Bit_7429
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7429) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7429, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7430)]
    public static bool Bit_7430
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7430) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7430, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7431)]
    public static bool Bit_7431
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7431) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7431, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7432)]
    public static bool Bit_7432
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7432) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7432, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7439)]
    public static bool Bit_7439
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7439) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7439, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7440)]
    public static bool Bit_7440
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7440) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7440, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7441)]
    public static bool Bit_7441
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7441) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7441, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7442)]
    public static bool Bit_7442
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7442) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7442, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7443)]
    public static bool Bit_7443
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7443) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7443, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7444)]
    public static bool Bit_7444
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7444) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7444, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7445)]
    public static bool Bit_7445
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7445) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7445, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7446)]
    public static bool Bit_7446
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7446) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7446, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7447)]
    public static bool Bit_7447
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7447) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7447, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7448)]
    public static bool Bit_7448
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7448) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7448, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7449)]
    public static bool Bit_7449
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7449) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7449, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7450)]
    public static bool Bit_7450
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7450) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7450, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7451)]
    public static bool Bit_7451
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7451) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7451, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7452)]
    public static bool Bit_7452
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7452) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7452, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7453)]
    public static bool Bit_7453
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7453) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7453, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7454)]
    public static bool Bit_7454
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7454) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7454, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7455)]
    public static bool Bit_7455
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7455) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7455, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7456)]
    public static bool Bit_7456
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7456) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7456, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7457)]
    public static bool Bit_7457
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7457) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7457, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7458)]
    public static bool Bit_7458
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7458) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7458, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7459)]
    public static bool Bit_7459
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7459) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7459, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7460)]
    public static bool Bit_7460
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7460) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7460, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7461)]
    public static bool Bit_7461
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7461) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7461, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7462)]
    public static bool Bit_7462
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7462) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7462, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7463)]
    public static bool Bit_7463
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7463) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7463, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7464)]
    public static bool Bit_7464
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7464) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7464, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7467)]
    public static bool Bit_7467
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7467) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7467, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7468)]
    public static bool Bit_7468
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7468) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7468, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7469)]
    public static bool Bit_7469
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7469) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7469, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7470)]
    public static bool Bit_7470
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7470) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7470, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7471)]
    public static bool Bit_7471
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7471) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7471, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7472)]
    public static bool Bit_7472
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7472) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7472, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7473)]
    public static bool Bit_7473
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7473) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7473, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7474)]
    public static bool Bit_7474
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7474) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7474, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7475)]
    public static bool Bit_7475
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7475) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7475, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7476)]
    public static bool Bit_7476
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7476) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7476, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7477)]
    public static bool Bit_7477
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7477) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7477, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7478)]
    public static bool Bit_7478
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7478) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7478, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7479)]
    public static bool Bit_7479
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7479) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7479, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7488)]
    public static bool Bit_7488
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7488) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7488, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7489)]
    public static bool Bit_7489
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7489) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7489, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7490)]
    public static bool Bit_7490
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7490) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7490, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7491)]
    public static bool Bit_7491
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7491) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7491, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7492)]
    public static bool Bit_7492
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7492) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7492, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7493)]
    public static bool Bit_7493
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7493) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7493, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7494)]
    public static bool Bit_7494
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7494) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7494, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7495)]
    public static bool Bit_7495
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7495) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7495, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7496)]
    public static bool Bit_7496
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7496) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7496, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7497)]
    public static bool Bit_7497
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7497) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7497, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7498)]
    public static bool Bit_7498
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7498) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7498, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7499)]
    public static bool Bit_7499
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7499) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7499, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7500)]
    public static bool Bit_7500
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7500) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7500, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7501)]
    public static bool Bit_7501
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7501) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7501, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7502)]
    public static bool Bit_7502
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7502) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7502, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7503)]
    public static bool Bit_7503
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7503) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7503, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7507)]
    public static bool Bit_7507
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7507) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7507, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7508)]
    public static bool Bit_7508
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7508) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7508, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7509)]
    public static bool Bit_7509
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7509) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7509, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7510)]
    public static bool Bit_7510
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7510) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7510, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7511)]
    public static bool Bit_7511
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7511) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7511, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7512)]
    public static bool Bit_7512
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7512) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7512, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7513)]
    public static bool Bit_7513
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7513) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7513, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7514)]
    public static bool Bit_7514
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7514) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7514, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7515)]
    public static bool Bit_7515
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7515) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7515, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7516)]
    public static bool Bit_7516
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7516) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7516, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7517)]
    public static bool Bit_7517
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7517) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7517, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7518)]
    public static bool Bit_7518
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7518) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7518, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7519)]
    public static bool Bit_7519
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7519) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7519, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7520)]
    public static bool Bit_7520
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7520) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7520, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7521)]
    public static bool Bit_7521
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7521) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7521, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7522)]
    public static bool Bit_7522
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7522) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7522, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7523)]
    public static bool Bit_7523
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7523) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7523, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7524)]
    public static bool Bit_7524
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7524) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7524, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7525)]
    public static bool Bit_7525
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7525) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7525, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7526)]
    public static bool Bit_7526
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7526) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7526, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7527)]
    public static bool Bit_7527
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7527) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7527, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7534)]
    public static bool Bit_7534
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7534) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7534, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7535)]
    public static bool Bit_7535
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7535) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7535, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7539)]
    public static bool Bit_7539
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7539) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7539, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7540)]
    public static bool Bit_7540
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7540) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7540, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7541)]
    public static bool Bit_7541
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7541) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7541, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7542)]
    public static bool Bit_7542
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7542) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7542, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7543)]
    public static bool Bit_7543
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7543) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7543, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7544)]
    public static bool Bit_7544
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7544) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7544, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7545)]
    public static bool Bit_7545
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7545) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7545, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7546)]
    public static bool Bit_7546
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7546) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7546, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7547)]
    public static bool Bit_7547
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7547) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7547, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7548)]
    public static bool Bit_7548
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7548) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7548, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7549)]
    public static bool Bit_7549
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7549) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7549, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7550)]
    public static bool Bit_7550
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7550) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7550, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7551)]
    public static bool Bit_7551
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7551) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7551, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7552)]
    public static bool Bit_7552
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7552) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7552, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7553)]
    public static bool Bit_7553
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7553) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7553, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7554)]
    public static bool Bit_7554
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7554) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7554, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7555)]
    public static bool Bit_7555
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7555) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7555, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7556)]
    public static bool Bit_7556
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7556) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7556, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7557)]
    public static bool Bit_7557
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7557) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7557, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7558)]
    public static bool Bit_7558
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7558) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7558, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7559)]
    public static bool Bit_7559
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7559) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7559, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7563)]
    public static bool Bit_7563
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7563) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7563, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7564)]
    public static bool Bit_7564
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7564) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7564, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7565)]
    public static bool Bit_7565
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7565) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7565, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7566)]
    public static bool Bit_7566
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7566) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7566, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7567)]
    public static bool Bit_7567
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7567) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7567, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7568)]
    public static bool Bit_7568
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7568) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7568, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7570)]
    public static bool Bit_7570
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7570) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7570, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7571)]
    public static bool Bit_7571
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7571) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7571, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7572)]
    public static bool Bit_7572
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7572) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7572, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7573)]
    public static bool Bit_7573
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7573) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7573, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7574)]
    public static bool Bit_7574
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7574) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7574, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7575)]
    public static bool Bit_7575
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7575) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7575, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7576)]
    public static bool Bit_7576
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7576) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7576, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7577)]
    public static bool Bit_7577
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7577) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7577, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7582)]
    public static bool Bit_7582
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7582) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7582, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7583)]
    public static bool Bit_7583
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7583) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7583, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7584)]
    public static bool Bit_7584
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7584) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7584, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7585)]
    public static bool Bit_7585
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7585) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7585, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7586)]
    public static bool Bit_7586
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7586) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7586, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7587)]
    public static bool Bit_7587
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7587) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7587, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7588)]
    public static bool Bit_7588
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7588) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7588, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7589)]
    public static bool Bit_7589
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7589) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7589, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7590)]
    public static bool Bit_7590
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7590) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7590, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7591)]
    public static bool Bit_7591
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7591) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7591, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7592)]
    public static bool Bit_7592
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7592) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7592, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7593)]
    public static bool Bit_7593
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7593) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7593, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7594)]
    public static bool Bit_7594
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7594) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7594, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7595)]
    public static bool Bit_7595
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7595) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7595, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7596)]
    public static bool Bit_7596
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7596) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7596, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7597)]
    public static bool Bit_7597
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7597) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7597, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7598)]
    public static bool Bit_7598
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7598) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7598, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7599)]
    public static bool Bit_7599
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7599) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7599, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7603)]
    public static bool Bit_7603
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7603) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7603, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7604)]
    public static bool Bit_7604
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7604) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7604, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7605)]
    public static bool Bit_7605
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7605) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7605, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7606)]
    public static bool Bit_7606
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7606) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7606, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7607)]
    public static bool Bit_7607
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7607) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7607, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7616)]
    public static bool Bit_7616
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7616) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7616, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7617)]
    public static bool Bit_7617
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7617) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7617, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7618)]
    public static bool Bit_7618
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7618) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7618, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7619)]
    public static bool Bit_7619
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7619) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7619, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7620)]
    public static bool Bit_7620
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7620) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7620, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7621)]
    public static bool Bit_7621
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7621) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7621, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7622)]
    public static bool Bit_7622
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7622) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7622, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7623)]
    public static bool Bit_7623
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7623) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7623, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7627)]
    public static bool Bit_7627
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7627) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7627, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7628)]
    public static bool Bit_7628
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7628) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7628, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7629)]
    public static bool Bit_7629
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7629) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7629, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7630)]
    public static bool Bit_7630
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7630) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7630, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7631)]
    public static bool Bit_7631
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7631) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7631, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7648)]
    public static bool Bit_7648
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7648) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7648, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7649)]
    public static bool Bit_7649
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7649) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7649, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7650)]
    public static bool Bit_7650
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7650) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7650, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7651)]
    public static bool Bit_7651
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7651) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7651, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7652)]
    public static bool Bit_7652
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7652) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7652, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7653)]
    public static bool Bit_7653
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7653) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7653, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7654)]
    public static bool Bit_7654
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7654) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7654, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7655)]
    public static bool Bit_7655
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7655) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7655, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7657)]
    public static bool Bit_7657
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7657) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7657, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7658)]
    public static bool Bit_7658
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7658) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7658, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7659)]
    public static bool Bit_7659
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7659) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7659, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7660)]
    public static bool Bit_7660
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7660) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7660, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7661)]
    public static bool Bit_7661
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7661) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7661, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7662)]
    public static bool Bit_7662
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7662) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7662, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7663)]
    public static bool Bit_7663
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7663) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7663, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7681)]
    public static bool Bit_7681
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7681) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7681, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7682)]
    public static bool Bit_7682
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7682) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7682, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7683)]
    public static bool Bit_7683
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7683) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7683, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7684)]
    public static bool Bit_7684
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7684) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7684, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7685)]
    public static bool Bit_7685
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7685) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7685, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7686)]
    public static bool Bit_7686
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7686) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7686, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7687)]
    public static bool Bit_7687
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7687) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7687, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7688)]
    public static bool Bit_7688
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7688) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7688, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7689)]
    public static bool Bit_7689
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7689) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7689, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7690)]
    public static bool Bit_7690
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7690) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7690, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7691)]
    public static bool Bit_7691
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7691) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7691, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7692)]
    public static bool Bit_7692
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7692) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7692, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7693)]
    public static bool Bit_7693
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7693) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7693, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7694)]
    public static bool Bit_7694
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7694) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7694, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7695)]
    public static bool Bit_7695
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7695) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7695, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7696)]
    public static bool Bit_7696
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7696) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7696, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7697)]
    public static bool Bit_7697
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7697) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7697, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7698)]
    public static bool Bit_7698
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7698) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7698, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7699)]
    public static bool Bit_7699
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7699) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7699, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7700)]
    public static bool Bit_7700
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7700) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7700, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7701)]
    public static bool Bit_7701
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7701) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7701, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7702)]
    public static bool Bit_7702
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7702) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7702, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7703)]
    public static bool Bit_7703
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7703) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7703, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7704)]
    public static bool Bit_7704
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7704) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7704, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7705)]
    public static bool Bit_7705
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7705) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7705, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7706)]
    public static bool Bit_7706
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7706) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7706, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7707)]
    public static bool Bit_7707
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7707) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7707, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7708)]
    public static bool Bit_7708
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7708) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7708, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7709)]
    public static bool Bit_7709
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7709) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7709, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7710)]
    public static bool Bit_7710
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7710) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7710, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7711)]
    public static bool Bit_7711
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7711) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7711, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7712)]
    public static bool Bit_7712
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7712) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7712, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7713)]
    public static bool Bit_7713
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7713) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7713, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7714)]
    public static bool Bit_7714
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7714) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7714, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7715)]
    public static bool Bit_7715
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7715) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7715, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7716)]
    public static bool Bit_7716
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7716) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7716, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7717)]
    public static bool Bit_7717
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7717) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7717, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7718)]
    public static bool Bit_7718
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7718) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7718, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7719)]
    public static bool Bit_7719
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7719) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7719, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7727)]
    public static bool Bit_7727
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7727) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7727, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7730)]
    public static bool Bit_7730
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7730) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7730, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7731)]
    public static bool Bit_7731
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7731) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7731, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7732)]
    public static bool Bit_7732
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7732) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7732, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7733)]
    public static bool Bit_7733
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7733) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7733, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7734)]
    public static bool Bit_7734
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7734) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7734, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7735)]
    public static bool Bit_7735
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7735) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7735, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7742)]
    public static bool Bit_7742
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7742) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7742, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7743)]
    public static bool Bit_7743
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7743) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7743, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7744)]
    public static bool Bit_7744
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7744) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7744, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7745)]
    public static bool Bit_7745
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7745) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7745, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7746)]
    public static bool Bit_7746
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7746) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7746, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7747)]
    public static bool Bit_7747
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7747) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7747, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7748)]
    public static bool Bit_7748
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7748) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7748, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7749)]
    public static bool Bit_7749
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7749) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7749, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7750)]
    public static bool Bit_7750
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7750) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7750, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7751)]
    public static bool Bit_7751
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7751) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7751, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7759)]
    public static bool Bit_7759
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7759) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7759, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7767)]
    public static bool Bit_7767
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7767) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7767, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 7775)]
    public static bool Bit_7775
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 7775) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 7775, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8376)]
    public static bool Bit_8376
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8376) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8376, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8377)]
    public static bool Bit_8377
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8377) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8377, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8378)]
    public static bool Bit_8378
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8378) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8378, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8379)]
    public static bool Bit_8379
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8379) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8379, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8380)]
    public static bool Bit_8380
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8380) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8380, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8381)]
    public static bool Bit_8381
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8381) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8381, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8382)]
    public static bool Bit_8382
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8382) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8382, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8383)]
    public static bool Bit_8383
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8383) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8383, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8384)]
    public static bool Bit_8384
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8384) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8384, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8385)]
    public static bool Bit_8385
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8385) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8385, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8386)]
    public static bool Bit_8386
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8386) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8386, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8387)]
    public static bool Bit_8387
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8387) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8387, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8388)]
    public static bool Bit_8388
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8388) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8388, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8389)]
    public static bool Bit_8389
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8389) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8389, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8390)]
    public static bool Bit_8390
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8390) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8390, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8391)]
    public static bool Bit_8391
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8391) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8391, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8392)]
    public static bool Bit_8392
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8392) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8392, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8393)]
    public static bool Bit_8393
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8393) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8393, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8394)]
    public static bool Bit_8394
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8394) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8394, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8395)]
    public static bool Bit_8395
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8395) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8395, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8396)]
    public static bool Bit_8396
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8396) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8396, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8397)]
    public static bool Bit_8397
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8397) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8397, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8398)]
    public static bool Bit_8398
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8398) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8398, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8399)]
    public static bool Bit_8399
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8399) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8399, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8400)]
    public static bool Bit_8400
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8400) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8400, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8401)]
    public static bool Bit_8401
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8401) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8401, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8402)]
    public static bool Bit_8402
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8402) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8402, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8403)]
    public static bool Bit_8403
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8403) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8403, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8404)]
    public static bool Bit_8404
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8404) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8404, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8405)]
    public static bool Bit_8405
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8405) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8405, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8406)]
    public static bool Bit_8406
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8406) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8406, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8407)]
    public static bool Bit_8407
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8407) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8407, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8408)]
    public static bool Bit_8408
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8408) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8408, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8409)]
    public static bool Bit_8409
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8409) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8409, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8410)]
    public static bool Bit_8410
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8410) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8410, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8411)]
    public static bool Bit_8411
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8411) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8411, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8412)]
    public static bool Bit_8412
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8412) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8412, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8413)]
    public static bool Bit_8413
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8413) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8413, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8414)]
    public static bool Bit_8414
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8414) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8414, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8415)]
    public static bool Bit_8415
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8415) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8415, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8416)]
    public static bool Bit_8416
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8416) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8416, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8417)]
    public static bool Bit_8417
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8417) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8417, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8418)]
    public static bool Bit_8418
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8418) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8418, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8419)]
    public static bool Bit_8419
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8419) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8419, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8420)]
    public static bool Bit_8420
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8420) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8420, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8421)]
    public static bool Bit_8421
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8421) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8421, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8422)]
    public static bool Bit_8422
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8422) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8422, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8423)]
    public static bool Bit_8423
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8423) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8423, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8424)]
    public static bool Bit_8424
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8424) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8424, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8425)]
    public static bool Bit_8425
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8425) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8425, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8426)]
    public static bool Bit_8426
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8426) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8426, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8427)]
    public static bool Bit_8427
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8427) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8427, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8428)]
    public static bool Bit_8428
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8428) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8428, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8429)]
    public static bool Bit_8429
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8429) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8429, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8430)]
    public static bool Bit_8430
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8430) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8430, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8431)]
    public static bool Bit_8431
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8431) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8431, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8432)]
    public static bool Bit_8432
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8432) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8432, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8433)]
    public static bool Bit_8433
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8433) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8433, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8434)]
    public static bool Bit_8434
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8434) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8434, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8435)]
    public static bool Bit_8435
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8435) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8435, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8436)]
    public static bool Bit_8436
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8436) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8436, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8437)]
    public static bool Bit_8437
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8437) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8437, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8438)]
    public static bool Bit_8438
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8438) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8438, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8439)]
    public static bool Bit_8439
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8439) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8439, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8440)]
    public static bool Bit_8440
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8440) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8440, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8441)]
    public static bool Bit_8441
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8441) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8441, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8442)]
    public static bool Bit_8442
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8442) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8442, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8443)]
    public static bool Bit_8443
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8443) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8443, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8444)]
    public static bool Bit_8444
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8444) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8444, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8445)]
    public static bool Bit_8445
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8445) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8445, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8446)]
    public static bool Bit_8446
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8446) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8446, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8447)]
    public static bool Bit_8447
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8447) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8447, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8448)]
    public static bool Bit_8448
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8448) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8448, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8449)]
    public static bool Bit_8449
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8449) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8449, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8450)]
    public static bool Bit_8450
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8450) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8450, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8451)]
    public static bool Bit_8451
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8451) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8451, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8452)]
    public static bool Bit_8452
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8452) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8452, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8453)]
    public static bool Bit_8453
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8453) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8453, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8454)]
    public static bool Bit_8454
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8454) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8454, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8455)]
    public static bool Bit_8455
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8455) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8455, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8456)]
    public static bool Bit_8456
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8456) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8456, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8457)]
    public static bool Bit_8457
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8457) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8457, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8458)]
    public static bool Bit_8458
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8458) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8458, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8459)]
    public static bool Bit_8459
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8459) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8459, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8460)]
    public static bool Bit_8460
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8460) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8460, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8461)]
    public static bool Bit_8461
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8461) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8461, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8462)]
    public static bool Bit_8462
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8462) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8462, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8463)]
    public static bool Bit_8463
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8463) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8463, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8464)]
    public static bool Bit_8464
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8464) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8464, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8465)]
    public static bool Bit_8465
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8465) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8465, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8466)]
    public static bool Bit_8466
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8466) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8466, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8467)]
    public static bool Bit_8467
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8467) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8467, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8468)]
    public static bool Bit_8468
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8468) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8468, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8469)]
    public static bool Bit_8469
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8469) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8469, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8470)]
    public static bool Bit_8470
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8470) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8470, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8471)]
    public static bool Bit_8471
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8471) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8471, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8472)]
    public static bool Bit_8472
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8472) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8472, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8473)]
    public static bool Bit_8473
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8473) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8473, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8474)]
    public static bool Bit_8474
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8474) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8474, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8475)]
    public static bool Bit_8475
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8475) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8475, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8476)]
    public static bool Bit_8476
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8476) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8476, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8477)]
    public static bool Bit_8477
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8477) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8477, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8478)]
    public static bool Bit_8478
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8478) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8478, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8479)]
    public static bool Bit_8479
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8479) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8479, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8480)]
    public static bool Bit_8480
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8480) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8480, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8481)]
    public static bool Bit_8481
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8481) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8481, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8482)]
    public static bool Bit_8482
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8482) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8482, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8483)]
    public static bool Bit_8483
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8483) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8483, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8484)]
    public static bool Bit_8484
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8484) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8484, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8485)]
    public static bool Bit_8485
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8485) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8485, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8486)]
    public static bool Bit_8486
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8486) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8486, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8487)]
    public static bool Bit_8487
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8487) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8487, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8488)]
    public static bool Bit_8488
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8488) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8488, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8489)]
    public static bool Bit_8489
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8489) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8489, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8490)]
    public static bool Bit_8490
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8490) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8490, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8491)]
    public static bool Bit_8491
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8491) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8491, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8492)]
    public static bool Bit_8492
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8492) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8492, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8493)]
    public static bool Bit_8493
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8493) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8493, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8494)]
    public static bool Bit_8494
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8494) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8494, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8495)]
    public static bool Bit_8495
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8495) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8495, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8496)]
    public static bool Bit_8496
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8496) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8496, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8497)]
    public static bool Bit_8497
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8497) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8497, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8498)]
    public static bool Bit_8498
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8498) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8498, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8499)]
    public static bool Bit_8499
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8499) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8499, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8500)]
    public static bool Bit_8500
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8500) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8500, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8501)]
    public static bool Bit_8501
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8501) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8501, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8502)]
    public static bool Bit_8502
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8502) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8502, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8503)]
    public static bool Bit_8503
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8503) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8503, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8510)]
    public static bool Bit_8510
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8510) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8510, value ? 1 : 0);
    }

    [StorageView(StorageScope.Global, StorageType.Bit, 8511)]
    public static bool Bit_8511
    {
        get => ScriptMemory.Read(StorageScope.Global, StorageType.Bit, 8511) != 0;
        set => ScriptMemory.Write(StorageScope.Global, StorageType.Bit, 8511, value ? 1 : 0);
    }

}

