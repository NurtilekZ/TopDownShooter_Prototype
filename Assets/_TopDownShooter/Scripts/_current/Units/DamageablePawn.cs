using System;
using _current.Systems.DamageSystem;
using UnityEngine;

namespace _current.Units
{
    public abstract class DamageablePawn : MonoBehaviour, IDamageable
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

        public bool IsAlive { get; protected set; } = true;

        public event Action<int> OnTakeDamage;
        public event Action<Vector3> OnDeath;

        public virtual void TakeDamage(IDamageSender sender, int damage)
        {
            OnTakeDamage?.Invoke(damage);
        }
    }
}