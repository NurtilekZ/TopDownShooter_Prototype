using System;
using _current.Core.Logic;
using _current.Utils;
using UnityEngine;

namespace _current.Core.Pawns.Components
{
    public abstract class AnimationHandlerBase : PawnComponent, IAnimationStateReader
    {
        [SerializeField] protected Animator animator;
        [SerializeField] protected float animDampTime;
        [SerializeField] protected float rotationDampTime;
        
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

        public void SetAnimatorOverride(AnimatorOverrideController overrideController)
        {
            animator.runtimeAnimatorController = overrideController;
        }

        public void PlayDeath() => animator.SetTrigger(AnimationStatics.Death);
        public void PlayReload() => animator.SetTrigger(AnimationStatics.Reload);
        
        public void SetIdle(bool isIdle) => animator.SetBool(AnimationStatics.Idle, isIdle);
        public void SetFreeFall(bool value) => animator.SetBool(AnimationStatics.FreeFall, value);
        public void SetIsGrounded(bool value) => animator.SetBool(AnimationStatics.Grounded, value);
        public void PlayHit() => animator.SetTrigger(AnimationStatics.Hit);
    }
}