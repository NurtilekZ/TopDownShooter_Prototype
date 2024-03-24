using System;
using _current.Systems.DamageSystem;
using UnityEngine;

namespace _current.Units
{
    public abstract class DamageSenderPawn : MonoBehaviour, IDamageSender
    {
        public event Action<IDamageSender, IDamageable, int> OnDoDamage;
        public void DoDamage(IDamageable target, int damage)
        {
            OnDoDamage?.Invoke(this, target, damage);
        }

        public void Dispose()
        {
            
        }
    }
}