using UnityEngine;

namespace _current.Utils
{
    public static class AnimationStatics
    {
        //Parameters
        //Common
        public static readonly int Idle = Animator.StringToHash("Idle");
        public static readonly int Hit = Animator.StringToHash("Hit");
        public static readonly int Death = Animator.StringToHash("Death");
        //Pawns
        public static readonly int Vertical = Animator.StringToHash("Vertical");
        public static readonly int Horizontal = Animator.StringToHash("Horizontal");
        public static readonly int Rotation = Animator.StringToHash("Rotation");
        public static readonly int Sprinting = Animator.StringToHash("Sprinting");
        public static readonly int Attack = Animator.StringToHash("Attack");
        public static readonly int Reload = Animator.StringToHash("Reload");
        public static readonly int Grounded = Animator.StringToHash("Grounded");
        public static readonly int Jump = Animator.StringToHash("Jump");
        public static readonly int FreeFall = Animator.StringToHash("FreeFall");
        public static readonly int Walking = Animator.StringToHash("Walking");
        public static readonly int MotionSpeed = Animator.StringToHash("MotionSpeed");
        //Spawners
        public static readonly int Spawn = Animator.StringToHash("Spawn");
        
        //States
        public static readonly int IdleState = Animator.StringToHash("MotionSpeed");
        public static readonly int MovementState = Animator.StringToHash("MotionSpeed");
        public static readonly int JumpingState = Animator.StringToHash("MotionSpeed");
        public static readonly int InAirState = Animator.StringToHash("MotionSpeed");
        public static readonly int LandingState = Animator.StringToHash("MotionSpeed");
        public static readonly int AttackState = Animator.StringToHash("MotionSpeed");
        public static readonly int DeathState = Animator.StringToHash("MotionSpeed");
    }
}