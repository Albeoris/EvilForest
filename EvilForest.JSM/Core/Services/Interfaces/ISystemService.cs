using System;
using EvilForest.Resources.Enums;

namespace FF8.Core
{
    public interface ISystemService
    {
        void InitService(Int32 objectIndex, Int32 uid);
        void InitRegion(Int32 objectIndex, Int32 uid);
        void InitActor(Int32 objectIndex, Int32 uid);
        void PullActorScreenPosition(Int32 objectIndex); // Inits System.ScreenActorX/Y
        IAwaitable Wait(Int32 frameDuration);

        public Int32 this[SystemData kind] { get; }
    }
}