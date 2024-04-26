using System;
using _current.Core.Logic;
using _current.Utils;
using UnityEngine;

namespace _current.Core.Pawns.Components
{
    public class AnimationHandler : PawnComponent, IAnimationStateReader
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private float _animDampTime;
        [SerializeField] private float _rotationDampTime;
        
        public event Action<AnimatorState> StateEntered;
        public event Action<AnimatorState> StateExited;
        
        public AnimatorState State { get; private set; }

        protected override void Bind() { }
        protected override void Unbind() { }

        public void OnEnter(int stateHash)
        {
            State = StateFor(stateHash);
            StateEntered?.Invoke(State);
        }

        public void OnExit(int stateHash) => 
            StateExited?.Invoke(StateFor(stateHash));

        private AnimatorState StateFor(int stateHash)
        {
            AnimatorState state = AnimatorState.Idle;
            if (stateHash == AnimationStatics.IdleState)
                state = AnimatorState.Idle;
            else if (stateHash == AnimationStatics.AttackState)
                state = AnimatorState.Attack;
            else if (stateHash == AnimationStatics.MovementState)
                state = AnimatorState.Walking;
            else if (stateHash == AnimationStatics.DeathState)
                state = AnimatorState.Death;
            
            return state;
        }

        public void UpdateMovements(Vector3 destination, float dampTime = 0)
        {
            var direction = transform.InverseTransformDirection(destination);
            var damp = dampTime == 0 ? _animDampTime : dampTime;
            
            _animator.SetFloat(AnimationStatics.Horizontal, direction.x, damp, Time.deltaTime);
            _animator.SetFloat(AnimationStatics.Vertical, direction.z, damp, Time.deltaTime);
        }

        public void UpdateMotionSpeed(float animMotionSpeed)
        {
            _animator.SetFloat(AnimationStatics.MotionSpeed, animMotionSpeed);
        }
        
        public void UpdateRotation(float angle)
        {
            _animator.SetFloat(AnimationStatics.Rotation, angle, _rotationDampTime, Time.deltaTime);
        }
        
        public void SetAnimatorOverride(AnimatorOverrideController overrideController)
        {
            _animator.runtimeAnimatorController = overrideController;
        }

        public void PlayAttack() => _animator.SetTrigger(AnimationStatics.Attack);
        public void PlayHit() => _animator.SetTrigger(AnimationStatics.Hit);
        public void PlayDeath() => _animator.SetTrigger(AnimationStatics.Death);
        public void PlayJump() => _animator.SetTrigger(AnimationStatics.Jump);
        public void PlayReload() => _animator.SetTrigger(AnimationStatics.Reload);
        
        public void SetIdle(bool isIdle) => _animator.SetBool(AnimationStatics.Idle, isIdle);
        public void SetIsWalking(bool value) => _animator.SetBool(AnimationStatics.Walking, value);
        public void SetIsSprinting(bool value) => _animator.SetBool(AnimationStatics.Sprinting, value);
        public void SetIsAttacking(bool value) => _animator.SetBool(AnimationStatics.Attack, value);
        public void SetFreeFall(bool value) => _animator.SetBool(AnimationStatics.FreeFall, value);
        public void SetIsGrounded(bool value) => _animator.SetBool(AnimationStatics.Grounded, value);
    }
}