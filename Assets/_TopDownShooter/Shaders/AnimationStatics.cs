using UnityEngine;

namespace Shaders
{
    public static class AnimationStatics
    {
        public static readonly int Vertical = Animator.StringToHash("Vertical");
        public static readonly int Horizontal = Animator.StringToHash("Horizontal");
        public static readonly int Rotation = Animator.StringToHash("Rotation");
        public static readonly int Sprinting = Animator.StringToHash("Sprinting");
        public static readonly int Movement = Animator.StringToHash("Moving");
        public static readonly int Idle = Animator.StringToHash("Idle");
        public static readonly int Shoot = Animator.StringToHash("Shoot");
        public static readonly int Reload = Animator.StringToHash("Reload");
        public static readonly int Hit = Animator.StringToHash("Hit");
        public static readonly int Death = Animator.StringToHash("Death");
        public static readonly int Grounded = Animator.StringToHash("Grounded");
        public static readonly int PreGrounded = Animator.StringToHash("PreGrounded");
        public static readonly int Jump = Animator.StringToHash("Jump");
        public static readonly int FreeFall = Animator.StringToHash("FreeFall");
        public static readonly int Walking = Animator.StringToHash("Walking");
    }
}