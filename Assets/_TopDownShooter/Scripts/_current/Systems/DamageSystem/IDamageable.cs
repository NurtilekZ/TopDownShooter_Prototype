using System;
using UnityEngine;

namespace _current.Systems.DamageSystem
{
    public interface IDamageable
    {
        public int CurrentHealth { get; }
        public int MaxHealth { get; }
        bool IsAlive { get; }
        event Action<int> OnTakeDamage;
        event Action<Vector3> OnDeath;

        void TakeDamage(IDamageSender sender, int damage);
    }
}