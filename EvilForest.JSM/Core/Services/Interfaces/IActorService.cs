using System;

namespace FF8.Core
{
    public interface IActorService
    {
        void SetModel(String modelName, Int32 height);
        void SetPosition(Int32 x, Int32 y);
        void SetAngle(Int32 angle);
        void SetAnimation(AnimationKind kind, String animationId);
        void SetRadius(Int32 walkRadius, Int32 collisionRadius, Int32 talkDistance);
        void SetIdleSpeed(Int32 speed1, Int32 speed2, Int32 speed3, Int32 speed4);
        
        void Turn(Int32 angle, Int32 speed);
        IAwaitable WaitTurnComplete();

        void Animate(Int32 animationId);
        IAwaitable WaitAnimationComplete();
    }

    public enum AnimationKind
    {
        Idle = 1,
        Sleep,
        Walk,
        Run,
        TurnLeft,
        TurnRight
    }
}