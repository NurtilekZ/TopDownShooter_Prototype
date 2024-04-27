using System;
using _current.Core.Systems.DamageSystem;
using UniRx;
using UnityEngine;

namespace _current.Core.Pawns.Components
{
    public abstract class PawnHealth : PawnComponent, IHealth
    {
        [SerializeField] private AnimationHandlerBase _animationHandler;
        [SerializeField] protected ReactiveProperty<float> _currentHealth = new(100f);
        [SerializeField] protected float _maxHealth = 100f;
        
        public float MaxHealth => _maxHealth;

        public IReadOnlyReactiveProperty<float> CurrentHealth => _currentHealth;

        public event Action<float> OnTakeDamage;

        public void Initialize(int hp)
        {
            _maxHealth = hp;
            _currentHealth.Value = hp;
        }

        protected override void Bind()
        {
            _currentHealth.Value = _maxHealth;
        }

        protected override void Unbind()
        {
            _currentHealth?.Dispose();
        }

        public virtual void TakeDamage(float damage, IDamageSender sender)
        {
            var damageTaken = Mathf.Clamp(damage, 0, _currentHealth.Value);

            _currentHealth.Value -= damageTaken;

            if (damageTaken != 0)
            {
                OnTakeDamage?.Invoke(damage);
                _animationHandler.PlayHit();
            }
        }

        public void Revive()
        {
            _currentHealth.Value = _maxHealth;
        }
    }
}