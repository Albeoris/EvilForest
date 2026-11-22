using Memoria.EventEngine.EV;

namespace EveilForest.CSharp;

internal static class CargoStorageNames
{
    private static readonly Dictionary<(EVStorageType Type, int Address), string> Location = new()
    {
        [(EVStorageType.Int16, 0)] = "SpawnX",
        [(EVStorageType.Int16, 2)] = "SpawnDepth3D",
        [(EVStorageType.Int16, 4)] = "SpawnPlaneDepthOrHeight3D",
        [(EVStorageType.Int16, 6)] = "SpawnFacing",
        [(EVStorageType.Int16, 8)] = "PartyCandidate0OrMemberIndex",
        [(EVStorageType.Int16, 10)] = "PartyCandidate1OrCurrentMask",
        [(EVStorageType.Int16, 12)] = "PartyCandidate2OrRequiredMask",
        [(EVStorageType.Int16, 14)] = "PartyCandidate3OrDifferenceMask",
        [(EVStorageType.Byte, 17)] = "ScreenFadeParameter",
        [(EVStorageType.Int16, 21)] = "PreloadedFieldId",
        [(EVStorageType.Byte, 24)] = "SceneStep",
        [(EVStorageType.Byte, 26)] = "SceneActorsRemaining",
        [(EVStorageType.Byte, 30)] = "CandleLightEnabled",
        [(EVStorageType.Byte, 31)] = "ZidaneTintRed",
        [(EVStorageType.Byte, 32)] = "ZidaneTintGreen",
        [(EVStorageType.Byte, 33)] = "ZidaneTintBlue",
        [(EVStorageType.UInt16, 34)] = "MainTileBrightness",
        [(EVStorageType.UInt16, 36)] = "Tile17BaseBrightness",
        [(EVStorageType.Byte, 38)] = "InsideCandleInteractionArea",
        [(EVStorageType.Int16, 40)] = "CandleHintTimer",
        [(EVStorageType.Byte, 42)] = "CandlePromptState",
        [(EVStorageType.Byte, 43)] = "SceneStep10ActorsReleased",
        [(EVStorageType.Byte, 44)] = "StrategyMusicStartSignal",
        [(EVStorageType.Bit, 144)] = "KeepMenuStateDuringControlChanges",
        [(EVStorageType.Bit, 145)] = "SuppressRewardInteraction",
        [(EVStorageType.Bit, 147)] = "LastPartyAddSucceeded",
        [(EVStorageType.Bit, 148)] = "RestoreCandidate0FromKnockout",
        [(EVStorageType.Bit, 149)] = "RestoreCandidate1FromKnockout",
        [(EVStorageType.Bit, 150)] = "RestoreCandidate2FromKnockout",
        [(EVStorageType.Bit, 151)] = "RestoreCandidate3FromKnockout",
        [(EVStorageType.Bit, 155)] = "RestoreActiveTimeEventOnReentry",
        [(EVStorageType.Bit, 156)] = "SuppressMovementRestore",
        [(EVStorageType.Bit, 158)] = "PlayerControlRequested",
        [(EVStorageType.Bit, 159)] = "PlayerControlInitialized",
        [(EVStorageType.Bit, 162)] = "KeepResidentSoundSlot0OnExit",
        [(EVStorageType.Bit, 163)] = "ReservedExitSoundFlag",
        [(EVStorageType.Bit, 167)] = "StopCurrentSongOnExit",
        [(EVStorageType.Bit, 230)] = "SceneActorSyncRequested",
        [(EVStorageType.Bit, 231)] = "AdvanceSceneStepRequested"
    };

    public static string? Lookup(string? eventName, int? slot, EVVariable view)
    {
        if (!string.Equals(eventName, "EVT_ALEX1_TS_CARGO_0", StringComparison.OrdinalIgnoreCase))
            return null;

        if (view.Scope == EVStorageScope.Map)
            return Location.GetValueOrDefault((view.Type, view.Address));

        if (view.Scope != EVStorageScope.Instance)
            return null;

        return (slot, view.Type, view.Address) switch
        {
            (2, EVStorageType.Byte, 0) => "CandleTileBrightness",
            (2, EVStorageType.Byte, 1) => "CandleFlickerPhase",
            (8 or 9 or 10, EVStorageType.Int24, 0) => "RewardDistanceSquared",
            (10, EVStorageType.Byte, 3) => "RewardResult",
            (11, EVStorageType.UInt16, 0) => "Tile17FlickerOffset",
            _ => null
        };
    }
}
