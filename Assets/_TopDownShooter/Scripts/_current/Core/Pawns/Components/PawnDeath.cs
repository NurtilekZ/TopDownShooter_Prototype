using System;
using UniRx;
using Unity.Mathematics;
using UnityEngine;

namespace _current.Core.Pawns.Components
{
    [RequireComponent(typeof(PawnHealth))]
    public abstract class PawnDeath : PawnComponent
    {
        [SerializeField] private PawnHealth _health;
        [SerializeField] private AnimationHandlerBase _animationHandler;
        [SerializeField] private GameObject _deathFx;
        
        protected readonly ReactiveProperty<bool> _isDead = new(false);
        
        public ReactiveProperty<bool> IsDead => _isDead;
        public event Action<PawnDeath> OnDeath;

        protected override void Bind()
        {
            _health.CurrentHealth.Subscribe(CheckHealth).AddTo(_disposables);
        }

        protected override void Unbind()
        {
            _isDead?.Dispose();
        }

        private void CheckHealth(float value)
        {
            if (!_isDead.Value && value <=0) 
                Die();
        }

        protected virtual void Die()
        {
            _isDead.Value = true;
            OnDeath?.Invoke(this);
            _animationHandler.PlayDeath();
            if (_deathFx) 
                Instantiate(_deathFx, transform);
        }

        [ContextMenu("Revive")]
        public void Revive()
        {
            _health.Revive();
            _isDead.Value = false;
            enabled = true;
        }
    }
}