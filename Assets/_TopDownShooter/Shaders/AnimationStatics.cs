using UnityEngine;

namespace Shaders
{
    public static class AnimationStatics
    {
        public static readonly int Vertical = Animator.StringToHash("Vertical");
        public static readonly int Horizontal = Animator.StringToHash("Horizontal");
        public static readonly int Crouching = Animator.StringToHash("Crouching");
        public static readonly int Sprinting = Animator.StringToHash("Sprinting");
        public static readonly int Movement = Animator.StringToHash("Moving");
        public static readonly int Idle = Animator.StringToHash("Idle");
        public static readonly int Shoot = Animator.StringToHash("Shoot");
        public static readonly int Reload = Animator.StringToHash("Reload");
        public static readonly int Hit = Animator.StringToHash("Hit");
        public static readonly int Death = Animator.StringToHash("Death");
    }
}