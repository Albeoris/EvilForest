using System.Diagnostics.CodeAnalysis;
using System.Text;
using FF8.JSM;
using Memoria.EventEngine.EV;

namespace EveilForest.CSharp;

internal sealed class SourceInstruction(Jsm.Opcode opcode, string service, string method, params string[] operands)
{
    public Jsm.Opcode Opcode { get; } = opcode;
    public string Service { get; } = service;
    public string Method { get; } = method;
    public string[] Operands { get; } = operands;
    public (int Index, int Value)? FixedOperand { get; init; }
    public bool HasMask => EVOperands.For(Opcode).StartsWith('m');

    // The source names follow the existing instruction formatters and Memoria's event dispatch.
    private static readonly SourceInstruction[] Instructions =
    {
        new(Jsm.Opcode.ATTACH, "Actors", "AttachObject", "targetUid", "carrier", "attachmentPoint"),
        new(Jsm.Opcode.ATTACHOFFSET, "Actors", "SetAttachmentOffset", "x", "z", "y"),
        new(Jsm.Opcode.BGI, "Actors", "SetPathing", "pathing"),
        new(Jsm.Opcode.BGSMOVE, "Actors", "SlideTo3D", "x", "z", "y"),
        new(Jsm.Opcode.CFLAG, "Actors", "SetObjectFlags", "flags"),
        new(Jsm.Opcode.CHRCLEAR, "Actors", "RemoveCharacterAttribute", "attributes"),
        new(Jsm.Opcode.CHRCOLOR, "Actors", "SetColor", "targetUid", "red", "green", "blue"),
        new(Jsm.Opcode.CHRFX, "Actors", "RunModelCode", "modelCode", "argument1", "argument2", "argument3"),
        new(Jsm.Opcode.CHRSCALE, "Actors", "SetObjectSize", "targetUid", "sizeX", "sizeZ", "sizeY"),
        new(Jsm.Opcode.CHRSET, "Actors", "AddCharacterAttribute", "attributes"),
        new(Jsm.Opcode.CLRDIST, "Actors", "BeginWalk"),
        new(Jsm.Opcode.DDIR, "Actors", "TurnInstantEx", "entry", "angle"),
        new(Jsm.Opcode.DETACH, "Actors", "DetachObject", "targetUid"),
        new(Jsm.Opcode.DIRE, "Actors", "TurnInstant", "angle"),
        new(Jsm.Opcode.DPOS, "Actors", "SetActorPosition", "targetUid", "x", "z"),
        new(Jsm.Opcode.DPOS3, "Actors", "SetActorPosition3D", "targetUid", "x", "z", "y"),
        new(Jsm.Opcode.DRADIUS, "Legacy", "LegacyActorRadius", "targetUid", "parameter1", "parameter2", "parameter3"),
        new(Jsm.Opcode.DTURN, "Actors", "TurnActorAtSpeed", "targetUid", "angle", "speed"),
        new(Jsm.Opcode.DWAITTURN, "Actors", "WaitForActorTurn", "targetUid"),
        new(Jsm.Opcode.GETSCREEN, "Actors", "CalculateScreenPosition", "targetUid"),
        new(Jsm.Opcode.JUMP3, "Actors", "Jump"),
        new(Jsm.Opcode.MESHHIDE, "Actors", "HideMeshes", "targetUid", "meshMask"),
        new(Jsm.Opcode.MESHSHOW, "Actors", "ShowMeshes", "targetUid", "meshMask"),
        new(Jsm.Opcode.MJPOS, "Actors", "CalculateExitPosition"),
        new(Jsm.Opcode.MODEL, "Actors", "SetModel", "model", "height"),
        new(Jsm.Opcode.MOVA, "Actors", "WalkTowardObject", "targetUid"),
        new(Jsm.Opcode.MOVE, "Actors", "WalkTo", "x", "z"),
        new(Jsm.Opcode.MOVE3, "Actors", "WalkTo3D", "x", "z", "y"),
        new(Jsm.Opcode.MOVE3H, "Actors", "SideStepTo3D", "x", "z", "y"),
        new(Jsm.Opcode.MOVH, "Actors", "SlideTo", "x", "z"),
        new(Jsm.Opcode.MOVJ, "Actors", "WalkToExit"),
        new(Jsm.Opcode.MROT, "Actors", "SetWalkTurnSpeed", "turnSpeed"),
        new(Jsm.Opcode.MSPEED, "Actors", "SetWalkSpeed", "speed"),
        new(Jsm.Opcode.NECKFLAG, "Actors", "EnableHeadFocus", "flags"),
        new(Jsm.Opcode.NECKID, "Actors", "SetHeadFocusMask", "selfMask", "targetMask"),
        new(Jsm.Opcode.OBJINDEX, "Actors", "SetObjectIndex", "index"),
        new(Jsm.Opcode.OVAL, "Actors", "SetObjectOvalRatio", "stretchingFactor"),
        new(Jsm.Opcode.POPSHOW, "Actors", "ShowAllObjects"),
        new(Jsm.Opcode.POS, "Actors", "SetPosition", "x", "z"),
        new(Jsm.Opcode.POS3, "Actors", "SetPosition3D", "x", "z", "y"),
        new(Jsm.Opcode.POSTJUMP, "Actors", "RunLandAnimation"),
        new(Jsm.Opcode.PREJUMP, "Actors", "RunJumpAnimation"),
        new(Jsm.Opcode.PRETEND, "Actors", "PretendToBe", "entry"),
        new(Jsm.Opcode.PUSHHIDE, "Actors", "HideAllObjects"),
        new(Jsm.Opcode.RADIUS, "Actors", "SetObjectLogicalSize", "walkRadius", "collisionRadius", "talkDistance"),
        new(Jsm.Opcode.ROTXZ, "Actors", "SetPitchAngle", "pitch", "xzAngle"),
        new(Jsm.Opcode.SETVY3, "Actors", "PrepareJump", "x", "z", "y", "steps"),
        new(Jsm.Opcode.SPEEDTH, "Actors", "SetRunSpeedLimit", "speedLimit"),
        new(Jsm.Opcode.STOP, "Scripts", "StopExecution", "parameter"),
        new(Jsm.Opcode.TSPEED, "Actors", "SetTurnSpeed", "speed"),
        new(Jsm.Opcode.TURN, "Actors", "TurnAtSpeed", "angle", "speed"),
        new(Jsm.Opcode.TURNA, "Actors", "TurnTowardObject", "targetUid", "speed"),
        new(Jsm.Opcode.TURNDS, "Actors", "Turn", "angle"),
        new(Jsm.Opcode.TURNTO, "Actors", "TurnTowardPosition", "x", "z"),
        new(Jsm.Opcode.WAITTURN, "Actors", "WaitForTurn"),
        new(Jsm.Opcode.WATCH, "Legacy", "WatchVariable"),
        new(Jsm.Opcode.AFRAME, "Animations", "SetFrameRange", "firstFrame", "lastFrame"),
        new(Jsm.Opcode.AIDLE, "Animations", "SetIdle", "animation"),
        new(Jsm.Opcode.AJUMP, "Animations", "SetJumpAnimation", "animation", "parameter2", "unknown"),
        new(Jsm.Opcode.AMODE, "Animations", "SetAnimationFlags", "flag", "repeat"),
        new(Jsm.Opcode.ANIM, "Animations", "Play", "animation"),
        new(Jsm.Opcode.ARUN, "Animations", "SetRun", "animation"),
        new(Jsm.Opcode.ASLEEP, "Animations", "SetSleep", "animation"),
        new(Jsm.Opcode.ASPEED, "Animations", "SetAnimationSpeed", "speed"),
        new(Jsm.Opcode.ATURNL, "Animations", "SetTurnLeft", "animation"),
        new(Jsm.Opcode.ATURNR, "Animations", "SetTurnRight", "animation"),
        new(Jsm.Opcode.AWALK, "Animations", "SetWalk", "animation"),
        new(Jsm.Opcode.DANIM, "Animations", "PlayForActor", "targetUid", "animation"),
        new(Jsm.Opcode.DWAITANIM, "Animations", "WaitForActor", "targetUid"),
        new(Jsm.Opcode.ENDANIM, "Animations", "Stop"),
        new(Jsm.Opcode.IDLESPEED, "Animations", "SetIdleSpeeds", "speed1", "speed2", "speed3", "speed4"),
        new(Jsm.Opcode.MOVT, "Animations", "MakeAnimationLoop", "amount"),
        new(Jsm.Opcode.SLEEPINH, "Animations", "DisableInactiveAnimation"),
        new(Jsm.Opcode.SLEEPON, "Animations", "EnableInactiveAnimation"),
        new(Jsm.Opcode.TEXPLAY, "Animations", "EnableTextureAnimation", "targetUid", "textureAnimation"),
        new(Jsm.Opcode.TEXPLAY1, "Animations", "RunTextureAnimation", "targetUid", "textureAnimation"),
        new(Jsm.Opcode.TEXSTOP, "Animations", "StopTextureAnimation", "targetUid", "textureAnimation"),
        new(Jsm.Opcode.WAITANIM, "Animations", "WaitForCompletion"),
        new(Jsm.Opcode.FLDSND0, "Audio", "DispatchSound", "command", "sound"),
        new(Jsm.Opcode.FLDSND1, "Audio", "DispatchSound1", "command", "sound", "parameter1"),
        new(Jsm.Opcode.FLDSND2, "Audio", "DispatchSound2", "command", "sound", "parameter1", "parameter2"),
        new(Jsm.Opcode.FLDSND3, "Audio", "DispatchSound3", "command", "sound", "parameter1", "parameter2", "parameter3"),
        new(Jsm.Opcode.SEPV, "Audio", "SetSoundPosition", "x", "z", "y", "volume"),
        new(Jsm.Opcode.SEPVA, "Audio", "SetSoundObjectPosition", "targetUid", "volume"),
        new(Jsm.Opcode.SONGFLAG, "Audio", "ContinueBattleMusic", "continueValue"),
        new(Jsm.Opcode.BEND, "Battle", "TerminateBattle"),
        new(Jsm.Opcode.BTLCMD, "Battle", "Attack", "attack"),
        new(Jsm.Opcode.BTLSEQ, "Battle", "AttackSpecial", "attack"),
        new(Jsm.Opcode.BTLSET, "Battle", "RunBattleCode", "code", "argument"),
        new(Jsm.Opcode.ENCOUNT, "Battle", "StartEncounter", "rushType", "battle"),
        new(Jsm.Opcode.ENCOUNT2, "Battle", "BattleEx", "rushType", "battleGroup", "battle"),
        new(Jsm.Opcode.ENCRATE, "Battle", "SetRandomBattleFrequency", "frequency"),
        new(Jsm.Opcode.ENCSCENE, "Battle", "SetRandomBattles", "pattern", "battle1", "battle2", "battle3", "battle4"),
        new(Jsm.Opcode.AUTOTURN, "Camera", "FollowFocus", "enable"),
        new(Jsm.Opcode.BGCACTIVE, "Camera", "EnableCameraServices", "enable", "time", "smoothness"),
        new(Jsm.Opcode.BGCHEIGHT, "Camera", "SetCameraFollowHeight", "height"),
        new(Jsm.Opcode.BGCLOCK, "Camera", "EnableCameraFollow"),
        new(Jsm.Opcode.BGCUNLOCK, "Camera", "DisableCameraFollow"),
        new(Jsm.Opcode.BGSRELEASE, "Camera", "ReleaseCamera", "time", "smoothness"),
        new(Jsm.Opcode.BGSSCROLL, "Camera", "ScrollTo", "x", "y", "frames", "smoothness"),
        new(Jsm.Opcode.BGVPORT, "Camera", "SetCameraBounds", "camera", "minX", "maxX", "minY", "maxY"),
        new(Jsm.Opcode.SETCAM, "Camera", "SetFieldCamera", "cameraId"),
        new(Jsm.Opcode.VRP, "Camera", "CalculateScreenOrigin"),
        new(Jsm.Opcode.TIMERCONTROL, "Clock", "RunTimer", "run"),
        new(Jsm.Opcode.TIMERDISPLAY, "Clock", "ShowTimer", "enable"),
        new(Jsm.Opcode.TIMERSET, "Clock", "ChangeTimerTime", "time"),
        new(Jsm.Opcode.CC, "Engine", "DefinePlayerCharacter"),
        new(Jsm.Opcode.DASHOFF, "Engine", "DisableRun"),
        new(Jsm.Opcode.DASHON, "Engine", "EnableRun"),
        new(Jsm.Opcode.GAMEOVER, "Engine", "GameOver"),
        new(Jsm.Opcode.MENU, "Engine", "Menu", "menuType", "menu"),
        new(Jsm.Opcode.MENUOFF, "Engine", "DisableMenu"),
        new(Jsm.Opcode.MENUON, "Engine", "EnableMenu"),
        new(Jsm.Opcode.NOP, "Engine", "YieldFrame"),
        new(Jsm.Opcode.UCOFF, "Engine", "DisableMove"),
        new(Jsm.Opcode.UCON, "Engine", "EnableMove"),
        new(Jsm.Opcode.WAIT, "Engine", "Wait", "frames"),
        new(Jsm.Opcode.AICON, "Events", "ControlActiveTimeEvent", "mode"),
        new(Jsm.Opcode.DISCCHANGE, "Field", "ChangeDisc", "destination"),
        new(Jsm.Opcode.HINT, "Field", "PreloadField", "unknown", "field"),
        new(Jsm.Opcode.MAPJUMP, "Field", "ChangeMap", "fieldId"),
        new(Jsm.Opcode.MOVQ, "Field", "ExitField"),
        new(Jsm.Opcode.RAIN, "Field", "SetWeather", "strength", "speed"),
        new(Jsm.Opcode.RESETMAPNAME, "Field", "ResetFieldName"),
        new(Jsm.Opcode.SETMAPNAME, "Field", "SetFieldName", "name"),
        new(Jsm.Opcode.CLEARKEYMASK, "Input", "RemoveControllerMask", "pad", "buttons"),
        new(Jsm.Opcode.FICON, "Input", "BubbleFromScript", "symbol"),
        new(Jsm.Opcode.HEREON, "Input", "ShowHereIcon", "show"),
        new(Jsm.Opcode.SETKEYMASK, "Input", "AddControllerMask", "pad", "buttons"),
        new(Jsm.Opcode.TWIST, "Input", "SetControlDirection", "arrowAngle", "analogicAngle"),
        new(Jsm.Opcode.DELETEALLCARD, "Inventory", "DeleteAllCards"),
        new(Jsm.Opcode.GILADD, "Inventory", "AddGil", "amount"),
        new(Jsm.Opcode.GILDELETE, "Inventory", "RemoveGil", "amount"),
        new(Jsm.Opcode.ITEMADD, "Inventory", "AddItem", "item", "amount"),
        new(Jsm.Opcode.ITEMDELETE, "Inventory", "RemoveItem", "item", "amount"),
        new(Jsm.Opcode.MINIGAME, "Inventory", "TetraMaster", "cardDeck"),
        new(Jsm.Opcode.DEBUGCC, "Legacy", "DebugControlCharacter"),
        new(Jsm.Opcode.DEBUGLOAD, "Legacy", "DebugLoad"),
        new(Jsm.Opcode.DEBUGSAVE, "Legacy", "DebugSave"),
        new(Jsm.Opcode.GLOBALCLEAR, "Legacy", "ClearGlobals"),
        new(Jsm.Opcode.LOCATE, "Legacy", "LegacyLocate", "parameter1", "parameter2"),
        new(Jsm.Opcode.MAPID, "Legacy", "LegacyMapid"),
        new(Jsm.Opcode.PPRINT, "Legacy", "PrintParallel"),
        new(Jsm.Opcode.PPRINTF, "Legacy", "PrintParallelFormatted"),
        new(Jsm.Opcode.PRINT1, "Legacy", "Print1"),
        new(Jsm.Opcode.PRINTF, "Legacy", "PrintFormatted"),
        new(Jsm.Opcode.pad0a, "Legacy", "Reserved0A"),
        new(Jsm.Opcode.pad0e, "Legacy", "Reserved0E"),
        new(Jsm.Opcode.pad0f, "Legacy", "Reserved0F"),
        new(Jsm.Opcode.pad11, "Legacy", "Reserved11"),
        new(Jsm.Opcode.pad13, "Legacy", "Reserved13"),
        new(Jsm.Opcode.pad15, "Legacy", "Reserved15"),
        new(Jsm.Opcode.pad17, "Legacy", "Reserved17"),
        new(Jsm.Opcode.pad19, "Legacy", "Reserved19"),
        new(Jsm.Opcode.rsv0c, "Legacy", "Reserved0C"),
        new(Jsm.Opcode.CHOOSEPARAM, "Messages", "EnableDialogChoices", "choiceList", "defaultChoice"),
        new(Jsm.Opcode.CLOSE, "Messages", "CloseWindow", "windowId"),
        new(Jsm.Opcode.CLOSEALL, "Messages", "CloseAllWindows"),
        new(Jsm.Opcode.MES, "Messages", "ShowAndWait", "windowId", "flags", "messageId"),
        new(Jsm.Opcode.MESA, "Messages", "ShowForActorAndWait", "speakerUid", "windowId", "flags", "messageId"),
        new(Jsm.Opcode.MESAN, "Messages", "ShowForActor", "speakerUid", "windowId", "flags", "messageId"),
        new(Jsm.Opcode.MESB, "Messages", "BattleDialog", "text"),
        new(Jsm.Opcode.MESN, "Messages", "Show", "windowId", "flags", "messageId"),
        new(Jsm.Opcode.MESVALUE, "Messages", "SetTextVariable", "variableId", "value"),
        new(Jsm.Opcode.NOINITMES, "Messages", "PreventWindowInit"),
        new(Jsm.Opcode.RAISE, "Messages", "RaiseWindows"),
        new(Jsm.Opcode.SETSIGNAL, "Messages", "SetDialogProgression", "progression"),
        new(Jsm.Opcode.WAITMES, "Messages", "WaitWindow", "windowId"),
        new(Jsm.Opcode.CLEARAP, "Party", "UnlearnAbility", "character", "ability"),
        new(Jsm.Opcode.CLEARSTATUS, "Party", "CureStatus", "character", "statuses"),
        new(Jsm.Opcode.FULLMEMBER, "Party", "SetPartyReserve", "charactersAvailable"),
        new(Jsm.Opcode.INCFROG, "Party", "AddFrog"),
        new(Jsm.Opcode.JOIN, "Party", "SetCharacterData", "character", "updateLevel", "equipmentSet", "category", "abilitySet"),
        new(Jsm.Opcode.MAXAP, "Party", "LearnAbility", "character", "ability"),
        new(Jsm.Opcode.PARTYDELETE, "Party", "RemoveParty", "character"),
        new(Jsm.Opcode.PARTYMENU, "Party", "ChooseMembers", "partySize", "lockedCharacters"),
        new(Jsm.Opcode.PLAYERNAME, "Party", "SetName", "character", "text"),
        new(Jsm.Opcode.SETHP, "Party", "SetHP", "character", "hp"),
        new(Jsm.Opcode.SETMP, "Party", "SetMP", "character", "mp"),
        new(Jsm.Opcode.SETROW, "Party", "SetRow", "character", "row"),
        new(Jsm.Opcode.SYNCPARTY, "Party", "UpdatePartyUID"),
        new(Jsm.Opcode.WINPOSE, "Party", "EnableVictoryPose", "character", "enable"),
        new((Jsm.Opcode)0x100, "Regions", "LegacySimulationStart", "parameter1", "parameter2"),
        new((Jsm.Opcode)0x101, "Regions", "SetLegacyWalkmeshFrame", "parameter1", "parameter2"),
        new((Jsm.Opcode)0x102, "Regions", "SetSimulationActive", "simulationId", "enabled"),
        new((Jsm.Opcode)0x103, "Regions", "SetSimulationFlags", "simulationId", "flags"),
        new((Jsm.Opcode)0x104, "Regions", "SetSimulationFloor", "simulationId", "floorId"),
        new((Jsm.Opcode)0x105, "Regions", "SetSimulationRate", "simulationId", "frameRate"),
        new((Jsm.Opcode)0x106, "Regions", "SetSimulationAlgorithm", "simulationId", "algorithm"),
        new((Jsm.Opcode)0x107, "Regions", "SetSimulationDelta", "simulationId", "minimum", "maximum"),
        new((Jsm.Opcode)0x108, "Regions", "SetSimulationAxis", "simulationId", "axis"),
        new((Jsm.Opcode)0x109, "Regions", "StartLegacyWalkmeshAnimation", "animationId", "parameter"),
        new((Jsm.Opcode)0x10A, "Regions", "SetWalkmeshAnimationFrame", "animationId", "frame"),
        new((Jsm.Opcode)0x10B, "Regions", "SetLegacyWalkmeshAnimationActive", "animationId", "enabled"),
        new((Jsm.Opcode)0x10C, "Regions", "SetLegacyWalkmeshAnimationFlags", "animationId", "flags"),
        new((Jsm.Opcode)0x10D, "Regions", "SetLegacyWalkmeshAnimationRate", "animationId", "frameRate"),
        new((Jsm.Opcode)0x10E, "Regions", "WaitForLegacyWalkmeshAnimations", "parameter1", "parameter2"),
        new((Jsm.Opcode)0x10F, "Regions", "WaitForLegacyWalkmeshAnimation", "animationId", "frame", "duration"),
        new((Jsm.Opcode)0x110, "Regions", "SetLegacyWalkmeshAnimationRange", "animationId", "startFrame", "endFrame"),
        new((Jsm.Opcode)0x111, "Regions", "SetLegacyWalkmeshAnimationVisible", "animationId", "visible"),
        new(Jsm.Opcode.BGIACTIVEF, "Regions", "EnablePath", "path", "enable"),
        new(Jsm.Opcode.BGIACTIVET, "Regions", "EnablePathTriangle", "triangleId", "enable"),
        new(Jsm.Opcode.BGIMASK, "Regions", "SetTriangleFlagMask", "flags"),
        new(Jsm.Opcode.TRACK, "Regions", "SetLastVertex", "x", "z"),
        new(Jsm.Opcode.TRACKADD, "Regions", "DrawRegionPushNew"),
        new(Jsm.Opcode.TRACKSTART, "Regions", "BeginPolygon", "x", "z"),
        new(Jsm.Opcode.DELETE, "Scripts", "DeleteObject", "uid"),
        new(Jsm.Opcode.DRET, "Scripts", "ReturnEntryFunctions", "entry"),
        new(Jsm.Opcode.ENDSEQ, "Scripts", "StopSharedScript"),
        new(Jsm.Opcode.NEW, "Scripts", "StartThread", "objectSlot", "uniqueId"),
        new(Jsm.Opcode.NEW2, "Scripts", "StartRegion", "objectSlot", "uniqueId"),
        new(Jsm.Opcode.NEW3, "Scripts", "StartActor", "objectSlot", "uniqueId"),
        new(Jsm.Opcode.REPLY, "Scripts", "ReplyAsync", "level", "scriptId"),
        new(Jsm.Opcode.REPLYEW, "Scripts", "ReplyAndWait", "level", "scriptId"),
        new(Jsm.Opcode.REPLYSW, "Scripts", "ReplyWhenReady", "level", "scriptId"),
        new(Jsm.Opcode.REQ, "Scripts", "RequestAsync", "level", "targetUid", "scriptId"),
        new(Jsm.Opcode.REQEW, "Scripts", "RequestAndWait", "level", "targetUid", "scriptId"),
        new(Jsm.Opcode.REQSW, "Scripts", "RequestWhenReady", "level", "targetUid", "scriptId"),
        new(Jsm.Opcode.Return, "Scripts", "ReturnFromScript"),
        new(Jsm.Opcode.STARTSEQ, "Scripts", "RunSharedScript", "entry"),
        new(Jsm.Opcode.WAITSEQ, "Scripts", "WaitSharedScript"),
        new(Jsm.Opcode.BGAACTIVE, "Tiles", "ActivateTileAnimation", "tileAnimation", "activate"),
        new(Jsm.Opcode.BGAANIME, "Tiles", "RunTileAnimation", "fieldAnimation", "frame"),
        new(Jsm.Opcode.BGAFLAG, "Tiles", "SetTileAnimationFlags", "tileAnimation", "flags"),
        new(Jsm.Opcode.BGAFRAME, "Tiles", "SetTileAnimationFrame", "tileAnimation", "frameId"),
        new(Jsm.Opcode.BGARANGE, "Tiles", "RunTileAnimationEx", "tileAnimation", "start", "end"),
        new(Jsm.Opcode.BGARATE, "Tiles", "SetTileAnimationSpeed", "tileAnimation", "speed"),
        new(Jsm.Opcode.BGAVISIBLE, "Tiles", "ResetTileAnimation", "tileAnimation", "visible"),
        new(Jsm.Opcode.BGAWAIT, "Tiles", "SetTileAnimationPause", "tileAnimation", "frameId", "time"),
        new(Jsm.Opcode.BGLACTIVE, "Tiles", "ShowTile", "tileBlock", "show"),
        new(Jsm.Opcode.BGLATTACH, "Tiles", "Attach", "tileBlock", "x", "y", "filterMode", "red", "green", "blue"),
        new(Jsm.Opcode.BGLCOLOR, "Tiles", "SetColor", "tileBlock", "cyan", "magenta", "yellow"),
        new(Jsm.Opcode.BGLLOOP, "Tiles", "MoveTileLoop", "tileBlock", "activate", "xLoop", "yLoop"),
        new(Jsm.Opcode.BGLLOOPTYPE, "Tiles", "SetTileLoopType", "tileBlock", "screenAnchored"),
        new(Jsm.Opcode.BGLMOVE, "Tiles", "SetTilePositionEx", "tileBlock", "positionX", "positionY", "positionCloseness"),
        new(Jsm.Opcode.BGLORIGIN, "Tiles", "SetTilePosition", "tileBlock", "positionX", "positionY"),
        new(Jsm.Opcode.BGLPARALLAX, "Tiles", "MoveTile", "tileBlock", "activate", "movementX", "movementY"),
        new(Jsm.Opcode.BGLSCROLLOFFSET, "Tiles", "MoveTileLoopWithOffset", "tileBlock", "enable", "speed", "offset", "isXMovement"),
        new(Jsm.Opcode.BGVALPHA, "Tiles", "SetTileLoopAlpha", "tileBlock", "unknownX", "unknownY"),
        new(Jsm.Opcode.BGVDEFINE, "Tiles", "DefineViewport", "viewportId", "x", "y", "width", "height"),
        new(Jsm.Opcode.BGVSET, "Tiles", "SetTileCamera", "tileBlock", "cameraId"),
        new(Jsm.Opcode.CLEARCOLOR, "Tiles", "SetBackgroundColor", "red", "green", "blue"),
        new(Jsm.Opcode.PRINTQUAD, "Legacy", "PrintQuad"),
        new(Jsm.Opcode.VIBACTIVE, "Vibration", "ActivateVibration", "activate"),
        new(Jsm.Opcode.VIBFLAG, "Vibration", "SetVibrationFlags", "flags"),
        new(Jsm.Opcode.VIBRANGE, "Vibration", "SetVibrationRange", "start", "end"),
        new(Jsm.Opcode.VIBRATE, "Vibration", "SetVibrationSpeed", "frameRate"),
        new(Jsm.Opcode.VIBSTART, "Vibration", "VibrateController", "frame"),
        new(Jsm.Opcode.VIBTRACK, "Vibration", "ActivateVibrationTrack", "track", "sample", "activate"),
        new(Jsm.Opcode.VIBTRACK1, "Vibration", "RunVibrationTrack", "track", "sample", "activate"),
        new(Jsm.Opcode.FMV, "VisualEffects", "ControlCinematic", "operation", "cinematicId", "parameter"),
        new(Jsm.Opcode.SHADOWAMP, "VisualEffects", "SetShadowAmplifier", "amplificationFactor"),
        new(Jsm.Opcode.SHADOWLOCK, "VisualEffects", "LockShadowRotation", "lockedRotation"),
        new(Jsm.Opcode.SHADOWOFF, "VisualEffects", "DisableShadow"),
        new(Jsm.Opcode.SHADOWOFFSET, "VisualEffects", "SetShadowOffset", "offsetX", "offsetY"),
        new(Jsm.Opcode.SHADOWON, "VisualEffects", "EnableShadow"),
        new(Jsm.Opcode.SHADOWSCALE, "VisualEffects", "SetShadowSize", "sizeX", "sizeY"),
        new(Jsm.Opcode.SHADOWUNLOCK, "VisualEffects", "UnlockShadowRotation"),
        new(Jsm.Opcode.SPS, "VisualEffects", "DispatchSpriteCommand", "effect", "command", "parameter1", "parameter2", "parameter3"),
        new(Jsm.Opcode.SPS2, "VisualEffects", "DispatchCompactSpriteCommand", "effect", "command", "parameter1", "parameter2", "parameter3"),
        new(Jsm.Opcode.WIPERGB, "VisualEffects", "FadeScreen", "mode", "frames", "unknown", "cyan", "magenta", "yellow"),
        new(Jsm.Opcode.AIM, "World", "FollowCameraAim"),
        new(Jsm.Opcode.EYE, "World", "FollowCameraEye"),
        new(Jsm.Opcode.WMAPJUMP, "World", "Enter", "worldMapId"),
        new(Jsm.Opcode.WPRM, "World", "RunWorldCode", "code", "argument"),
    };

    private static readonly (int Command, string Name, string First, string Second, string Third)[] SpriteCommands =
    {
        (130, "LoadSprite", "spriteId", "source", "unused"),
        (131, "SetSpriteAttributes", "flags", "enabled", "unused"),
        (135, "SetSpritePosition", "x", "negativeY", "z"),
        (140, "SetSpriteRotation", "x", "y", "z"),
        (145, "SetScale", "scale", "parameter2", "parameter3"),
        (150, "AttachSprite", "actorUid", "bone", "unused"),
        (155, "SetSpriteFade", "fade", "unused2", "unused3"),
        (156, "SetSpriteShader", "shader", "unused2", "unused3"),
        (160, "SetSpriteFrameRate", "rate", "unused2", "unused3"),
        (161, "SetSpriteFrame", "frame", "unused2", "unused3"),
        (165, "SetSpritePositionOffset", "x", "negativeY", "z"),
        (170, "SetSpriteDepthOffset", "depth", "unused2", "unused3")
    };

    private static readonly SourceInstruction[] SpriteOperations = SpriteCommands.SelectMany(command => new[]
    {
        new SourceInstruction(Jsm.Opcode.SPS, "VisualEffects", command.Name, "effect", command.First, command.Second, command.Third) { FixedOperand = (1, command.Command) },
        new SourceInstruction(Jsm.Opcode.SPS2, "VisualEffects", command.Name + "Compact", "effect", command.First, command.Second, command.Third) { FixedOperand = (1, command.Command) }
    }).ToArray();

    private static SourceInstruction Audio(Jsm.Opcode opcode, int command, string method, params string[] operands) =>
        new(opcode, "Audio", method, operands) { FixedOperand = (0, command) };

    private static readonly SourceInstruction[] AudioOperations =
    {
        Audio(Jsm.Opcode.FLDSND0, 0, "PlaySong", "sound"),
        Audio(Jsm.Opcode.FLDSND0, 256, "StopSong", "sound"),
        Audio(Jsm.Opcode.FLDSND0, 265, "StopCurrentSong", "sound"),
        Audio(Jsm.Opcode.FLDSND0, 1792, "LoadSong", "sound"),
        Audio(Jsm.Opcode.FLDSND0, 2048, "SuspendSong", "sound"),
        Audio(Jsm.Opcode.FLDSND0, 2304, "ResumeSong", "sound"),
        Audio(Jsm.Opcode.FLDSND0, 2566, "MarkSongJumpPoint", "sound"),
        Audio(Jsm.Opcode.FLDSND1, 16903, "SkipSongPhrase", "sound", "phrase"),
        Audio(Jsm.Opcode.FLDSND1, 16897, "SetSongVolume", "sound", "volume"),
        Audio(Jsm.Opcode.FLDSND2, 33537, "InterpolateSongVolume", "sound", "ticks", "volume"),
        Audio(Jsm.Opcode.FLDSND3, 50177, "FadeSongVolume", "sound", "ticks", "fromVolume", "toVolume"),
        Audio(Jsm.Opcode.FLDSND2, 40960, "PlayStream", "sound", "position", "reverberation"),
        Audio(Jsm.Opcode.FLDSND0, 8448, "StopStream", "sound"),
        Audio(Jsm.Opcode.FLDSND1, 25089, "SetStreamVolume", "sound", "volume"),
        Audio(Jsm.Opcode.FLDSND0, 4616, "IgnoreSoundEffect", "sound"),
        Audio(Jsm.Opcode.FLDSND3, 53248, "PlayEffect", "sound", "attributes", "position", "volume"),
        Audio(Jsm.Opcode.FLDSND1, 20736, "StopEffect", "sound", "attributes"),
        Audio(Jsm.Opcode.FLDSND2, 37377, "SetEffectVolume", "sound", "parameter1", "volume"),
        Audio(Jsm.Opcode.FLDSND2, 54017, "InterpolateEffectVolume", "sound", "ticks", "volume"),
        Audio(Jsm.Opcode.FLDSND3, 53376, "PlayResidentEffect", "sound", "timeMsec", "position", "volume"),
        Audio(Jsm.Opcode.FLDSND3, 53376 | 0x40, "PlayResidentEffectInSlot1", "sound", "timeMsec", "position", "volume"),
        Audio(Jsm.Opcode.FLDSND3, 54145, "InterpolateResidentEffectVolume", "sound", "attributes", "ticks", "volume"),
        Audio(Jsm.Opcode.FLDSND3, 54145 | 0x40, "InterpolateResidentEffectVolumeInSlot1", "sound", "attributes", "ticks", "volume"),
        Audio(Jsm.Opcode.FLDSND1, 20864, "StopResidentEffect", "sound", "timeMsec"),
        Audio(Jsm.Opcode.FLDSND1, 20864 | 0x40, "StopResidentEffectInSlot1", "sound", "timeMsec"),
        Audio(Jsm.Opcode.FLDSND0, 4489, "StopCurrentResidentEffect", "unusedSound"),
        Audio(Jsm.Opcode.FLDSND1, 21761, "SetAllEffectVolumes", "diagnosticSound", "volume"),
        Audio(Jsm.Opcode.FLDSND1, 25098, "SetMovieStreamVolume", "diagnosticSound", "volume"),
        Audio(Jsm.Opcode.FLDSND2, 33539, "InterpolateSongPitch", "sound", "ticks", "pitch"),
        Audio(Jsm.Opcode.FLDSND2, 34305, "InterpolateAllSongVolumes", "unusedSound", "ticks", "volume"),
        Audio(Jsm.Opcode.FLDSND2, 38401, "InterpolateAllEffectVolumes", "diagnosticSound", "ticks", "volume"),
        Audio(Jsm.Opcode.FLDSND3, 54017, "InterpolateEffectVolumeWithAttributes", "sound", "attributes", "ticks", "volume"),
        Audio(Jsm.Opcode.FLDSND3, 54021, "InterpolateEffectPosition", "sound", "attributes", "ticks", "position"),
        Audio(Jsm.Opcode.FLDSND3, 54147, "InterpolateResidentEffectPitch", "sound", "attributes", "ticks", "pitch"),
        Audio(Jsm.Opcode.FLDSND3, 54149, "InterpolateResidentEffectPosition", "sound", "attributes", "ticks", "position")
    };

    private static readonly IReadOnlyDictionary<Jsm.Opcode, SourceInstruction> ByOpcode = Instructions.ToDictionary(operation => operation.Opcode);
    private static readonly IReadOnlyDictionary<(Jsm.Opcode, int), SourceInstruction> BySpriteCommand =
        SpriteOperations.ToDictionary(operation => (operation.Opcode, operation.FixedOperand!.Value.Value));
    private static readonly IReadOnlyDictionary<(Jsm.Opcode, int), SourceInstruction> BySoundCommand =
        AudioOperations.ToDictionary(operation => (operation.Opcode, operation.FixedOperand!.Value.Value));

    public static bool TryGet(EVInstruction instruction, [NotNullWhen(true)] out SourceInstruction? operation)
    {
        if (instruction.Opcode is Jsm.Opcode.SPS or Jsm.Opcode.SPS2 &&
            instruction.Operands[1].Expression == null &&
            BySpriteCommand.TryGetValue((instruction.Opcode, instruction.Operands[1].Value), out operation))
            return true;
        if (instruction.Opcode is Jsm.Opcode.FLDSND0 or Jsm.Opcode.FLDSND1 or Jsm.Opcode.FLDSND2 or Jsm.Opcode.FLDSND3 &&
            instruction.Operands[0].Expression == null &&
            BySoundCommand.TryGetValue((instruction.Opcode, instruction.Operands[0].Value), out operation))
            return true;
        return ByOpcode.TryGetValue(instruction.Opcode, out operation);
    }

    // EBin.wait/commandCodeFlow and EventEngine.DoEventCode.
    public static Suspension SuspensionOf(Jsm.Opcode opcode) => opcode switch
    {
        Jsm.Opcode.WAIT or Jsm.Opcode.NOP => Suspension.Frames,
        Jsm.Opcode.REQSW or Jsm.Opcode.REPLYSW => Suspension.Retry,
        Jsm.Opcode.REQEW or Jsm.Opcode.REPLYEW => Suspension.RequestCompletion,
        Jsm.Opcode.MES or Jsm.Opcode.MESA or Jsm.Opcode.WAITMES or Jsm.Opcode.MESB => Suspension.Message,
        Jsm.Opcode.MESN or Jsm.Opcode.MESAN or Jsm.Opcode.CLOSE or Jsm.Opcode.MOVE or Jsm.Opcode.MOVA or
            Jsm.Opcode.MOVE3 or Jsm.Opcode.MOVJ or Jsm.Opcode.MOVH or Jsm.Opcode.MOVE3H or
            Jsm.Opcode.WAITANIM or Jsm.Opcode.DWAITANIM or Jsm.Opcode.WAITSEQ or
            Jsm.Opcode.WAITTURN or Jsm.Opcode.DWAITTURN or Jsm.Opcode.CHRCOLOR or Jsm.Opcode.JUMP3 => Suspension.Retry,
        Jsm.Opcode.DISCCHANGE or Jsm.Opcode.DRET or Jsm.Opcode.MENUOFF or Jsm.Opcode.MENU or Jsm.Opcode.PARTYMENU or
            Jsm.Opcode.RAIN or Jsm.Opcode.MINIGAME => Suspension.Host,
        _ => Suspension.Immediate
    };

    public static byte InferMask(Jsm.Opcode opcode, EVOperand[] operands)
    {
        int mask = 0;
        for (int index = 0; index < operands.Length; index++)
        {
            if (operands[index].Expression == null)
                continue;
            if (!EVOperands.UsesExpression(opcode, byte.MaxValue, index))
                throw new InvalidDataException($"Operand {index + 1} of {opcode} cannot use expression encoding.");
            mask |= 1 << index;
        }
        return (byte)mask;
    }

    public static void AppendDeclarations(StringBuilder source)
    {
        foreach (var group in Instructions.Concat(SpriteOperations).Concat(AudioOperations).GroupBy(operation => operation.Service))
        {
            source.AppendLine();
            source.AppendLine($"public static partial class {group.Key}");
            source.AppendLine("{");
            foreach (SourceInstruction operation in group)
            {
                string opcode = Enum.IsDefined(operation.Opcode) ? $"Opcode.{operation.Opcode}" : $"(Opcode)0x{(int)operation.Opcode:X}";
                string fixedOperand = operation.FixedOperand is { } fixedValue ? $", fixedIndex: {fixedValue.Index}, fixedValue: {fixedValue.Value}" : "";
                var parameters = operation.Operands.Select(name => $"Operand {name}").ToList();
                if (operation.HasMask)
                    parameters.Add("int mask = -1");
                source.AppendLine($"    [Instruction({opcode}{fixedOperand})]");
                bool immediate = SuspensionOf(operation.Opcode) == Suspension.Immediate;
                string result = immediate ? "void" : "EventAwaitable";
                var arguments = operation.Operands.ToList();
                if (operation.FixedOperand is { } fixedArgument)
                    arguments.Insert(fixedArgument.Index, fixedArgument.Value.ToString(System.Globalization.CultureInfo.InvariantCulture));
                arguments.Insert(0, opcode);
                string dispatch = immediate ? "Execute" : "ExecuteAsync";
                source.AppendLine($"    public static {result} {operation.Method}({string.Join(", ", parameters)}) => EventScheduler.{dispatch}({string.Join(", ", arguments)});");
            }
            if (group.Key == "Regions")
            {
                source.AppendLine("    [Instruction(Opcode.QUAD)]");
                source.AppendLine("    public static void DefinePolygon(Operand vertexCount, int mask, params Vertex[] vertices) => EventScheduler.DefinePolygon(vertexCount, vertices);");
            }
            source.AppendLine("}");
        }
        AppendActorService("ActorAnimation", instruction => instruction.UsesActingAnimation && instruction.Opcode != Jsm.Opcode.AIDLE);
        AppendActorService("ActorActions", instruction => instruction.UsesActingActor);

        source.AppendLine();
        source.AppendLine("public static class SoundCommands");
        source.AppendLine("{");
        foreach (var (number, name) in SoundCommands)
        {
            source.AppendLine($"    public const int {name} = {number};");
            source.AppendLine($"    public const int {name}Slot1 = {number | 0x40};");
        }
        source.AppendLine("}");

        void AppendActorService(string className, Func<SourceInstruction, bool> accepts)
        {
            source.AppendLine();
            source.AppendLine($"public sealed partial class {className}");
            source.AppendLine("{");
            foreach (SourceInstruction operation in Instructions.Where(accepts))
            {
                var parameters = operation.Operands.Select(name => $"Operand {name}").ToList();
                var arguments = operation.Operands.ToList();
                if (operation.HasMask)
                {
                    parameters.Add("int mask = -1");
                    arguments.Add("mask");
                }
                bool immediate = SuspensionOf(operation.Opcode) == Suspension.Immediate;
                string result = immediate ? "void" : "EventAwaitable";
                source.AppendLine($"    [Instruction(Opcode.{operation.Opcode})]");
                source.AppendLine($"    public {result} {operation.Method}({string.Join(", ", parameters)})");
                source.AppendLine("    {");
                source.AppendLine("        RequireActing();");
                source.AppendLine($"        {(immediate ? "" : "return ")}{operation.Service}.{operation.Method}({string.Join(", ", arguments)});");
                source.AppendLine("    }");
                source.AppendLine();
            }
            source.AppendLine("}");
        }
    }

    public bool UsesActingAnimation => Service == "Animations" && !Operands.Contains("targetUid");

    public bool UsesActingActor => Opcode is Jsm.Opcode.MODEL or Jsm.Opcode.BGI or Jsm.Opcode.DIRE or
        Jsm.Opcode.MOVE or Jsm.Opcode.MOVE3 or Jsm.Opcode.MOVA or Jsm.Opcode.MOVH or Jsm.Opcode.MOVE3H or
        Jsm.Opcode.MROT or Jsm.Opcode.MSPEED or Jsm.Opcode.NECKFLAG or Jsm.Opcode.NECKID or Jsm.Opcode.OVAL or
        Jsm.Opcode.POS or Jsm.Opcode.POS3 or Jsm.Opcode.RADIUS or Jsm.Opcode.ROTXZ or Jsm.Opcode.SPEEDTH or
        Jsm.Opcode.TSPEED or Jsm.Opcode.TURN or Jsm.Opcode.WAITTURN;

    public static readonly IReadOnlyDictionary<int, string> SoundCommands = new Dictionary<int, string>
    {
        [0] = "SongPlay",
        [1792] = "SongLoad",
        [2048] = "SongSuspend",
        [2304] = "SongResume",
        [2566] = "SongJumpPoint",
        [16903] = "SongSkipPhrase",
        [256] = "SongStop",
        [265] = "SongStopCurrent",
        [16897] = "SongVol",
        [50177] = "SongVolFade",
        [51969] = "SongVolFadeAll",
        [33537] = "SongVolIntpl",
        [34305] = "SongVolIntplAll",
        [16899] = "SongPitch",
        [33539] = "SongPitchIntpl",
        [50179] = "SongPitchFade",
        [40960] = "StreamPlay",
        [8448] = "StreamStop",
        [25089] = "StreamVol",
        [53248] = "SfxPlay",
        [20736] = "SfxStop",
        [37377] = "SfxVol",
        [54017] = "SfxVolIntpl",
        [21761] = "SfxVolAll",
        [38401] = "SfxVolIntplAll",
        [37379] = "SfxPitch",
        [4489] = "SfxResidentStopCurrent",
        [6272] = "SfxResidentSuspend",
        [6528] = "SfxResidentRestore",
        [20864] = "SfxResidentStop",
        [21889] = "SfxResidentVolAll",
        [37505] = "SfxResidentVol",
        [38529] = "SfxResidentVolIntplAll",
        [53376] = "SfxResidentPlay",
        [54145] = "SfxResidentVolIntpl",
        [3072] = "Sync",
        [30464] = "InstrLoad",
        [520] = "SongNull",
        [16898] = "SongTempo",
        [16900] = "SongTempoPitch",
        [1028] = "SongTempoPitchFade",
        [33540] = "SongTempoPitchIntpl",
        [50178] = "SongTempoFade",
        [33538] = "SongTempoIntpl",
        [8712] = "StreamNull",
        [25093] = "StreamPos",
        [25098] = "StreamFmvVol",
        [25100] = "StreamReverb",
        [4616] = "SfxNull",
        [21763] = "SfxPitchAll",
        [54019] = "SfxPitchIntpl",
        [38403] = "SfxPitchIntplAll",
        [37381] = "SfxPos",
        [21765] = "SfxPosAll",
        [54021] = "SfxPosIntpl",
        [38405] = "SfxPosIntplAll",
        [37507] = "SfxResPitch",
        [21891] = "SfxResPitchAll",
        [54147] = "SfxResPitchIntpl",
        [38531] = "SfxResPitchIntplAll",
        [37509] = "SfxResPos",
        [21893] = "SfxResPosAll",
        [54149] = "SfxResPosIntpl",
        [38533] = "SfxResPosIntplAll",
    };
}

internal enum Suspension { Immediate, Frames, Retry, RequestCompletion, Message, Host }
