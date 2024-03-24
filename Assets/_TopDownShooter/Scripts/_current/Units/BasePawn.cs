using System;
using System.Collections.Generic;
using _current.Systems.DamageSystem;
using UnityEngine;

namespace _current.Units
{
    public abstract class BasePawn : MonoBehaviour, IDamageable, IDamageSender
    {
        [SerializeField] private int _currentHealth;
        [SerializeField] private int _maxHealth;

        public int CurrentHealth
        {
            get => _currentHealth;
            private set => _currentHealth = value;
        }

        public int MaxHealth
        {
            get => _maxHealth;
            private set => _maxHealth = value;
        }

        public abstract bool IsAlive { get; protected set; }

        public event Action<IDamageSender, IDamageable, int> OnDoDamage;
        public event Action<int> OnTakeDamage;
        public event Action<Vector3> OnDeath;

        protected readonly List<IDisposable> _disposables = new();

        protected virtual void OnEnable()
        {
            _currentHealth = _maxHealth;
        }

        public virtual void DoDamage(IDamageable target, int damage)
        {
            OnDoDamage?.Invoke(this, target, damage);
        }
        
        public virtual void TakeDamage(IDamageSender sender, int damage)
        {
            var damageTaken = Mathf.Clamp(damage, 0, _currentHealth);

            _currentHealth -= damageTaken;

            if (damageTaken != 0)
            {
                OnTakeDamage?.Invoke(damage);
            }

            if (_currentHealth == 0 && damage != 0)
            {
                IsAlive = false;
                OnDeath?.Invoke(transform.position);
            }
        }
        
        public virtual void Dispose()
        {
            foreach (var disposable in _disposables) 
                disposable.Dispose();
        }
    }
}