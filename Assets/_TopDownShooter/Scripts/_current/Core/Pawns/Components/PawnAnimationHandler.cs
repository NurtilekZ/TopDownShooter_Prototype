using _current.Utils;
using UnityEngine;

namespace _current.Core.Pawns.Components
{
    public class PawnAnimationHandler : AnimationHandlerBase
    {
        public void UpdateMovements(Vector3 destination, float dampTime = 0)
        {
            var direction = transform.InverseTransformDirection(destination);
            var damp = dampTime == 0 ? animDampTime : dampTime;

            animator.SetFloat(AnimationStatics.Horizontal, direction.x, damp, Time.deltaTime);
            animator.SetFloat(AnimationStatics.Vertical, direction.z, damp, Time.deltaTime);
        }

        public void UpdateMotionSpeed(float animMotionSpeed)
        {
            animator.SetFloat(AnimationStatics.MotionSpeed, animMotionSpeed);
        }

        public void UpdateRotation(float angle)
        {
            animator.SetFloat(AnimationStatics.Rotation, angle, rotationDampTime, Time.deltaTime);
        }

        public void PlayAttack() => animator.SetTrigger(AnimationStatics.Attack);
        public void PlayJump() => animator.SetTrigger(AnimationStatics.Jump);
        public void SetIsWalking(bool value) => animator.SetBool(AnimationStatics.Walking, value);
        public void SetIsSprinting(bool value) => animator.SetBool(AnimationStatics.Sprinting, value);
        public void SetIsAttacking(bool value) => animator.SetBool(AnimationStatics.Attack, value);
    }
}