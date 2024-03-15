using System;
using UnityEngine;

namespace _old.Properties
{
    public abstract class DamagablePawn : MonoBehaviour, IDamageable
    {
        public event Action<DamageSender, float> OnTakeDamage;

        public virtual void TakeDamage(DamageSender sender, float damage)
        {
            OnTakeDamage?.Invoke(sender, damage);
        }
    }
}