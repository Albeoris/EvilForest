namespace EvilForest.EventSource;

public enum Opcode
{
    NOP = 0x0,
    JMP = 0x1,
    JMP_IFN = 0x2,
    JMP_IF = 0x3,
    Return = 0x4,
    EXPR = 0x5,
    JMP_SWITCHEX = 0x6,
    NEW = 0x7,
    NEW2 = 0x8,
    NEW3 = 0x9,
    pad0a = 0xA,
    JMP_SWITCH = 0xB,
    rsv0c = 0xC,
    rsv0d = 0xD,
    pad0e = 0xE,
    pad0f = 0xF,
    REQ = 0x10,
    pad11 = 0x11,
    REQSW = 0x12,
    pad13 = 0x13,
    REQEW = 0x14,
    pad15 = 0x15,
    REPLY = 0x16,
    pad17 = 0x17,
    REPLYSW = 0x18,
    pad19 = 0x19,
    REPLYEW = 0x1A,
    SONGFLAG = 0x1B,
    DELETE = 0x1C,
    POS = 0x1D,
    BGVPORT = 0x1E,
    MES = 0x1F,
    MESN = 0x20,
    CLOSE = 0x21,
    WAIT = 0x22,
    MOVE = 0x23,
    MOVA = 0x24,
    CLRDIST = 0x25,
    MSPEED = 0x26,
    BGIMASK = 0x27,
    FMV = 0x28,
    QUAD = 0x29,
    ENCOUNT = 0x2A,
    MAPJUMP = 0x2B,
    CC = 0x2C,
    UCOFF = 0x2D,
    UCON = 0x2E,
    MODEL = 0x2F,
    PRINT1 = 0x30,
    PRINTF = 0x31,
    LOCATE = 0x32,
    AIDLE = 0x33,
    AWALK = 0x34,
    ARUN = 0x35,
    DIRE = 0x36,
    ROTXZ = 0x37,
    BTLCMD = 0x38,
    MESHSHOW = 0x39,
    MESHHIDE = 0x3A,
    OBJINDEX = 0x3B,
    ENCSCENE = 0x3C,
    AFRAME = 0x3D,
    ASPEED = 0x3E,
    AMODE = 0x3F,
    ANIM = 0x40,
    WAITANIM = 0x41,
    ENDANIM = 0x42,
    STARTSEQ = 0x43,
    WAITSEQ = 0x44,
    ENDSEQ = 0x45,
    DEBUGCC = 0x46,
    NECKFLAG = 0x47,
    ITEMADD = 0x48,
    ITEMDELETE = 0x49,
    BTLSET = 0x4A,
    RADIUS = 0x4B,
    ATTACH = 0x4C,
    DETACH = 0x4D,
    WATCH = 0x4E,
    STOP = 0x4F,
    WAITTURN = 0x50,
    TURNA = 0x51,
    ASLEEP = 0x52,
    NOINITMES = 0x53,
    WAITMES = 0x54,
    MROT = 0x55,
    TURN = 0x56,
    ENCRATE = 0x57,
    BGSMOVE = 0x58,
    BGLCOLOR = 0x59,
    BGLMOVE = 0x5A,
    BGLACTIVE = 0x5B,
    BGLLOOP = 0x5C,
    BGLPARALLAX = 0x5D,
    BGLORIGIN = 0x5E,
    BGAANIME = 0x5F,
    BGAACTIVE = 0x60,
    BGARATE = 0x61,
    SETROW = 0x62,
    BGAWAIT = 0x63,
    BGAFLAG = 0x64,
    BGARANGE = 0x65,
    MESVALUE = 0x66,
    TWIST = 0x67,
    FICON = 0x68,
    TIMERSET = 0x69,
    DASHOFF = 0x6A,
    CLEARCOLOR = 0x6B,
    PPRINT = 0x6C,
    PPRINTF = 0x6D,
    MAPID = 0x6E,
    BGSSCROLL = 0x6F,
    BGSRELEASE = 0x70,
    BGCACTIVE = 0x71,
    BGCHEIGHT = 0x72,
    BGCLOCK = 0x73,
    BGCUNLOCK = 0x74,
    MENU = 0x75,
    TRACKSTART = 0x76,
    TRACK = 0x77,
    TRACKADD = 0x78,
    PRINTQUAD = 0x79,
    ATURNL = 0x7A,
    ATURNR = 0x7B,
    CHOOSEPARAM = 0x7C,
    TIMERCONTROL = 0x7D,
    SETCAM = 0x7E,
    SHADOWON = 0x7F,
    SHADOWOFF = 0x80,
    SHADOWSCALE = 0x81,
    SHADOWOFFSET = 0x82,
    SHADOWLOCK = 0x83,
    SHADOWUNLOCK = 0x84,
    SHADOWAMP = 0x85,
    IDLESPEED = 0x86,
    DDIR = 0x87,
    CHRFX = 0x88,
    SEPV = 0x89,
    SEPVA = 0x8A,
    NECKID = 0x8B,
    ENCOUNT2 = 0x8C,
    TIMERDISPLAY = 0x8D,
    RAISE = 0x8E,
    CHRCOLOR = 0x8F,
    SLEEPINH = 0x90,
    AUTOTURN = 0x91,
    BGLATTACH = 0x92,
    CFLAG = 0x93,
    AJUMP = 0x94,
    MESA = 0x95,
    MESAN = 0x96,
    DRET = 0x97,
    MOVT = 0x98,
    TSPEED = 0x99,
    BGIACTIVET = 0x9A,
    TURNTO = 0x9B,
    PREJUMP = 0x9C,
    POSTJUMP = 0x9D,
    MOVQ = 0x9E,
    CHRSCALE = 0x9F,
    MOVJ = 0xA0,
    POS3 = 0xA1,
    MOVE3 = 0xA2,
    DRADIUS = 0xA3,
    MJPOS = 0xA4,
    MOVH = 0xA5,
    SPEEDTH = 0xA6,
    TURNDS = 0xA7,
    BGI = 0xA8,
    GETSCREEN = 0xA9,
    MENUON = 0xAA,
    MENUOFF = 0xAB,
    DISCCHANGE = 0xAC,
    DPOS3 = 0xAD,
    MINIGAME = 0xAE,
    DELETEALLCARD = 0xAF,
    SETMAPNAME = 0xB0,
    RESETMAPNAME = 0xB1,
    PARTYMENU = 0xB2,
    SPS = 0xB3,
    FULLMEMBER = 0xB4,
    PRETEND = 0xB5,
    WMAPJUMP = 0xB6,
    EYE = 0xB7,
    AIM = 0xB8,
    SETKEYMASK = 0xB9,
    CLEARKEYMASK = 0xBA,
    DTURN = 0xBB,
    DWAITTURN = 0xBC,
    DANIM = 0xBD,
    DWAITANIM = 0xBE,
    DPOS = 0xBF,
    TEXPLAY = 0xC0,
    TEXPLAY1 = 0xC1,
    TEXSTOP = 0xC2,
    BGVSET = 0xC3,
    WPRM = 0xC4,
    FLDSND0 = 0xC5,
    FLDSND1 = 0xC6,
    FLDSND2 = 0xC7,
    FLDSND3 = 0xC8,
    BGVDEFINE = 0xC9,
    BGAVISIBLE = 0xCA,
    BGIACTIVEF = 0xCB,
    CHRSET = 0xCC,
    CHRCLEAR = 0xCD,
    GILADD = 0xCE,
    GILDELETE = 0xCF,
    MESB = 0xD0,
    GLOBALCLEAR = 0xD1,
    DEBUGSAVE = 0xD2,
    DEBUGLOAD = 0xD3,
    ATTACHOFFSET = 0xD4,
    PUSHHIDE = 0xD5,
    POPSHOW = 0xD6,
    AICON = 0xD7,
    RAIN = 0xD8,
    CLEARSTATUS = 0xD9,
    SPS2 = 0xDA,
    WINPOSE = 0xDB,
    JUMP3 = 0xDC,
    PARTYDELETE = 0xDD,
    PLAYERNAME = 0xDE,
    OVAL = 0xDF,
    INCFROG = 0xE0,
    BEND = 0xE1,
    SETVY3 = 0xE2,
    SETSIGNAL = 0xE3,
    BGLSCROLLOFFSET = 0xE4,
    BTLSEQ = 0xE5,
    BGLLOOPTYPE = 0xE6,
    BGAFRAME = 0xE7,
    MOVE3H = 0xE8,
    SYNCPARTY = 0xE9,
    VRP = 0xEA,
    CLOSEALL = 0xEB,
    WIPERGB = 0xEC,
    BGVALPHA = 0xED,
    SLEEPON = 0xEE,
    HEREON = 0xEF,
    DASHON = 0xF0,
    SETHP = 0xF1,
    SETMP = 0xF2,
    CLEARAP = 0xF3,
    MAXAP = 0xF4,
    GAMEOVER = 0xF5,
    VIBSTART = 0xF6,
    VIBACTIVE = 0xF7,
    VIBTRACK1 = 0xF8,
    VIBTRACK = 0xF9,
    VIBRATE = 0xFA,
    VIBFLAG = 0xFB,
    VIBRANGE = 0xFC,
    HINT = 0xFD,
    JOIN = 0xFE,
    EXT = 0xFF,
}

public enum Excode
{
    B_PAD0 = 0x0,
    B_PAD1 = 0x1,
    B_PAD2 = 0x2,
    B_PAD3 = 0x3,
    IncrementPost = 0x4,
    DecrementPost = 0x5,
    B_PRE_PLUS = 0x6,
    B_PRE_MINUS = 0x7,
    B_POST_PLUS_A = 0x8,
    B_POST_MINUS_A = 0x9,
    B_PRE_PLUS_A = 0xA,
    B_PRE_MINUS_A = 0xB,
    B_SINGLE_PLUS = 0xC,
    B_SINGLE_MINUS = 0xD,
    B_NOT = 0xE,
    B_NOT_E = 0xF,
    B_COMP = 0x10,
    Mul = 0x11,
    Div = 0x12,
    Mod = 0x13,
    Add = 0x14,
    Sub = 0x15,
    BitLeft = 0x16,
    BitRight = 0x17,
    LessThan = 0x18,
    GreatThan = 0x19,
    LessOrEquals = 0x1A,
    GreatOrEquals = 0x1B,
    LessThanList = 0x1C,
    GreatThanList = 0x1D,
    LessOrEqualsList = 0x1E,
    GreatOrEqualsList = 0x1F,
    Equivalence = 0x20,
    NotEquivalence = 0x21,
    B_EQ_E = 0x22,
    B_NE_E = 0x23,
    BitAnd = 0x24,
    BitXor = 0x25,
    BitOr = 0x26,
    BooleanAnd = 0x27,
    BooleanOr = 0x28,
    B_MEMBER = 0x29,
    B_COUNT = 0x2A,
    B_PICK = 0x2B,
    Let = 0x2C,
    B_LET_A = 0x2D,
    B_LET_E = 0x2E,
    MulLet = 0x2F,
    DivLet = 0x30,
    ModeLet = 0x31,
    AddLet = 0x32,
    SubLet = 0x33,
    BitLeftLet = 0x34,
    BitRightLet = 0x35,
    B_MULT_LET_A = 0x36,
    B_DIV_LET_A = 0x37,
    B_REM_LET_A = 0x38,
    B_PLUS_LET_A = 0x39,
    B_MINUS_LET_A = 0x3A,
    B_SHIFT_LEFT_LET_A = 0x3B,
    B_SHIFT_RIGHT_LET_A = 0x3C,
    BitAndLet = 0x3D,
    BitXorLet = 0x3E,
    BitOrLet = 0x3F,
    B_AND_LET_A = 0x40,
    B_XOR_LET_A = 0x41,
    B_OR_LET_A = 0x42,
    B_AND_LET_E = 0x43,
    B_XOR_LET_E = 0x44,
    B_OR_LET_E = 0x45,
    B_CAST8 = 0x46,
    B_CAST8U = 0x47,
    B_CAST16 = 0x48,
    B_CAST16U = 0x49,
    B_CAST_LIST = 0x4A,
    B_LMAX = 0x4B,
    B_LMIN = 0x4C,
    B_SELECT = 0x4D,
    B_OBJSPEC = 0x4E,
    IsKeyPressed = 0x4F,
    B_SIN2 = 0x50,
    B_COS2 = 0x51,
    GetCurrentHP = 0x52,
    GetMaxHP = 0x53,
    B_AND_E = 0x54,
    B_NAND_E = 0x55,
    B_XOR_E = 0x56,
    B_OR_E = 0x57,
    B_KEYOFF = 0x58,
    B_KEY = 0x59,
    B_KEYON2 = 0x5A,
    B_KEYOFF2 = 0x5B,
    B_KEY2 = 0x5C,
    GetActorAngle = 0x5D,
    GetActorDistance = 0x5E,
    B_PTR = 0x5F,
    B_ANGLEA = 0x60,
    B_DISTANCEA = 0x61,
    B_SIN = 0x62,
    B_COS = 0x63,
    B_HAVE_ITEM = 0x64,
    B_BAFRAME = 0x65,
    GetAngle = 0x66,
    pad67 = 0x67,
    pad68 = 0x68,
    pad69 = 0x69,
    B_FRAME = 0x6A,
    IsInParty = 0x6B,
    B_SPS = 0x6C,
    AddToParty = 0x6D,
    GetCurrentMP = 0x6E,
    GetMaxMP = 0x6F,
    B_BGIID = 0x70,
    B_BGIFLOOR = 0x71,
    B_OBJSPECA = 0x78,
    B_SYSLIST = 0x79,
    B_SYSVAR = 0x7A,
    B_pad7b = 0x7B,
    B_PAD4 = 0x7C,
    Const16 = 0x7D,
    Const26 = 0x7E,
    End = 0x7F,
    B_VAR = 0xC0,
}

public enum StorageScope
{
    Global = 0x0,
    Map = 0x1,
    Instance = 0x2,
    Special = 0x3,
}

public enum StorageType
{
    SBit = 0x0,
    Bit = 0x1,
    Int24 = 0x2,
    UInt24 = 0x3,
    SByte = 0x4,
    Byte = 0x5,
    Int16 = 0x6,
    UInt16 = 0x7,
}

public enum SystemVariable
{
    Random = 0x0,
    CurrentCameraIndex = 0x1,
    IsMovementControlEnabled = 0x2,
    SoundSync = 0x3,
    CollisionAngle = 0x4,
    ObjectIndex = 0x5,
    GilCount = 0x6,
    StepCount = 0x7,
    DialogProgression = 0x8,
    DialogChoice = 0x9,
    MapJumpX = 0xA,
    MapJumpY = 0xB,
    ScreenActorX = 0xC,
    ScreenActorY = 0xD,
    FmvFrame = 0xE,
    FmvSync = 0xF,
    FrogsCount = 0x10,
    Timer = 0x11,
    TetraMasterLastResult = 0x12,
    TetraMasterCardNumber = 0x13,
    GameTime = 0x14,
    IsTimerEnabled = 0x15,
    SoundPosition = 0x16,
    SoundVolume = 0x17,
    BattleMapId = 0x18,
    IsBattleSequenceBusy = 0x19,
    IsBattleMode = 0x1A,
    BattleResult = 0x1B,
    BattleCommandId = 0x1C,
    BattleScriptId = 0x1D,
    BattlePhase = 0x1E,
    BattlePatternIndex = 0x1F,
    FieldMapId = 0xBF,
}

public static partial class Actors
{
    [Instruction(Opcode.ATTACH)]
    public static void AttachObject(Operand targetUid, Operand carrier, Operand attachmentPoint, int mask = -1) => EventScheduler.Execute(Opcode.ATTACH, targetUid, carrier, attachmentPoint);
    [Instruction(Opcode.ATTACHOFFSET)]
    public static void SetAttachmentOffset(Operand x, Operand z, Operand y, int mask = -1) => EventScheduler.Execute(Opcode.ATTACHOFFSET, x, z, y);
    [Instruction(Opcode.BGI)]
    public static void SetPathing(Operand pathing, int mask = -1) => EventScheduler.Execute(Opcode.BGI, pathing);
    [Instruction(Opcode.BGSMOVE)]
    public static void SlideTo3D(Operand x, Operand z, Operand y, int mask = -1) => EventScheduler.Execute(Opcode.BGSMOVE, x, z, y);
    [Instruction(Opcode.CFLAG)]
    public static void SetObjectFlags(Operand flags, int mask = -1) => EventScheduler.Execute(Opcode.CFLAG, flags);
    [Instruction(Opcode.CHRCLEAR)]
    public static void RemoveCharacterAttribute(Operand attributes, int mask = -1) => EventScheduler.Execute(Opcode.CHRCLEAR, attributes);
    [Instruction(Opcode.CHRCOLOR)]
    public static EventAwaitable SetColor(Operand targetUid, Operand red, Operand green, Operand blue, int mask = -1) => EventScheduler.ExecuteAsync(Opcode.CHRCOLOR, targetUid, red, green, blue);
    [Instruction(Opcode.CHRFX)]
    public static void RunModelCode(Operand modelCode, Operand argument1, Operand argument2, Operand argument3, int mask = -1) => EventScheduler.Execute(Opcode.CHRFX, modelCode, argument1, argument2, argument3);
    [Instruction(Opcode.CHRSCALE)]
    public static void SetObjectSize(Operand targetUid, Operand sizeX, Operand sizeZ, Operand sizeY, int mask = -1) => EventScheduler.Execute(Opcode.CHRSCALE, targetUid, sizeX, sizeZ, sizeY);
    [Instruction(Opcode.CHRSET)]
    public static void AddCharacterAttribute(Operand attributes, int mask = -1) => EventScheduler.Execute(Opcode.CHRSET, attributes);
    [Instruction(Opcode.CLRDIST)]
    public static void BeginWalk() => EventScheduler.Execute(Opcode.CLRDIST);
    [Instruction(Opcode.DDIR)]
    public static void TurnInstantEx(Operand entry, Operand angle, int mask = -1) => EventScheduler.Execute(Opcode.DDIR, entry, angle);
    [Instruction(Opcode.DETACH)]
    public static void DetachObject(Operand targetUid, int mask = -1) => EventScheduler.Execute(Opcode.DETACH, targetUid);
    [Instruction(Opcode.DIRE)]
    public static void TurnInstant(Operand angle, int mask = -1) => EventScheduler.Execute(Opcode.DIRE, angle);
    [Instruction(Opcode.DPOS)]
    public static void SetActorPosition(Operand targetUid, Operand x, Operand z, int mask = -1) => EventScheduler.Execute(Opcode.DPOS, targetUid, x, z);
    [Instruction(Opcode.DPOS3)]
    public static void SetActorPosition3D(Operand targetUid, Operand x, Operand z, Operand y, int mask = -1) => EventScheduler.Execute(Opcode.DPOS3, targetUid, x, z, y);
    [Instruction(Opcode.DTURN)]
    public static void TurnActorAtSpeed(Operand targetUid, Operand angle, Operand speed, int mask = -1) => EventScheduler.Execute(Opcode.DTURN, targetUid, angle, speed);
    [Instruction(Opcode.DWAITTURN)]
    public static EventAwaitable WaitForActorTurn(Operand targetUid, int mask = -1) => EventScheduler.ExecuteAsync(Opcode.DWAITTURN, targetUid);
    [Instruction(Opcode.GETSCREEN)]
    public static void CalculateScreenPosition(Operand targetUid, int mask = -1) => EventScheduler.Execute(Opcode.GETSCREEN, targetUid);
    [Instruction(Opcode.JUMP3)]
    public static EventAwaitable Jump() => EventScheduler.ExecuteAsync(Opcode.JUMP3);
    [Instruction(Opcode.MESHHIDE)]
    public static void HideMeshes(Operand targetUid, Operand meshMask, int mask = -1) => EventScheduler.Execute(Opcode.MESHHIDE, targetUid, meshMask);
    [Instruction(Opcode.MESHSHOW)]
    public static void ShowMeshes(Operand targetUid, Operand meshMask, int mask = -1) => EventScheduler.Execute(Opcode.MESHSHOW, targetUid, meshMask);
    [Instruction(Opcode.MJPOS)]
    public static void CalculateExitPosition() => EventScheduler.Execute(Opcode.MJPOS);
    [Instruction(Opcode.MODEL)]
    public static void SetModel(Operand model, Operand height, int mask = -1) => EventScheduler.Execute(Opcode.MODEL, model, height);
    [Instruction(Opcode.MOVA)]
    public static EventAwaitable WalkTowardObject(Operand targetUid, int mask = -1) => EventScheduler.ExecuteAsync(Opcode.MOVA, targetUid);
    [Instruction(Opcode.MOVE)]
    public static EventAwaitable WalkTo(Operand x, Operand z, int mask = -1) => EventScheduler.ExecuteAsync(Opcode.MOVE, x, z);
    [Instruction(Opcode.MOVE3)]
    public static EventAwaitable WalkTo3D(Operand x, Operand z, Operand y, int mask = -1) => EventScheduler.ExecuteAsync(Opcode.MOVE3, x, z, y);
    [Instruction(Opcode.MOVE3H)]
    public static EventAwaitable SideStepTo3D(Operand x, Operand z, Operand y, int mask = -1) => EventScheduler.ExecuteAsync(Opcode.MOVE3H, x, z, y);
    [Instruction(Opcode.MOVH)]
    public static EventAwaitable SlideTo(Operand x, Operand z, int mask = -1) => EventScheduler.ExecuteAsync(Opcode.MOVH, x, z);
    [Instruction(Opcode.MOVJ)]
    public static EventAwaitable WalkToExit() => EventScheduler.ExecuteAsync(Opcode.MOVJ);
    [Instruction(Opcode.MROT)]
    public static void SetWalkTurnSpeed(Operand turnSpeed, int mask = -1) => EventScheduler.Execute(Opcode.MROT, turnSpeed);
    [Instruction(Opcode.MSPEED)]
    public static void SetWalkSpeed(Operand speed, int mask = -1) => EventScheduler.Execute(Opcode.MSPEED, speed);
    [Instruction(Opcode.NECKFLAG)]
    public static void EnableHeadFocus(Operand flags, int mask = -1) => EventScheduler.Execute(Opcode.NECKFLAG, flags);
    [Instruction(Opcode.NECKID)]
    public static void SetHeadFocusMask(Operand selfMask, Operand targetMask, int mask = -1) => EventScheduler.Execute(Opcode.NECKID, selfMask, targetMask);
    [Instruction(Opcode.OBJINDEX)]
    public static void SetObjectIndex(Operand index, int mask = -1) => EventScheduler.Execute(Opcode.OBJINDEX, index);
    [Instruction(Opcode.OVAL)]
    public static void SetObjectOvalRatio(Operand stretchingFactor, int mask = -1) => EventScheduler.Execute(Opcode.OVAL, stretchingFactor);
    [Instruction(Opcode.POPSHOW)]
    public static void ShowAllObjects() => EventScheduler.Execute(Opcode.POPSHOW);
    [Instruction(Opcode.POS)]
    public static void SetPosition(Operand x, Operand z, int mask = -1) => EventScheduler.Execute(Opcode.POS, x, z);
    [Instruction(Opcode.POS3)]
    public static void SetPosition3D(Operand x, Operand z, Operand y, int mask = -1) => EventScheduler.Execute(Opcode.POS3, x, z, y);
    [Instruction(Opcode.POSTJUMP)]
    public static void RunLandAnimation() => EventScheduler.Execute(Opcode.POSTJUMP);
    [Instruction(Opcode.PREJUMP)]
    public static void RunJumpAnimation() => EventScheduler.Execute(Opcode.PREJUMP);
    [Instruction(Opcode.PRETEND)]
    public static void PretendToBe(Operand entry, int mask = -1) => EventScheduler.Execute(Opcode.PRETEND, entry);
    [Instruction(Opcode.PUSHHIDE)]
    public static void HideAllObjects() => EventScheduler.Execute(Opcode.PUSHHIDE);
    [Instruction(Opcode.RADIUS)]
    public static void SetObjectLogicalSize(Operand walkRadius, Operand collisionRadius, Operand talkDistance, int mask = -1) => EventScheduler.Execute(Opcode.RADIUS, walkRadius, collisionRadius, talkDistance);
    [Instruction(Opcode.ROTXZ)]
    public static void SetPitchAngle(Operand pitch, Operand xzAngle, int mask = -1) => EventScheduler.Execute(Opcode.ROTXZ, pitch, xzAngle);
    [Instruction(Opcode.SETVY3)]
    public static void PrepareJump(Operand x, Operand z, Operand y, Operand steps, int mask = -1) => EventScheduler.Execute(Opcode.SETVY3, x, z, y, steps);
    [Instruction(Opcode.SPEEDTH)]
    public static void SetRunSpeedLimit(Operand speedLimit, int mask = -1) => EventScheduler.Execute(Opcode.SPEEDTH, speedLimit);
    [Instruction(Opcode.TSPEED)]
    public static void SetTurnSpeed(Operand speed, int mask = -1) => EventScheduler.Execute(Opcode.TSPEED, speed);
    [Instruction(Opcode.TURN)]
    public static void TurnAtSpeed(Operand angle, Operand speed, int mask = -1) => EventScheduler.Execute(Opcode.TURN, angle, speed);
    [Instruction(Opcode.TURNA)]
    public static void TurnTowardObject(Operand targetUid, Operand speed, int mask = -1) => EventScheduler.Execute(Opcode.TURNA, targetUid, speed);
    [Instruction(Opcode.TURNDS)]
    public static void Turn(Operand angle, int mask = -1) => EventScheduler.Execute(Opcode.TURNDS, angle);
    [Instruction(Opcode.TURNTO)]
    public static void TurnTowardPosition(Operand x, Operand z, int mask = -1) => EventScheduler.Execute(Opcode.TURNTO, x, z);
    [Instruction(Opcode.WAITTURN)]
    public static EventAwaitable WaitForTurn() => EventScheduler.ExecuteAsync(Opcode.WAITTURN);
}

public static partial class Legacy
{
    [Instruction(Opcode.DRADIUS)]
    public static void LegacyActorRadius(Operand targetUid, Operand parameter1, Operand parameter2, Operand parameter3, int mask = -1) => EventScheduler.Execute(Opcode.DRADIUS, targetUid, parameter1, parameter2, parameter3);
    [Instruction(Opcode.WATCH)]
    public static void WatchVariable() => EventScheduler.Execute(Opcode.WATCH);
    [Instruction(Opcode.DEBUGCC)]
    public static void DebugControlCharacter() => EventScheduler.Execute(Opcode.DEBUGCC);
    [Instruction(Opcode.DEBUGLOAD)]
    public static void DebugLoad() => EventScheduler.Execute(Opcode.DEBUGLOAD);
    [Instruction(Opcode.DEBUGSAVE)]
    public static void DebugSave() => EventScheduler.Execute(Opcode.DEBUGSAVE);
    [Instruction(Opcode.GLOBALCLEAR)]
    public static void ClearGlobals() => EventScheduler.Execute(Opcode.GLOBALCLEAR);
    [Instruction(Opcode.LOCATE)]
    public static void LegacyLocate(Operand parameter1, Operand parameter2, int mask = -1) => EventScheduler.Execute(Opcode.LOCATE, parameter1, parameter2);
    [Instruction(Opcode.MAPID)]
    public static void LegacyMapid() => EventScheduler.Execute(Opcode.MAPID);
    [Instruction(Opcode.PPRINT)]
    public static void PrintParallel() => EventScheduler.Execute(Opcode.PPRINT);
    [Instruction(Opcode.PPRINTF)]
    public static void PrintParallelFormatted() => EventScheduler.Execute(Opcode.PPRINTF);
    [Instruction(Opcode.PRINT1)]
    public static void Print1() => EventScheduler.Execute(Opcode.PRINT1);
    [Instruction(Opcode.PRINTF)]
    public static void PrintFormatted() => EventScheduler.Execute(Opcode.PRINTF);
    [Instruction(Opcode.pad0a)]
    public static void Reserved0A() => EventScheduler.Execute(Opcode.pad0a);
    [Instruction(Opcode.pad0e)]
    public static void Reserved0E() => EventScheduler.Execute(Opcode.pad0e);
    [Instruction(Opcode.pad0f)]
    public static void Reserved0F() => EventScheduler.Execute(Opcode.pad0f);
    [Instruction(Opcode.pad11)]
    public static void Reserved11() => EventScheduler.Execute(Opcode.pad11);
    [Instruction(Opcode.pad13)]
    public static void Reserved13() => EventScheduler.Execute(Opcode.pad13);
    [Instruction(Opcode.pad15)]
    public static void Reserved15() => EventScheduler.Execute(Opcode.pad15);
    [Instruction(Opcode.pad17)]
    public static void Reserved17() => EventScheduler.Execute(Opcode.pad17);
    [Instruction(Opcode.pad19)]
    public static void Reserved19() => EventScheduler.Execute(Opcode.pad19);
    [Instruction(Opcode.rsv0c)]
    public static void Reserved0C() => EventScheduler.Execute(Opcode.rsv0c);
    [Instruction(Opcode.PRINTQUAD)]
    public static void PrintQuad() => EventScheduler.Execute(Opcode.PRINTQUAD);
}

public static partial class Scripts
{
    [Instruction(Opcode.STOP)]
    public static void StopExecution(Operand parameter, int mask = -1) => EventScheduler.Execute(Opcode.STOP, parameter);
    [Instruction(Opcode.DELETE)]
    public static void DeleteObject(Operand uid, int mask = -1) => EventScheduler.Execute(Opcode.DELETE, uid);
    [Instruction(Opcode.DRET)]
    public static EventAwaitable ReturnEntryFunctions(Operand entry, int mask = -1) => EventScheduler.ExecuteAsync(Opcode.DRET, entry);
    [Instruction(Opcode.ENDSEQ)]
    public static void StopSharedScript() => EventScheduler.Execute(Opcode.ENDSEQ);
    [Instruction(Opcode.NEW)]
    public static void StartThread(Operand objectSlot, Operand uniqueId) => EventScheduler.Execute(Opcode.NEW, objectSlot, uniqueId);
    [Instruction(Opcode.NEW2)]
    public static void StartRegion(Operand objectSlot, Operand uniqueId) => EventScheduler.Execute(Opcode.NEW2, objectSlot, uniqueId);
    [Instruction(Opcode.NEW3)]
    public static void StartActor(Operand objectSlot, Operand uniqueId) => EventScheduler.Execute(Opcode.NEW3, objectSlot, uniqueId);
    [Instruction(Opcode.REPLY)]
    public static void ReplyAsync(Operand level, Operand scriptId, int mask = -1) => EventScheduler.Execute(Opcode.REPLY, level, scriptId);
    [Instruction(Opcode.REPLYEW)]
    public static EventAwaitable ReplyAndWait(Operand level, Operand scriptId, int mask = -1) => EventScheduler.ExecuteAsync(Opcode.REPLYEW, level, scriptId);
    [Instruction(Opcode.REPLYSW)]
    public static EventAwaitable ReplyWhenReady(Operand level, Operand scriptId, int mask = -1) => EventScheduler.ExecuteAsync(Opcode.REPLYSW, level, scriptId);
    [Instruction(Opcode.REQ)]
    public static void RequestAsync(Operand level, Operand targetUid, Operand scriptId, int mask = -1) => EventScheduler.Execute(Opcode.REQ, level, targetUid, scriptId);
    [Instruction(Opcode.REQEW)]
    public static EventAwaitable RequestAndWait(Operand level, Operand targetUid, Operand scriptId, int mask = -1) => EventScheduler.ExecuteAsync(Opcode.REQEW, level, targetUid, scriptId);
    [Instruction(Opcode.REQSW)]
    public static EventAwaitable RequestWhenReady(Operand level, Operand targetUid, Operand scriptId, int mask = -1) => EventScheduler.ExecuteAsync(Opcode.REQSW, level, targetUid, scriptId);
    [Instruction(Opcode.Return)]
    public static void ReturnFromScript() => EventScheduler.Execute(Opcode.Return);
    [Instruction(Opcode.STARTSEQ)]
    public static void RunSharedScript(Operand entry, int mask = -1) => EventScheduler.Execute(Opcode.STARTSEQ, entry);
    [Instruction(Opcode.WAITSEQ)]
    public static EventAwaitable WaitSharedScript() => EventScheduler.ExecuteAsync(Opcode.WAITSEQ);
}

public static partial class Animations
{
    [Instruction(Opcode.AFRAME)]
    public static void SetFrameRange(Operand firstFrame, Operand lastFrame, int mask = -1) => EventScheduler.Execute(Opcode.AFRAME, firstFrame, lastFrame);
    [Instruction(Opcode.AIDLE)]
    public static void SetIdle(Operand animation, int mask = -1) => EventScheduler.Execute(Opcode.AIDLE, animation);
    [Instruction(Opcode.AJUMP)]
    public static void SetJumpAnimation(Operand animation, Operand parameter2, Operand unknown, int mask = -1) => EventScheduler.Execute(Opcode.AJUMP, animation, parameter2, unknown);
    [Instruction(Opcode.AMODE)]
    public static void SetAnimationFlags(Operand flag, Operand repeat, int mask = -1) => EventScheduler.Execute(Opcode.AMODE, flag, repeat);
    [Instruction(Opcode.ANIM)]
    public static void Play(Operand animation, int mask = -1) => EventScheduler.Execute(Opcode.ANIM, animation);
    [Instruction(Opcode.ARUN)]
    public static void SetRun(Operand animation, int mask = -1) => EventScheduler.Execute(Opcode.ARUN, animation);
    [Instruction(Opcode.ASLEEP)]
    public static void SetSleep(Operand animation, int mask = -1) => EventScheduler.Execute(Opcode.ASLEEP, animation);
    [Instruction(Opcode.ASPEED)]
    public static void SetAnimationSpeed(Operand speed, int mask = -1) => EventScheduler.Execute(Opcode.ASPEED, speed);
    [Instruction(Opcode.ATURNL)]
    public static void SetTurnLeft(Operand animation, int mask = -1) => EventScheduler.Execute(Opcode.ATURNL, animation);
    [Instruction(Opcode.ATURNR)]
    public static void SetTurnRight(Operand animation, int mask = -1) => EventScheduler.Execute(Opcode.ATURNR, animation);
    [Instruction(Opcode.AWALK)]
    public static void SetWalk(Operand animation, int mask = -1) => EventScheduler.Execute(Opcode.AWALK, animation);
    [Instruction(Opcode.DANIM)]
    public static void PlayForActor(Operand targetUid, Operand animation, int mask = -1) => EventScheduler.Execute(Opcode.DANIM, targetUid, animation);
    [Instruction(Opcode.DWAITANIM)]
    public static EventAwaitable WaitForActor(Operand targetUid, int mask = -1) => EventScheduler.ExecuteAsync(Opcode.DWAITANIM, targetUid);
    [Instruction(Opcode.ENDANIM)]
    public static void Stop() => EventScheduler.Execute(Opcode.ENDANIM);
    [Instruction(Opcode.IDLESPEED)]
    public static void SetIdleSpeeds(Operand speed1, Operand speed2, Operand speed3, Operand speed4, int mask = -1) => EventScheduler.Execute(Opcode.IDLESPEED, speed1, speed2, speed3, speed4);
    [Instruction(Opcode.MOVT)]
    public static void MakeAnimationLoop(Operand amount, int mask = -1) => EventScheduler.Execute(Opcode.MOVT, amount);
    [Instruction(Opcode.SLEEPINH)]
    public static void DisableInactiveAnimation() => EventScheduler.Execute(Opcode.SLEEPINH);
    [Instruction(Opcode.SLEEPON)]
    public static void EnableInactiveAnimation() => EventScheduler.Execute(Opcode.SLEEPON);
    [Instruction(Opcode.TEXPLAY)]
    public static void EnableTextureAnimation(Operand targetUid, Operand textureAnimation, int mask = -1) => EventScheduler.Execute(Opcode.TEXPLAY, targetUid, textureAnimation);
    [Instruction(Opcode.TEXPLAY1)]
    public static void RunTextureAnimation(Operand targetUid, Operand textureAnimation, int mask = -1) => EventScheduler.Execute(Opcode.TEXPLAY1, targetUid, textureAnimation);
    [Instruction(Opcode.TEXSTOP)]
    public static void StopTextureAnimation(Operand targetUid, Operand textureAnimation, int mask = -1) => EventScheduler.Execute(Opcode.TEXSTOP, targetUid, textureAnimation);
    [Instruction(Opcode.WAITANIM)]
    public static EventAwaitable WaitForCompletion() => EventScheduler.ExecuteAsync(Opcode.WAITANIM);
}

public static partial class Audio
{
    [Instruction(Opcode.FLDSND0)]
    public static void DispatchSound(Operand command, Operand sound, int mask = -1) => EventScheduler.Execute(Opcode.FLDSND0, command, sound);
    [Instruction(Opcode.FLDSND1)]
    public static void DispatchSound1(Operand command, Operand sound, Operand parameter1, int mask = -1) => EventScheduler.Execute(Opcode.FLDSND1, command, sound, parameter1);
    [Instruction(Opcode.FLDSND2)]
    public static void DispatchSound2(Operand command, Operand sound, Operand parameter1, Operand parameter2, int mask = -1) => EventScheduler.Execute(Opcode.FLDSND2, command, sound, parameter1, parameter2);
    [Instruction(Opcode.FLDSND3)]
    public static void DispatchSound3(Operand command, Operand sound, Operand parameter1, Operand parameter2, Operand parameter3, int mask = -1) => EventScheduler.Execute(Opcode.FLDSND3, command, sound, parameter1, parameter2, parameter3);
    [Instruction(Opcode.SEPV)]
    public static void SetSoundPosition(Operand x, Operand z, Operand y, Operand volume, int mask = -1) => EventScheduler.Execute(Opcode.SEPV, x, z, y, volume);
    [Instruction(Opcode.SEPVA)]
    public static void SetSoundObjectPosition(Operand targetUid, Operand volume, int mask = -1) => EventScheduler.Execute(Opcode.SEPVA, targetUid, volume);
    [Instruction(Opcode.SONGFLAG)]
    public static void ContinueBattleMusic(Operand continueValue, int mask = -1) => EventScheduler.Execute(Opcode.SONGFLAG, continueValue);
    [Instruction(Opcode.FLDSND0, fixedIndex: 0, fixedValue: 0)]
    public static void PlaySong(Operand sound, int mask = -1) => EventScheduler.Execute(Opcode.FLDSND0, 0, sound);
    [Instruction(Opcode.FLDSND0, fixedIndex: 0, fixedValue: 256)]
    public static void StopSong(Operand sound, int mask = -1) => EventScheduler.Execute(Opcode.FLDSND0, 256, sound);
    [Instruction(Opcode.FLDSND0, fixedIndex: 0, fixedValue: 265)]
    public static void StopCurrentSong(Operand sound, int mask = -1) => EventScheduler.Execute(Opcode.FLDSND0, 265, sound);
    [Instruction(Opcode.FLDSND0, fixedIndex: 0, fixedValue: 1792)]
    public static void LoadSong(Operand sound, int mask = -1) => EventScheduler.Execute(Opcode.FLDSND0, 1792, sound);
    [Instruction(Opcode.FLDSND0, fixedIndex: 0, fixedValue: 2048)]
    public static void SuspendSong(Operand sound, int mask = -1) => EventScheduler.Execute(Opcode.FLDSND0, 2048, sound);
    [Instruction(Opcode.FLDSND0, fixedIndex: 0, fixedValue: 2304)]
    public static void ResumeSong(Operand sound, int mask = -1) => EventScheduler.Execute(Opcode.FLDSND0, 2304, sound);
    [Instruction(Opcode.FLDSND0, fixedIndex: 0, fixedValue: 2566)]
    public static void MarkSongJumpPoint(Operand sound, int mask = -1) => EventScheduler.Execute(Opcode.FLDSND0, 2566, sound);
    [Instruction(Opcode.FLDSND1, fixedIndex: 0, fixedValue: 16903)]
    public static void SkipSongPhrase(Operand sound, Operand phrase, int mask = -1) => EventScheduler.Execute(Opcode.FLDSND1, 16903, sound, phrase);
    [Instruction(Opcode.FLDSND1, fixedIndex: 0, fixedValue: 16897)]
    public static void SetSongVolume(Operand sound, Operand volume, int mask = -1) => EventScheduler.Execute(Opcode.FLDSND1, 16897, sound, volume);
    [Instruction(Opcode.FLDSND2, fixedIndex: 0, fixedValue: 33537)]
    public static void InterpolateSongVolume(Operand sound, Operand ticks, Operand volume, int mask = -1) => EventScheduler.Execute(Opcode.FLDSND2, 33537, sound, ticks, volume);
    [Instruction(Opcode.FLDSND3, fixedIndex: 0, fixedValue: 50177)]
    public static void FadeSongVolume(Operand sound, Operand ticks, Operand fromVolume, Operand toVolume, int mask = -1) => EventScheduler.Execute(Opcode.FLDSND3, 50177, sound, ticks, fromVolume, toVolume);
    [Instruction(Opcode.FLDSND2, fixedIndex: 0, fixedValue: 40960)]
    public static void PlayStream(Operand sound, Operand position, Operand reverberation, int mask = -1) => EventScheduler.Execute(Opcode.FLDSND2, 40960, sound, position, reverberation);
    [Instruction(Opcode.FLDSND0, fixedIndex: 0, fixedValue: 8448)]
    public static void StopStream(Operand sound, int mask = -1) => EventScheduler.Execute(Opcode.FLDSND0, 8448, sound);
    [Instruction(Opcode.FLDSND1, fixedIndex: 0, fixedValue: 25089)]
    public static void SetStreamVolume(Operand sound, Operand volume, int mask = -1) => EventScheduler.Execute(Opcode.FLDSND1, 25089, sound, volume);
    [Instruction(Opcode.FLDSND0, fixedIndex: 0, fixedValue: 4616)]
    public static void IgnoreSoundEffect(Operand sound, int mask = -1) => EventScheduler.Execute(Opcode.FLDSND0, 4616, sound);
    [Instruction(Opcode.FLDSND3, fixedIndex: 0, fixedValue: 53248)]
    public static void PlayEffect(Operand sound, Operand attributes, Operand position, Operand volume, int mask = -1) => EventScheduler.Execute(Opcode.FLDSND3, 53248, sound, attributes, position, volume);
    [Instruction(Opcode.FLDSND1, fixedIndex: 0, fixedValue: 20736)]
    public static void StopEffect(Operand sound, Operand attributes, int mask = -1) => EventScheduler.Execute(Opcode.FLDSND1, 20736, sound, attributes);
    [Instruction(Opcode.FLDSND2, fixedIndex: 0, fixedValue: 37377)]
    public static void SetEffectVolume(Operand sound, Operand parameter1, Operand volume, int mask = -1) => EventScheduler.Execute(Opcode.FLDSND2, 37377, sound, parameter1, volume);
    [Instruction(Opcode.FLDSND2, fixedIndex: 0, fixedValue: 54017)]
    public static void InterpolateEffectVolume(Operand sound, Operand ticks, Operand volume, int mask = -1) => EventScheduler.Execute(Opcode.FLDSND2, 54017, sound, ticks, volume);
    [Instruction(Opcode.FLDSND3, fixedIndex: 0, fixedValue: 53376)]
    public static void PlayResidentEffect(Operand sound, Operand timeMsec, Operand position, Operand volume, int mask = -1) => EventScheduler.Execute(Opcode.FLDSND3, 53376, sound, timeMsec, position, volume);
    [Instruction(Opcode.FLDSND3, fixedIndex: 0, fixedValue: 53440)]
    public static void PlayResidentEffectInSlot1(Operand sound, Operand timeMsec, Operand position, Operand volume, int mask = -1) => EventScheduler.Execute(Opcode.FLDSND3, 53440, sound, timeMsec, position, volume);
    [Instruction(Opcode.FLDSND3, fixedIndex: 0, fixedValue: 54145)]
    public static void InterpolateResidentEffectVolume(Operand sound, Operand attributes, Operand ticks, Operand volume, int mask = -1) => EventScheduler.Execute(Opcode.FLDSND3, 54145, sound, attributes, ticks, volume);
    [Instruction(Opcode.FLDSND3, fixedIndex: 0, fixedValue: 54209)]
    public static void InterpolateResidentEffectVolumeInSlot1(Operand sound, Operand attributes, Operand ticks, Operand volume, int mask = -1) => EventScheduler.Execute(Opcode.FLDSND3, 54209, sound, attributes, ticks, volume);
    [Instruction(Opcode.FLDSND1, fixedIndex: 0, fixedValue: 20864)]
    public static void StopResidentEffect(Operand sound, Operand timeMsec, int mask = -1) => EventScheduler.Execute(Opcode.FLDSND1, 20864, sound, timeMsec);
    [Instruction(Opcode.FLDSND1, fixedIndex: 0, fixedValue: 20928)]
    public static void StopResidentEffectInSlot1(Operand sound, Operand timeMsec, int mask = -1) => EventScheduler.Execute(Opcode.FLDSND1, 20928, sound, timeMsec);
}

public static partial class Battle
{
    [Instruction(Opcode.BEND)]
    public static void TerminateBattle() => EventScheduler.Execute(Opcode.BEND);
    [Instruction(Opcode.BTLCMD)]
    public static void Attack(Operand attack, int mask = -1) => EventScheduler.Execute(Opcode.BTLCMD, attack);
    [Instruction(Opcode.BTLSEQ)]
    public static void AttackSpecial(Operand attack, int mask = -1) => EventScheduler.Execute(Opcode.BTLSEQ, attack);
    [Instruction(Opcode.BTLSET)]
    public static void RunBattleCode(Operand code, Operand argument, int mask = -1) => EventScheduler.Execute(Opcode.BTLSET, code, argument);
    [Instruction(Opcode.ENCOUNT)]
    public static void StartEncounter(Operand rushType, Operand battle, int mask = -1) => EventScheduler.Execute(Opcode.ENCOUNT, rushType, battle);
    [Instruction(Opcode.ENCOUNT2)]
    public static void BattleEx(Operand rushType, Operand battleGroup, Operand battle, int mask = -1) => EventScheduler.Execute(Opcode.ENCOUNT2, rushType, battleGroup, battle);
    [Instruction(Opcode.ENCRATE)]
    public static void SetRandomBattleFrequency(Operand frequency, int mask = -1) => EventScheduler.Execute(Opcode.ENCRATE, frequency);
    [Instruction(Opcode.ENCSCENE)]
    public static void SetRandomBattles(Operand pattern, Operand battle1, Operand battle2, Operand battle3, Operand battle4, int mask = -1) => EventScheduler.Execute(Opcode.ENCSCENE, pattern, battle1, battle2, battle3, battle4);
}

public static partial class Camera
{
    [Instruction(Opcode.AUTOTURN)]
    public static void FollowFocus(Operand enable, int mask = -1) => EventScheduler.Execute(Opcode.AUTOTURN, enable);
    [Instruction(Opcode.BGCACTIVE)]
    public static void EnableCameraServices(Operand enable, Operand time, Operand smoothness, int mask = -1) => EventScheduler.Execute(Opcode.BGCACTIVE, enable, time, smoothness);
    [Instruction(Opcode.BGCHEIGHT)]
    public static void SetCameraFollowHeight(Operand height, int mask = -1) => EventScheduler.Execute(Opcode.BGCHEIGHT, height);
    [Instruction(Opcode.BGCLOCK)]
    public static void EnableCameraFollow() => EventScheduler.Execute(Opcode.BGCLOCK);
    [Instruction(Opcode.BGCUNLOCK)]
    public static void DisableCameraFollow() => EventScheduler.Execute(Opcode.BGCUNLOCK);
    [Instruction(Opcode.BGSRELEASE)]
    public static void ReleaseCamera(Operand time, Operand smoothness, int mask = -1) => EventScheduler.Execute(Opcode.BGSRELEASE, time, smoothness);
    [Instruction(Opcode.BGSSCROLL)]
    public static void ScrollTo(Operand x, Operand y, Operand frames, Operand smoothness, int mask = -1) => EventScheduler.Execute(Opcode.BGSSCROLL, x, y, frames, smoothness);
    [Instruction(Opcode.BGVPORT)]
    public static void SetCameraBounds(Operand camera, Operand minX, Operand maxX, Operand minY, Operand maxY, int mask = -1) => EventScheduler.Execute(Opcode.BGVPORT, camera, minX, maxX, minY, maxY);
    [Instruction(Opcode.SETCAM)]
    public static void SetFieldCamera(Operand cameraId, int mask = -1) => EventScheduler.Execute(Opcode.SETCAM, cameraId);
    [Instruction(Opcode.VRP)]
    public static void CalculateScreenOrigin() => EventScheduler.Execute(Opcode.VRP);
}

public static partial class Clock
{
    [Instruction(Opcode.TIMERCONTROL)]
    public static void RunTimer(Operand run, int mask = -1) => EventScheduler.Execute(Opcode.TIMERCONTROL, run);
    [Instruction(Opcode.TIMERDISPLAY)]
    public static void ShowTimer(Operand enable, int mask = -1) => EventScheduler.Execute(Opcode.TIMERDISPLAY, enable);
    [Instruction(Opcode.TIMERSET)]
    public static void ChangeTimerTime(Operand time, int mask = -1) => EventScheduler.Execute(Opcode.TIMERSET, time);
}

public static partial class Engine
{
    [Instruction(Opcode.CC)]
    public static void DefinePlayerCharacter() => EventScheduler.Execute(Opcode.CC);
    [Instruction(Opcode.DASHOFF)]
    public static void DisableRun() => EventScheduler.Execute(Opcode.DASHOFF);
    [Instruction(Opcode.DASHON)]
    public static void EnableRun() => EventScheduler.Execute(Opcode.DASHON);
    [Instruction(Opcode.GAMEOVER)]
    public static void GameOver() => EventScheduler.Execute(Opcode.GAMEOVER);
    [Instruction(Opcode.MENU)]
    public static EventAwaitable Menu(Operand menuType, Operand menu, int mask = -1) => EventScheduler.ExecuteAsync(Opcode.MENU, menuType, menu);
    [Instruction(Opcode.MENUOFF)]
    public static EventAwaitable DisableMenu() => EventScheduler.ExecuteAsync(Opcode.MENUOFF);
    [Instruction(Opcode.MENUON)]
    public static void EnableMenu() => EventScheduler.Execute(Opcode.MENUON);
    [Instruction(Opcode.NOP)]
    public static EventAwaitable YieldFrame() => EventScheduler.ExecuteAsync(Opcode.NOP);
    [Instruction(Opcode.UCOFF)]
    public static void DisableMove() => EventScheduler.Execute(Opcode.UCOFF);
    [Instruction(Opcode.UCON)]
    public static void EnableMove() => EventScheduler.Execute(Opcode.UCON);
    [Instruction(Opcode.WAIT)]
    public static EventAwaitable Wait(Operand frames, int mask = -1) => EventScheduler.ExecuteAsync(Opcode.WAIT, frames);
}

public static partial class Events
{
    [Instruction(Opcode.AICON)]
    public static void ControlActiveTimeEvent(Operand mode, int mask = -1) => EventScheduler.Execute(Opcode.AICON, mode);
}

public static partial class Field
{
    [Instruction(Opcode.DISCCHANGE)]
    public static EventAwaitable ChangeDisc(Operand destination, int mask = -1) => EventScheduler.ExecuteAsync(Opcode.DISCCHANGE, destination);
    [Instruction(Opcode.HINT)]
    public static void PreloadField(Operand unknown, Operand field, int mask = -1) => EventScheduler.Execute(Opcode.HINT, unknown, field);
    [Instruction(Opcode.MAPJUMP)]
    public static void ChangeMap(Operand fieldId, int mask = -1) => EventScheduler.Execute(Opcode.MAPJUMP, fieldId);
    [Instruction(Opcode.MOVQ)]
    public static void ExitField() => EventScheduler.Execute(Opcode.MOVQ);
    [Instruction(Opcode.RAIN)]
    public static EventAwaitable SetWeather(Operand strength, Operand speed, int mask = -1) => EventScheduler.ExecuteAsync(Opcode.RAIN, strength, speed);
    [Instruction(Opcode.RESETMAPNAME)]
    public static void ResetFieldName() => EventScheduler.Execute(Opcode.RESETMAPNAME);
    [Instruction(Opcode.SETMAPNAME)]
    public static void SetFieldName(Operand name, int mask = -1) => EventScheduler.Execute(Opcode.SETMAPNAME, name);
}

public static partial class Input
{
    [Instruction(Opcode.CLEARKEYMASK)]
    public static void RemoveControllerMask(Operand pad, Operand buttons, int mask = -1) => EventScheduler.Execute(Opcode.CLEARKEYMASK, pad, buttons);
    [Instruction(Opcode.FICON)]
    public static void Bubble(Operand symbol, int mask = -1) => EventScheduler.Execute(Opcode.FICON, symbol);
    [Instruction(Opcode.HEREON)]
    public static void ShowHereIcon(Operand show, int mask = -1) => EventScheduler.Execute(Opcode.HEREON, show);
    [Instruction(Opcode.SETKEYMASK)]
    public static void AddControllerMask(Operand pad, Operand buttons, int mask = -1) => EventScheduler.Execute(Opcode.SETKEYMASK, pad, buttons);
    [Instruction(Opcode.TWIST)]
    public static void SetControlDirection(Operand arrowAngle, Operand analogicAngle, int mask = -1) => EventScheduler.Execute(Opcode.TWIST, arrowAngle, analogicAngle);
}

public static partial class Inventory
{
    [Instruction(Opcode.DELETEALLCARD)]
    public static void DeleteAllCards() => EventScheduler.Execute(Opcode.DELETEALLCARD);
    [Instruction(Opcode.GILADD)]
    public static void AddGil(Operand amount, int mask = -1) => EventScheduler.Execute(Opcode.GILADD, amount);
    [Instruction(Opcode.GILDELETE)]
    public static void RemoveGil(Operand amount, int mask = -1) => EventScheduler.Execute(Opcode.GILDELETE, amount);
    [Instruction(Opcode.ITEMADD)]
    public static void AddItem(Operand item, Operand amount, int mask = -1) => EventScheduler.Execute(Opcode.ITEMADD, item, amount);
    [Instruction(Opcode.ITEMDELETE)]
    public static void RemoveItem(Operand item, Operand amount, int mask = -1) => EventScheduler.Execute(Opcode.ITEMDELETE, item, amount);
    [Instruction(Opcode.MINIGAME)]
    public static void TetraMaster(Operand cardDeck, int mask = -1) => EventScheduler.Execute(Opcode.MINIGAME, cardDeck);
}

public static partial class Messages
{
    [Instruction(Opcode.CHOOSEPARAM)]
    public static void EnableDialogChoices(Operand choiceList, Operand defaultChoice, int mask = -1) => EventScheduler.Execute(Opcode.CHOOSEPARAM, choiceList, defaultChoice);
    [Instruction(Opcode.CLOSE)]
    public static EventAwaitable CloseWindow(Operand windowId, int mask = -1) => EventScheduler.ExecuteAsync(Opcode.CLOSE, windowId);
    [Instruction(Opcode.CLOSEALL)]
    public static void CloseAllWindows() => EventScheduler.Execute(Opcode.CLOSEALL);
    [Instruction(Opcode.MES)]
    public static EventAwaitable ShowAndWait(Operand windowId, Operand flags, Operand messageId, int mask = -1) => EventScheduler.ExecuteAsync(Opcode.MES, windowId, flags, messageId);
    [Instruction(Opcode.MESA)]
    public static EventAwaitable ShowForActorAndWait(Operand speakerUid, Operand windowId, Operand flags, Operand messageId, int mask = -1) => EventScheduler.ExecuteAsync(Opcode.MESA, speakerUid, windowId, flags, messageId);
    [Instruction(Opcode.MESAN)]
    public static EventAwaitable ShowForActor(Operand speakerUid, Operand windowId, Operand flags, Operand messageId, int mask = -1) => EventScheduler.ExecuteAsync(Opcode.MESAN, speakerUid, windowId, flags, messageId);
    [Instruction(Opcode.MESB)]
    public static EventAwaitable BattleDialog(Operand text, int mask = -1) => EventScheduler.ExecuteAsync(Opcode.MESB, text);
    [Instruction(Opcode.MESN)]
    public static EventAwaitable Show(Operand windowId, Operand flags, Operand messageId, int mask = -1) => EventScheduler.ExecuteAsync(Opcode.MESN, windowId, flags, messageId);
    [Instruction(Opcode.MESVALUE)]
    public static void SetTextVariable(Operand variableId, Operand value, int mask = -1) => EventScheduler.Execute(Opcode.MESVALUE, variableId, value);
    [Instruction(Opcode.NOINITMES)]
    public static void PreventWindowInit() => EventScheduler.Execute(Opcode.NOINITMES);
    [Instruction(Opcode.RAISE)]
    public static void RaiseWindows() => EventScheduler.Execute(Opcode.RAISE);
    [Instruction(Opcode.SETSIGNAL)]
    public static void SetDialogProgression(Operand progression, int mask = -1) => EventScheduler.Execute(Opcode.SETSIGNAL, progression);
    [Instruction(Opcode.WAITMES)]
    public static EventAwaitable WaitWindow(Operand windowId, int mask = -1) => EventScheduler.ExecuteAsync(Opcode.WAITMES, windowId);
}

public static partial class Party
{
    [Instruction(Opcode.CLEARAP)]
    public static void UnlearnAbility(Operand character, Operand ability, int mask = -1) => EventScheduler.Execute(Opcode.CLEARAP, character, ability);
    [Instruction(Opcode.CLEARSTATUS)]
    public static void CureStatus(Operand character, Operand statuses, int mask = -1) => EventScheduler.Execute(Opcode.CLEARSTATUS, character, statuses);
    [Instruction(Opcode.FULLMEMBER)]
    public static void SetPartyReserve(Operand charactersAvailable, int mask = -1) => EventScheduler.Execute(Opcode.FULLMEMBER, charactersAvailable);
    [Instruction(Opcode.INCFROG)]
    public static void AddFrog() => EventScheduler.Execute(Opcode.INCFROG);
    [Instruction(Opcode.JOIN)]
    public static void SetCharacterData(Operand character, Operand updateLevel, Operand equipmentSet, Operand category, Operand abilitySet, int mask = -1) => EventScheduler.Execute(Opcode.JOIN, character, updateLevel, equipmentSet, category, abilitySet);
    [Instruction(Opcode.MAXAP)]
    public static void LearnAbility(Operand character, Operand ability, int mask = -1) => EventScheduler.Execute(Opcode.MAXAP, character, ability);
    [Instruction(Opcode.PARTYDELETE)]
    public static void RemoveParty(Operand character, int mask = -1) => EventScheduler.Execute(Opcode.PARTYDELETE, character);
    [Instruction(Opcode.PARTYMENU)]
    public static EventAwaitable ChooseMembers(Operand partySize, Operand lockedCharacters, int mask = -1) => EventScheduler.ExecuteAsync(Opcode.PARTYMENU, partySize, lockedCharacters);
    [Instruction(Opcode.PLAYERNAME)]
    public static void SetName(Operand character, Operand text, int mask = -1) => EventScheduler.Execute(Opcode.PLAYERNAME, character, text);
    [Instruction(Opcode.SETHP)]
    public static void SetHP(Operand character, Operand hp, int mask = -1) => EventScheduler.Execute(Opcode.SETHP, character, hp);
    [Instruction(Opcode.SETMP)]
    public static void SetMP(Operand character, Operand mp, int mask = -1) => EventScheduler.Execute(Opcode.SETMP, character, mp);
    [Instruction(Opcode.SETROW)]
    public static void SetRow(Operand character, Operand row, int mask = -1) => EventScheduler.Execute(Opcode.SETROW, character, row);
    [Instruction(Opcode.SYNCPARTY)]
    public static void UpdatePartyUID() => EventScheduler.Execute(Opcode.SYNCPARTY);
    [Instruction(Opcode.WINPOSE)]
    public static void EnableVictoryPose(Operand character, Operand enable, int mask = -1) => EventScheduler.Execute(Opcode.WINPOSE, character, enable);
}

public static partial class Regions
{
    [Instruction((Opcode)0x100)]
    public static void LegacySimulationStart(Operand parameter1, Operand parameter2, int mask = -1) => EventScheduler.Execute((Opcode)0x100, parameter1, parameter2);
    [Instruction((Opcode)0x101)]
    public static void SetLegacyWalkmeshFrame(Operand parameter1, Operand parameter2, int mask = -1) => EventScheduler.Execute((Opcode)0x101, parameter1, parameter2);
    [Instruction((Opcode)0x102)]
    public static void SetSimulationActive(Operand simulationId, Operand enabled, int mask = -1) => EventScheduler.Execute((Opcode)0x102, simulationId, enabled);
    [Instruction((Opcode)0x103)]
    public static void SetSimulationFlags(Operand simulationId, Operand flags, int mask = -1) => EventScheduler.Execute((Opcode)0x103, simulationId, flags);
    [Instruction((Opcode)0x104)]
    public static void SetSimulationFloor(Operand simulationId, Operand floorId, int mask = -1) => EventScheduler.Execute((Opcode)0x104, simulationId, floorId);
    [Instruction((Opcode)0x105)]
    public static void SetSimulationRate(Operand simulationId, Operand frameRate, int mask = -1) => EventScheduler.Execute((Opcode)0x105, simulationId, frameRate);
    [Instruction((Opcode)0x106)]
    public static void SetSimulationAlgorithm(Operand simulationId, Operand algorithm, int mask = -1) => EventScheduler.Execute((Opcode)0x106, simulationId, algorithm);
    [Instruction((Opcode)0x107)]
    public static void SetSimulationDelta(Operand simulationId, Operand minimum, Operand maximum, int mask = -1) => EventScheduler.Execute((Opcode)0x107, simulationId, minimum, maximum);
    [Instruction((Opcode)0x108)]
    public static void SetSimulationAxis(Operand simulationId, Operand axis, int mask = -1) => EventScheduler.Execute((Opcode)0x108, simulationId, axis);
    [Instruction((Opcode)0x109)]
    public static void StartLegacyWalkmeshAnimation(Operand animationId, Operand parameter, int mask = -1) => EventScheduler.Execute((Opcode)0x109, animationId, parameter);
    [Instruction((Opcode)0x10A)]
    public static void SetWalkmeshAnimationFrame(Operand animationId, Operand frame, int mask = -1) => EventScheduler.Execute((Opcode)0x10A, animationId, frame);
    [Instruction((Opcode)0x10B)]
    public static void SetLegacyWalkmeshAnimationActive(Operand animationId, Operand enabled, int mask = -1) => EventScheduler.Execute((Opcode)0x10B, animationId, enabled);
    [Instruction((Opcode)0x10C)]
    public static void SetLegacyWalkmeshAnimationFlags(Operand animationId, Operand flags, int mask = -1) => EventScheduler.Execute((Opcode)0x10C, animationId, flags);
    [Instruction((Opcode)0x10D)]
    public static void SetLegacyWalkmeshAnimationRate(Operand animationId, Operand frameRate, int mask = -1) => EventScheduler.Execute((Opcode)0x10D, animationId, frameRate);
    [Instruction((Opcode)0x10E)]
    public static void WaitForLegacyWalkmeshAnimations(Operand parameter1, Operand parameter2, int mask = -1) => EventScheduler.Execute((Opcode)0x10E, parameter1, parameter2);
    [Instruction((Opcode)0x10F)]
    public static void WaitForLegacyWalkmeshAnimation(Operand animationId, Operand frame, Operand duration, int mask = -1) => EventScheduler.Execute((Opcode)0x10F, animationId, frame, duration);
    [Instruction((Opcode)0x110)]
    public static void SetLegacyWalkmeshAnimationRange(Operand animationId, Operand startFrame, Operand endFrame, int mask = -1) => EventScheduler.Execute((Opcode)0x110, animationId, startFrame, endFrame);
    [Instruction((Opcode)0x111)]
    public static void SetLegacyWalkmeshAnimationVisible(Operand animationId, Operand visible, int mask = -1) => EventScheduler.Execute((Opcode)0x111, animationId, visible);
    [Instruction(Opcode.BGIACTIVEF)]
    public static void EnablePath(Operand path, Operand enable, int mask = -1) => EventScheduler.Execute(Opcode.BGIACTIVEF, path, enable);
    [Instruction(Opcode.BGIACTIVET)]
    public static void EnablePathTriangle(Operand triangleId, Operand enable, int mask = -1) => EventScheduler.Execute(Opcode.BGIACTIVET, triangleId, enable);
    [Instruction(Opcode.BGIMASK)]
    public static void SetTriangleFlagMask(Operand flags, int mask = -1) => EventScheduler.Execute(Opcode.BGIMASK, flags);
    [Instruction(Opcode.TRACK)]
    public static void SetLastVertex(Operand x, Operand z, int mask = -1) => EventScheduler.Execute(Opcode.TRACK, x, z);
    [Instruction(Opcode.TRACKADD)]
    public static void DrawRegionPushNew() => EventScheduler.Execute(Opcode.TRACKADD);
    [Instruction(Opcode.TRACKSTART)]
    public static void BeginPolygon(Operand x, Operand z, int mask = -1) => EventScheduler.Execute(Opcode.TRACKSTART, x, z);
    [Instruction(Opcode.QUAD)]
    public static void DefinePolygon(Operand vertexCount, int mask, params Vertex[] vertices) => EventScheduler.DefinePolygon(vertexCount, vertices);
}

public static partial class Tiles
{
    [Instruction(Opcode.BGAACTIVE)]
    public static void ActivateTileAnimation(Operand tileAnimation, Operand activate, int mask = -1) => EventScheduler.Execute(Opcode.BGAACTIVE, tileAnimation, activate);
    [Instruction(Opcode.BGAANIME)]
    public static void RunTileAnimation(Operand fieldAnimation, Operand frame, int mask = -1) => EventScheduler.Execute(Opcode.BGAANIME, fieldAnimation, frame);
    [Instruction(Opcode.BGAFLAG)]
    public static void SetTileAnimationFlags(Operand tileAnimation, Operand flags, int mask = -1) => EventScheduler.Execute(Opcode.BGAFLAG, tileAnimation, flags);
    [Instruction(Opcode.BGAFRAME)]
    public static void SetTileAnimationFrame(Operand tileAnimation, Operand frameId, int mask = -1) => EventScheduler.Execute(Opcode.BGAFRAME, tileAnimation, frameId);
    [Instruction(Opcode.BGARANGE)]
    public static void RunTileAnimationEx(Operand tileAnimation, Operand start, Operand end, int mask = -1) => EventScheduler.Execute(Opcode.BGARANGE, tileAnimation, start, end);
    [Instruction(Opcode.BGARATE)]
    public static void SetTileAnimationSpeed(Operand tileAnimation, Operand speed, int mask = -1) => EventScheduler.Execute(Opcode.BGARATE, tileAnimation, speed);
    [Instruction(Opcode.BGAVISIBLE)]
    public static void ResetTileAnimation(Operand tileAnimation, Operand visible, int mask = -1) => EventScheduler.Execute(Opcode.BGAVISIBLE, tileAnimation, visible);
    [Instruction(Opcode.BGAWAIT)]
    public static void SetTileAnimationPause(Operand tileAnimation, Operand frameId, Operand time, int mask = -1) => EventScheduler.Execute(Opcode.BGAWAIT, tileAnimation, frameId, time);
    [Instruction(Opcode.BGLACTIVE)]
    public static void ShowTile(Operand tileBlock, Operand show, int mask = -1) => EventScheduler.Execute(Opcode.BGLACTIVE, tileBlock, show);
    [Instruction(Opcode.BGLATTACH)]
    public static void Attach(Operand tileBlock, Operand x, Operand y, Operand filterMode, Operand red, Operand green, Operand blue, int mask = -1) => EventScheduler.Execute(Opcode.BGLATTACH, tileBlock, x, y, filterMode, red, green, blue);
    [Instruction(Opcode.BGLCOLOR)]
    public static void SetColor(Operand tileBlock, Operand cyan, Operand magenta, Operand yellow, int mask = -1) => EventScheduler.Execute(Opcode.BGLCOLOR, tileBlock, cyan, magenta, yellow);
    [Instruction(Opcode.BGLLOOP)]
    public static void MoveTileLoop(Operand tileBlock, Operand activate, Operand xLoop, Operand yLoop, int mask = -1) => EventScheduler.Execute(Opcode.BGLLOOP, tileBlock, activate, xLoop, yLoop);
    [Instruction(Opcode.BGLLOOPTYPE)]
    public static void SetTileLoopType(Operand tileBlock, Operand screenAnchored, int mask = -1) => EventScheduler.Execute(Opcode.BGLLOOPTYPE, tileBlock, screenAnchored);
    [Instruction(Opcode.BGLMOVE)]
    public static void SetTilePositionEx(Operand tileBlock, Operand positionX, Operand positionY, Operand positionCloseness, int mask = -1) => EventScheduler.Execute(Opcode.BGLMOVE, tileBlock, positionX, positionY, positionCloseness);
    [Instruction(Opcode.BGLORIGIN)]
    public static void SetTilePosition(Operand tileBlock, Operand positionX, Operand positionY, int mask = -1) => EventScheduler.Execute(Opcode.BGLORIGIN, tileBlock, positionX, positionY);
    [Instruction(Opcode.BGLPARALLAX)]
    public static void MoveTile(Operand tileBlock, Operand activate, Operand movementX, Operand movementY, int mask = -1) => EventScheduler.Execute(Opcode.BGLPARALLAX, tileBlock, activate, movementX, movementY);
    [Instruction(Opcode.BGLSCROLLOFFSET)]
    public static void MoveTileLoopWithOffset(Operand tileBlock, Operand enable, Operand speed, Operand offset, Operand isXMovement, int mask = -1) => EventScheduler.Execute(Opcode.BGLSCROLLOFFSET, tileBlock, enable, speed, offset, isXMovement);
    [Instruction(Opcode.BGVALPHA)]
    public static void SetTileLoopAlpha(Operand tileBlock, Operand unknownX, Operand unknownY, int mask = -1) => EventScheduler.Execute(Opcode.BGVALPHA, tileBlock, unknownX, unknownY);
    [Instruction(Opcode.BGVDEFINE)]
    public static void DefineViewport(Operand viewportId, Operand x, Operand y, Operand width, Operand height, int mask = -1) => EventScheduler.Execute(Opcode.BGVDEFINE, viewportId, x, y, width, height);
    [Instruction(Opcode.BGVSET)]
    public static void SetTileCamera(Operand tileBlock, Operand cameraId, int mask = -1) => EventScheduler.Execute(Opcode.BGVSET, tileBlock, cameraId);
    [Instruction(Opcode.CLEARCOLOR)]
    public static void SetBackgroundColor(Operand red, Operand green, Operand blue, int mask = -1) => EventScheduler.Execute(Opcode.CLEARCOLOR, red, green, blue);
}

public static partial class Vibration
{
    [Instruction(Opcode.VIBACTIVE)]
    public static void ActivateVibration(Operand activate, int mask = -1) => EventScheduler.Execute(Opcode.VIBACTIVE, activate);
    [Instruction(Opcode.VIBFLAG)]
    public static void SetVibrationFlags(Operand flags, int mask = -1) => EventScheduler.Execute(Opcode.VIBFLAG, flags);
    [Instruction(Opcode.VIBRANGE)]
    public static void SetVibrationRange(Operand start, Operand end, int mask = -1) => EventScheduler.Execute(Opcode.VIBRANGE, start, end);
    [Instruction(Opcode.VIBRATE)]
    public static void SetVibrationSpeed(Operand frameRate, int mask = -1) => EventScheduler.Execute(Opcode.VIBRATE, frameRate);
    [Instruction(Opcode.VIBSTART)]
    public static void VibrateController(Operand frame, int mask = -1) => EventScheduler.Execute(Opcode.VIBSTART, frame);
    [Instruction(Opcode.VIBTRACK)]
    public static void ActivateVibrationTrack(Operand track, Operand sample, Operand activate, int mask = -1) => EventScheduler.Execute(Opcode.VIBTRACK, track, sample, activate);
    [Instruction(Opcode.VIBTRACK1)]
    public static void RunVibrationTrack(Operand track, Operand sample, Operand activate, int mask = -1) => EventScheduler.Execute(Opcode.VIBTRACK1, track, sample, activate);
}

public static partial class VisualEffects
{
    [Instruction(Opcode.FMV)]
    public static void ControlCinematic(Operand operation, Operand cinematicId, Operand parameter, int mask = -1) => EventScheduler.Execute(Opcode.FMV, operation, cinematicId, parameter);
    [Instruction(Opcode.SHADOWAMP)]
    public static void SetShadowAmplifier(Operand amplificationFactor, int mask = -1) => EventScheduler.Execute(Opcode.SHADOWAMP, amplificationFactor);
    [Instruction(Opcode.SHADOWLOCK)]
    public static void LockShadowRotation(Operand lockedRotation, int mask = -1) => EventScheduler.Execute(Opcode.SHADOWLOCK, lockedRotation);
    [Instruction(Opcode.SHADOWOFF)]
    public static void DisableShadow() => EventScheduler.Execute(Opcode.SHADOWOFF);
    [Instruction(Opcode.SHADOWOFFSET)]
    public static void SetShadowOffset(Operand offsetX, Operand offsetY, int mask = -1) => EventScheduler.Execute(Opcode.SHADOWOFFSET, offsetX, offsetY);
    [Instruction(Opcode.SHADOWON)]
    public static void EnableShadow() => EventScheduler.Execute(Opcode.SHADOWON);
    [Instruction(Opcode.SHADOWSCALE)]
    public static void SetShadowSize(Operand sizeX, Operand sizeY, int mask = -1) => EventScheduler.Execute(Opcode.SHADOWSCALE, sizeX, sizeY);
    [Instruction(Opcode.SHADOWUNLOCK)]
    public static void UnlockShadowRotation() => EventScheduler.Execute(Opcode.SHADOWUNLOCK);
    [Instruction(Opcode.SPS)]
    public static void DispatchSpriteCommand(Operand effect, Operand command, Operand parameter1, Operand parameter2, Operand parameter3, int mask = -1) => EventScheduler.Execute(Opcode.SPS, effect, command, parameter1, parameter2, parameter3);
    [Instruction(Opcode.SPS2)]
    public static void DispatchCompactSpriteCommand(Operand effect, Operand command, Operand parameter1, Operand parameter2, Operand parameter3, int mask = -1) => EventScheduler.Execute(Opcode.SPS2, effect, command, parameter1, parameter2, parameter3);
    [Instruction(Opcode.WIPERGB)]
    public static void FadeScreen(Operand mode, Operand frames, Operand unknown, Operand cyan, Operand magenta, Operand yellow, int mask = -1) => EventScheduler.Execute(Opcode.WIPERGB, mode, frames, unknown, cyan, magenta, yellow);
    [Instruction(Opcode.SPS, fixedIndex: 1, fixedValue: 130)]
    public static void LoadSprite(Operand effect, Operand spriteId, Operand source, Operand unused, int mask = -1) => EventScheduler.Execute(Opcode.SPS, effect, 130, spriteId, source, unused);
    [Instruction(Opcode.SPS2, fixedIndex: 1, fixedValue: 130)]
    public static void LoadSpriteCompact(Operand effect, Operand spriteId, Operand source, Operand unused, int mask = -1) => EventScheduler.Execute(Opcode.SPS2, effect, 130, spriteId, source, unused);
    [Instruction(Opcode.SPS, fixedIndex: 1, fixedValue: 131)]
    public static void SetSpriteAttributes(Operand effect, Operand flags, Operand enabled, Operand unused, int mask = -1) => EventScheduler.Execute(Opcode.SPS, effect, 131, flags, enabled, unused);
    [Instruction(Opcode.SPS2, fixedIndex: 1, fixedValue: 131)]
    public static void SetSpriteAttributesCompact(Operand effect, Operand flags, Operand enabled, Operand unused, int mask = -1) => EventScheduler.Execute(Opcode.SPS2, effect, 131, flags, enabled, unused);
    [Instruction(Opcode.SPS, fixedIndex: 1, fixedValue: 135)]
    public static void SetSpritePosition(Operand effect, Operand x, Operand negativeY, Operand z, int mask = -1) => EventScheduler.Execute(Opcode.SPS, effect, 135, x, negativeY, z);
    [Instruction(Opcode.SPS2, fixedIndex: 1, fixedValue: 135)]
    public static void SetSpritePositionCompact(Operand effect, Operand x, Operand negativeY, Operand z, int mask = -1) => EventScheduler.Execute(Opcode.SPS2, effect, 135, x, negativeY, z);
    [Instruction(Opcode.SPS, fixedIndex: 1, fixedValue: 140)]
    public static void SetSpriteRotation(Operand effect, Operand x, Operand y, Operand z, int mask = -1) => EventScheduler.Execute(Opcode.SPS, effect, 140, x, y, z);
    [Instruction(Opcode.SPS2, fixedIndex: 1, fixedValue: 140)]
    public static void SetSpriteRotationCompact(Operand effect, Operand x, Operand y, Operand z, int mask = -1) => EventScheduler.Execute(Opcode.SPS2, effect, 140, x, y, z);
    [Instruction(Opcode.SPS, fixedIndex: 1, fixedValue: 145)]
    public static void SetScale(Operand effect, Operand scale, Operand parameter2, Operand parameter3, int mask = -1) => EventScheduler.Execute(Opcode.SPS, effect, 145, scale, parameter2, parameter3);
    [Instruction(Opcode.SPS2, fixedIndex: 1, fixedValue: 145)]
    public static void SetScaleCompact(Operand effect, Operand scale, Operand parameter2, Operand parameter3, int mask = -1) => EventScheduler.Execute(Opcode.SPS2, effect, 145, scale, parameter2, parameter3);
    [Instruction(Opcode.SPS, fixedIndex: 1, fixedValue: 150)]
    public static void AttachSprite(Operand effect, Operand actorUid, Operand bone, Operand unused, int mask = -1) => EventScheduler.Execute(Opcode.SPS, effect, 150, actorUid, bone, unused);
    [Instruction(Opcode.SPS2, fixedIndex: 1, fixedValue: 150)]
    public static void AttachSpriteCompact(Operand effect, Operand actorUid, Operand bone, Operand unused, int mask = -1) => EventScheduler.Execute(Opcode.SPS2, effect, 150, actorUid, bone, unused);
    [Instruction(Opcode.SPS, fixedIndex: 1, fixedValue: 155)]
    public static void SetSpriteFade(Operand effect, Operand fade, Operand unused2, Operand unused3, int mask = -1) => EventScheduler.Execute(Opcode.SPS, effect, 155, fade, unused2, unused3);
    [Instruction(Opcode.SPS2, fixedIndex: 1, fixedValue: 155)]
    public static void SetSpriteFadeCompact(Operand effect, Operand fade, Operand unused2, Operand unused3, int mask = -1) => EventScheduler.Execute(Opcode.SPS2, effect, 155, fade, unused2, unused3);
    [Instruction(Opcode.SPS, fixedIndex: 1, fixedValue: 156)]
    public static void SetSpriteShader(Operand effect, Operand shader, Operand unused2, Operand unused3, int mask = -1) => EventScheduler.Execute(Opcode.SPS, effect, 156, shader, unused2, unused3);
    [Instruction(Opcode.SPS2, fixedIndex: 1, fixedValue: 156)]
    public static void SetSpriteShaderCompact(Operand effect, Operand shader, Operand unused2, Operand unused3, int mask = -1) => EventScheduler.Execute(Opcode.SPS2, effect, 156, shader, unused2, unused3);
    [Instruction(Opcode.SPS, fixedIndex: 1, fixedValue: 160)]
    public static void SetSpriteFrameRate(Operand effect, Operand rate, Operand unused2, Operand unused3, int mask = -1) => EventScheduler.Execute(Opcode.SPS, effect, 160, rate, unused2, unused3);
    [Instruction(Opcode.SPS2, fixedIndex: 1, fixedValue: 160)]
    public static void SetSpriteFrameRateCompact(Operand effect, Operand rate, Operand unused2, Operand unused3, int mask = -1) => EventScheduler.Execute(Opcode.SPS2, effect, 160, rate, unused2, unused3);
    [Instruction(Opcode.SPS, fixedIndex: 1, fixedValue: 161)]
    public static void SetSpriteFrame(Operand effect, Operand frame, Operand unused2, Operand unused3, int mask = -1) => EventScheduler.Execute(Opcode.SPS, effect, 161, frame, unused2, unused3);
    [Instruction(Opcode.SPS2, fixedIndex: 1, fixedValue: 161)]
    public static void SetSpriteFrameCompact(Operand effect, Operand frame, Operand unused2, Operand unused3, int mask = -1) => EventScheduler.Execute(Opcode.SPS2, effect, 161, frame, unused2, unused3);
    [Instruction(Opcode.SPS, fixedIndex: 1, fixedValue: 165)]
    public static void SetSpritePositionOffset(Operand effect, Operand x, Operand negativeY, Operand z, int mask = -1) => EventScheduler.Execute(Opcode.SPS, effect, 165, x, negativeY, z);
    [Instruction(Opcode.SPS2, fixedIndex: 1, fixedValue: 165)]
    public static void SetSpritePositionOffsetCompact(Operand effect, Operand x, Operand negativeY, Operand z, int mask = -1) => EventScheduler.Execute(Opcode.SPS2, effect, 165, x, negativeY, z);
    [Instruction(Opcode.SPS, fixedIndex: 1, fixedValue: 170)]
    public static void SetSpriteDepthOffset(Operand effect, Operand depth, Operand unused2, Operand unused3, int mask = -1) => EventScheduler.Execute(Opcode.SPS, effect, 170, depth, unused2, unused3);
    [Instruction(Opcode.SPS2, fixedIndex: 1, fixedValue: 170)]
    public static void SetSpriteDepthOffsetCompact(Operand effect, Operand depth, Operand unused2, Operand unused3, int mask = -1) => EventScheduler.Execute(Opcode.SPS2, effect, 170, depth, unused2, unused3);
}

public static partial class World
{
    [Instruction(Opcode.AIM)]
    public static void FollowCameraAim() => EventScheduler.Execute(Opcode.AIM);
    [Instruction(Opcode.EYE)]
    public static void FollowCameraEye() => EventScheduler.Execute(Opcode.EYE);
    [Instruction(Opcode.WMAPJUMP)]
    public static void Enter(Operand worldMapId, int mask = -1) => EventScheduler.Execute(Opcode.WMAPJUMP, worldMapId);
    [Instruction(Opcode.WPRM)]
    public static void RunWorldCode(Operand code, Operand argument, int mask = -1) => EventScheduler.Execute(Opcode.WPRM, code, argument);
}

public static class SoundCommands
{
    public const int SongPlay = 0;
    public const int SongPlaySlot1 = 64;
    public const int SongLoad = 1792;
    public const int SongLoadSlot1 = 1856;
    public const int SongSuspend = 2048;
    public const int SongSuspendSlot1 = 2112;
    public const int SongResume = 2304;
    public const int SongResumeSlot1 = 2368;
    public const int SongJumpPoint = 2566;
    public const int SongJumpPointSlot1 = 2630;
    public const int SongSkipPhrase = 16903;
    public const int SongSkipPhraseSlot1 = 16967;
    public const int SongStop = 256;
    public const int SongStopSlot1 = 320;
    public const int SongStopCurrent = 265;
    public const int SongStopCurrentSlot1 = 329;
    public const int SongVol = 16897;
    public const int SongVolSlot1 = 16961;
    public const int SongVolFade = 50177;
    public const int SongVolFadeSlot1 = 50241;
    public const int SongVolFadeAll = 51969;
    public const int SongVolFadeAllSlot1 = 52033;
    public const int SongVolIntpl = 33537;
    public const int SongVolIntplSlot1 = 33601;
    public const int SongVolIntplAll = 34305;
    public const int SongVolIntplAllSlot1 = 34369;
    public const int SongPitch = 16899;
    public const int SongPitchSlot1 = 16963;
    public const int SongPitchIntpl = 33539;
    public const int SongPitchIntplSlot1 = 33603;
    public const int SongPitchFade = 50179;
    public const int SongPitchFadeSlot1 = 50243;
    public const int StreamPlay = 40960;
    public const int StreamPlaySlot1 = 41024;
    public const int StreamStop = 8448;
    public const int StreamStopSlot1 = 8512;
    public const int StreamVol = 25089;
    public const int StreamVolSlot1 = 25153;
    public const int SfxPlay = 53248;
    public const int SfxPlaySlot1 = 53312;
    public const int SfxStop = 20736;
    public const int SfxStopSlot1 = 20800;
    public const int SfxVol = 37377;
    public const int SfxVolSlot1 = 37441;
    public const int SfxVolIntpl = 54017;
    public const int SfxVolIntplSlot1 = 54081;
    public const int SfxVolAll = 21761;
    public const int SfxVolAllSlot1 = 21825;
    public const int SfxVolIntplAll = 38401;
    public const int SfxVolIntplAllSlot1 = 38465;
    public const int SfxPitch = 37379;
    public const int SfxPitchSlot1 = 37443;
    public const int SfxResidentStopCurrent = 4489;
    public const int SfxResidentStopCurrentSlot1 = 4553;
    public const int SfxResidentSuspend = 6272;
    public const int SfxResidentSuspendSlot1 = 6336;
    public const int SfxResidentRestore = 6528;
    public const int SfxResidentRestoreSlot1 = 6592;
    public const int SfxResidentStop = 20864;
    public const int SfxResidentStopSlot1 = 20928;
    public const int SfxResidentVolAll = 21889;
    public const int SfxResidentVolAllSlot1 = 21953;
    public const int SfxResidentVol = 37505;
    public const int SfxResidentVolSlot1 = 37569;
    public const int SfxResidentVolIntplAll = 38529;
    public const int SfxResidentVolIntplAllSlot1 = 38593;
    public const int SfxResidentPlay = 53376;
    public const int SfxResidentPlaySlot1 = 53440;
    public const int SfxResidentVolIntpl = 54145;
    public const int SfxResidentVolIntplSlot1 = 54209;
    public const int Sync = 3072;
    public const int SyncSlot1 = 3136;
    public const int InstrLoad = 30464;
    public const int InstrLoadSlot1 = 30528;
    public const int SongNull = 520;
    public const int SongNullSlot1 = 584;
    public const int SongTempo = 16898;
    public const int SongTempoSlot1 = 16962;
    public const int SongTempoPitch = 16900;
    public const int SongTempoPitchSlot1 = 16964;
    public const int SongTempoPitchFade = 1028;
    public const int SongTempoPitchFadeSlot1 = 1092;
    public const int SongTempoPitchIntpl = 33540;
    public const int SongTempoPitchIntplSlot1 = 33604;
    public const int SongTempoFade = 50178;
    public const int SongTempoFadeSlot1 = 50242;
    public const int SongTempoIntpl = 33538;
    public const int SongTempoIntplSlot1 = 33602;
    public const int StreamNull = 8712;
    public const int StreamNullSlot1 = 8776;
    public const int StreamPos = 25093;
    public const int StreamPosSlot1 = 25157;
    public const int StreamFmvVol = 25098;
    public const int StreamFmvVolSlot1 = 25162;
    public const int StreamReverb = 25100;
    public const int StreamReverbSlot1 = 25164;
    public const int SfxNull = 4616;
    public const int SfxNullSlot1 = 4680;
    public const int SfxPitchAll = 21763;
    public const int SfxPitchAllSlot1 = 21827;
    public const int SfxPitchIntpl = 54019;
    public const int SfxPitchIntplSlot1 = 54083;
    public const int SfxPitchIntplAll = 38403;
    public const int SfxPitchIntplAllSlot1 = 38467;
    public const int SfxPos = 37381;
    public const int SfxPosSlot1 = 37445;
    public const int SfxPosAll = 21765;
    public const int SfxPosAllSlot1 = 21829;
    public const int SfxPosIntpl = 54021;
    public const int SfxPosIntplSlot1 = 54085;
    public const int SfxPosIntplAll = 38405;
    public const int SfxPosIntplAllSlot1 = 38469;
    public const int SfxResPitch = 37507;
    public const int SfxResPitchSlot1 = 37571;
    public const int SfxResPitchAll = 21891;
    public const int SfxResPitchAllSlot1 = 21955;
    public const int SfxResPitchIntpl = 54147;
    public const int SfxResPitchIntplSlot1 = 54211;
    public const int SfxResPitchIntplAll = 38531;
    public const int SfxResPitchIntplAllSlot1 = 38595;
    public const int SfxResPos = 37509;
    public const int SfxResPosSlot1 = 37573;
    public const int SfxResPosAll = 21893;
    public const int SfxResPosAllSlot1 = 21957;
    public const int SfxResPosIntpl = 54149;
    public const int SfxResPosIntplSlot1 = 54213;
    public const int SfxResPosIntplAll = 38533;
    public const int SfxResPosIntplAllSlot1 = 38597;
}

public static partial class Variables
{
    [ExpressionInstruction(Excode.IncrementPost)]
    public static Operand PostIncrement(Operand target) => EventScheduler.Read(Excode.IncrementPost, target);
    [ExpressionInstruction(Excode.DecrementPost)]
    public static Operand PostDecrement(Operand target) => EventScheduler.Read(Excode.DecrementPost, target);
    [ExpressionInstruction(Excode.B_PRE_PLUS)]
    public static Operand PreIncrement(Operand target) => EventScheduler.Read(Excode.B_PRE_PLUS, target);
    [ExpressionInstruction(Excode.B_PRE_MINUS)]
    public static Operand PreDecrement(Operand target) => EventScheduler.Read(Excode.B_PRE_MINUS, target);
    [ExpressionInstruction(Excode.Let)]
    public static Operand Set(Operand target, Operand value) => EventScheduler.Read(Excode.Let, target, value);
    [ExpressionInstruction(Excode.MulLet)]
    public static Operand MultiplyBy(Operand target, Operand value) => EventScheduler.Read(Excode.MulLet, target, value);
    [ExpressionInstruction(Excode.DivLet)]
    public static Operand DivideBy(Operand target, Operand value) => EventScheduler.Read(Excode.DivLet, target, value);
    [ExpressionInstruction(Excode.ModeLet)]
    public static Operand KeepRemainder(Operand target, Operand divisor) => EventScheduler.Read(Excode.ModeLet, target, divisor);
    [ExpressionInstruction(Excode.AddLet)]
    public static Operand AddTo(Operand target, Operand value) => EventScheduler.Read(Excode.AddLet, target, value);
    [ExpressionInstruction(Excode.SubLet)]
    public static Operand SubtractFrom(Operand target, Operand value) => EventScheduler.Read(Excode.SubLet, target, value);
    [ExpressionInstruction(Excode.BitLeftLet)]
    public static Operand ShiftLeft(Operand target, Operand bits) => EventScheduler.Read(Excode.BitLeftLet, target, bits);
    [ExpressionInstruction(Excode.BitRightLet)]
    public static Operand ShiftRight(Operand target, Operand bits) => EventScheduler.Read(Excode.BitRightLet, target, bits);
    [ExpressionInstruction(Excode.BitAndLet)]
    public static Operand KeepBits(Operand target, Operand mask) => EventScheduler.Read(Excode.BitAndLet, target, mask);
    [ExpressionInstruction(Excode.BitXorLet)]
    public static Operand ToggleBits(Operand target, Operand mask) => EventScheduler.Read(Excode.BitXorLet, target, mask);
    [ExpressionInstruction(Excode.BitOrLet)]
    public static Operand SetBits(Operand target, Operand mask) => EventScheduler.Read(Excode.BitOrLet, target, mask);
}

public static partial class Values
{
    [ExpressionInstruction(Excode.B_SINGLE_PLUS)]
    public static int ReuseLastCalculation(Operand value) => (int)value;
    [ExpressionInstruction(Excode.B_SINGLE_MINUS)]
    public static int Negate(Operand value) => EventScheduler.Read(Excode.B_SINGLE_MINUS, value);
    [ExpressionInstruction(Excode.B_NOT)]
    public static bool NotPositive(Operand value) => EventScheduler.Read(Excode.B_NOT, value) != 0;
    [ExpressionInstruction(Excode.B_COMP)]
    public static int InvertBits(Operand value) => EventScheduler.Read(Excode.B_COMP, value);
    [ExpressionInstruction(Excode.Mul)]
    public static int Multiply(Operand left, Operand right) => EventScheduler.Read(Excode.Mul, left, right);
    [ExpressionInstruction(Excode.Div)]
    public static int Divide(Operand dividend, Operand divisor) => EventScheduler.Read(Excode.Div, dividend, divisor);
    [ExpressionInstruction(Excode.Mod)]
    public static int Remainder(Operand dividend, Operand divisor) => EventScheduler.Read(Excode.Mod, dividend, divisor);
    [ExpressionInstruction(Excode.Add)]
    public static int Add(Operand left, Operand right) => EventScheduler.Read(Excode.Add, left, right);
    [ExpressionInstruction(Excode.Sub)]
    public static int Subtract(Operand left, Operand right) => EventScheduler.Read(Excode.Sub, left, right);
    [ExpressionInstruction(Excode.BitLeft)]
    public static int ShiftLeft(Operand value, Operand bits) => EventScheduler.Read(Excode.BitLeft, value, bits);
    [ExpressionInstruction(Excode.BitRight)]
    public static int ShiftRight(Operand value, Operand bits) => EventScheduler.Read(Excode.BitRight, value, bits);
    [ExpressionInstruction(Excode.LessThan)]
    public static int LessThan(Operand left, Operand right) => EventScheduler.Read(Excode.LessThan, left, right);
    [ExpressionInstruction(Excode.GreatThan)]
    public static int GreaterThan(Operand left, Operand right) => EventScheduler.Read(Excode.GreatThan, left, right);
    [ExpressionInstruction(Excode.LessOrEquals)]
    public static int AtMost(Operand left, Operand right) => EventScheduler.Read(Excode.LessOrEquals, left, right);
    [ExpressionInstruction(Excode.GreatOrEquals)]
    public static int AtLeast(Operand left, Operand right) => EventScheduler.Read(Excode.GreatOrEquals, left, right);
    [ExpressionInstruction(Excode.Equivalence)]
    public static int Equal(Operand left, Operand right) => EventScheduler.Read(Excode.Equivalence, left, right);
    [ExpressionInstruction(Excode.NotEquivalence)]
    public static int NotEqual(Operand left, Operand right) => EventScheduler.Read(Excode.NotEquivalence, left, right);
    [ExpressionInstruction(Excode.BitAnd)]
    public static int BitAnd(Operand left, Operand right) => EventScheduler.Read(Excode.BitAnd, left, right);
    [ExpressionInstruction(Excode.BitXor)]
    public static int BitXor(Operand left, Operand right) => EventScheduler.Read(Excode.BitXor, left, right);
    [ExpressionInstruction(Excode.BitOr)]
    public static int BitOr(Operand left, Operand right) => EventScheduler.Read(Excode.BitOr, left, right);
    [ExpressionInstruction(Excode.BooleanAnd)]
    public static int And(Operand left, Operand right) => EventScheduler.Read(Excode.BooleanAnd, left, right);
    [ExpressionInstruction(Excode.BooleanOr)]
    public static int Or(Operand left, Operand right) => EventScheduler.Read(Excode.BooleanOr, left, right);
    [ExpressionInstruction(Excode.B_SIN2)]
    public static int Sin(Operand angle) => EventScheduler.Read(Excode.B_SIN2, angle);
    [ExpressionInstruction(Excode.B_COS2)]
    public static int Cos(Operand angle) => EventScheduler.Read(Excode.B_COS2, angle);
    [ExpressionInstruction(Excode.B_SIN)]
    public static int SinScaledAngle(Operand angle) => EventScheduler.Read(Excode.B_SIN, angle);
    [ExpressionInstruction(Excode.B_COS)]
    public static int CosScaledAngle(Operand angle) => EventScheduler.Read(Excode.B_COS, angle);
    [ExpressionInstruction(Excode.GetAngle)]
    public static int AngleOfVector(Operand x, Operand z) => EventScheduler.Read(Excode.GetAngle, x, z);
}

public static partial class Input
{
    [ExpressionInstruction(Excode.IsKeyPressed)]
    public static int IsKeyPressed(Operand buttons) => EventScheduler.Read(Excode.IsKeyPressed, buttons);
    [ExpressionInstruction(Excode.B_KEYOFF)]
    public static int IsKeyReleased(Operand buttons) => EventScheduler.Read(Excode.B_KEYOFF, buttons);
    [ExpressionInstruction(Excode.B_KEY)]
    public static int IsKeyHeld(Operand buttons) => EventScheduler.Read(Excode.B_KEY, buttons);
}

public static partial class Actors
{
    [ExpressionInstruction(Excode.GetActorAngle)]
    public static int AngleToPosition(Operand x, Operand z) => EventScheduler.Read(Excode.GetActorAngle, x, z);
    [ExpressionInstruction(Excode.GetActorDistance)]
    public static int DistanceToPosition(Operand x, Operand z) => EventScheduler.Read(Excode.GetActorDistance, x, z);
    [ExpressionInstruction(Excode.B_ANGLEA)]
    public static int AngleToActor(Operand targetUid) => EventScheduler.Read(Excode.B_ANGLEA, targetUid);
    [ExpressionInstruction(Excode.B_DISTANCEA)]
    public static int DistanceToActor(Operand targetUid) => EventScheduler.Read(Excode.B_DISTANCEA, targetUid);
    [ExpressionInstruction(Excode.B_PTR)]
    public static int ResolveUid(byte targetUid) => EventScheduler.Read(Excode.B_PTR, targetUid);
    [ExpressionInstruction(Excode.B_MEMBER)]
    public static int ReadMemberProperty(byte property) => EventScheduler.Read(Excode.B_MEMBER, property);
    [ExpressionInstruction(Excode.B_OBJSPECA)]
    public static int ReadProperty(byte targetUid, byte property) => EventScheduler.Read(Excode.B_OBJSPECA, targetUid, property);
}

public static partial class Inventory
{
    [ExpressionInstruction(Excode.B_HAVE_ITEM)]
    public static int Count(Operand itemId) => EventScheduler.Read(Excode.B_HAVE_ITEM, itemId);
}

public static partial class Regions
{
    [ExpressionInstruction(Excode.B_BAFRAME)]
    public static int GetWalkmeshAnimationFrame(Operand animationId) => EventScheduler.Read(Excode.B_BAFRAME, animationId);
    [ExpressionInstruction(Excode.B_BGIID)]
    public static int TriangleOf(Operand actorUid) => EventScheduler.Read(Excode.B_BGIID, actorUid);
    [ExpressionInstruction(Excode.B_BGIFLOOR)]
    public static int FloorOf(Operand actorUid) => EventScheduler.Read(Excode.B_BGIFLOOR, actorUid);
}

public static partial class Animations
{
    [ExpressionInstruction(Excode.B_FRAME)]
    public static int GetLength(Operand animation) => EventScheduler.Read(Excode.B_FRAME, animation);
}

public static partial class Party
{
    [ExpressionInstruction(Excode.IsInParty)]
    public static int Contains(Operand character) => EventScheduler.Read(Excode.IsInParty, character);
    [ExpressionInstruction(Excode.AddToParty)]
    public static int TryAdd(Operand character) => EventScheduler.Read(Excode.AddToParty, character);
    [ExpressionInstruction(Excode.GetCurrentHP)]
    public static int CurrentHp(Operand character) => EventScheduler.Read(Excode.GetCurrentHP, character);
    [ExpressionInstruction(Excode.GetMaxHP)]
    public static int MaximumHp(Operand character) => EventScheduler.Read(Excode.GetMaxHP, character);
    [ExpressionInstruction(Excode.GetCurrentMP)]
    public static int CurrentMp(Operand character) => EventScheduler.Read(Excode.GetCurrentMP, character);
    [ExpressionInstruction(Excode.GetMaxMP)]
    public static int MaximumMp(Operand character) => EventScheduler.Read(Excode.GetMaxMP, character);
}

public static partial class VisualEffects
{
    [ExpressionInstruction(Excode.B_SPS)]
    public static int QuerySprite(Operand effect, Operand property) => EventScheduler.Read(Excode.B_SPS, effect, property);
}

public static partial class Engine
{
    [ExpressionInstruction(Excode.B_SYSLIST)]
    public static int ReadSystemList(byte slot) => EventScheduler.Read(Excode.B_SYSLIST, slot);
    [ExpressionInstruction(Excode.B_SYSVAR)]
    public static int ReadSystem(SystemVariable variable) => EventScheduler.Read(Excode.B_SYSVAR, (int)variable);
}
