using System;

namespace _old.Properties
{
    public interface IDamageSender : IDisposable
    {
        event Action<DamageSender, IDamageable, float> OnDoDamage;
        void DoDamage(IDamageable target, float damage);
    }
}