using System;
using Core.Logic;
using Shaders;
using UnityEngine;

namespace Core.Enemy
{
    public class EnemyAnimator : MonoBehaviour, IAnimationStateReader
    {
        [SerializeField] private Animator _animator;

        public event Action<AnimatorState> StateEntered; 
        public event Action<AnimatorState> StateExited; 
        public AnimatorState State { get; private set; }
        
        #region Play Methods

        public void PlayMove(float velocity) =>
            _animator.SetFloat(AnimationStatics.Movement, velocity);

        public void PlayAttack() =>
            _animator.SetTrigger(AnimationStatics.Shoot);

        public void PlayHit() =>
            _animator.SetTrigger(AnimationStatics.Hit);

        public void PlayDie() =>
            _animator.SetTrigger(AnimationStatics.Death);

        #endregion

        public void OnEnter(int stateHash)
        {
            State = StateFor(stateHash);
            StateEntered?.Invoke(State);
        }

        public void OnExit(int stateHash)
        {
            StateExited?.Invoke(State);
        }
        
        private AnimatorState StateFor(int stateHash)
        {
            AnimatorState state;

            if      (stateHash == AnimationStatics.Idle)    state = AnimatorState.Idle;
            else if (stateHash == AnimationStatics.Shoot)  state = AnimatorState.Attack;
            else if (stateHash == AnimationStatics.Movement) state = AnimatorState.Walking;
            else if (stateHash == AnimationStatics.Hit)     state = AnimatorState.Hit;
            else if (stateHash == AnimationStatics.Death)   state = AnimatorState.Death;
            else                                     state = AnimatorState.Unknown;

            return state;
        }
    }
}