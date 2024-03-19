using System;

namespace _current.Properties
{
    public abstract class DamageSender : BasePawn, IDamageSender
    {
        public event Action<DamageSender, IDamageable, float> OnDoDamage;

        public virtual void DoDamage(IDamageable target, float damage)
        {
            OnDoDamage?.Invoke(this, target, damage);
        }
    }
}