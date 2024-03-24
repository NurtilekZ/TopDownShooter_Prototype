using System;

namespace _current.Systems.DamageSystem
{
    public interface IDamageSender : IDisposable
    {
        event Action<IDamageSender, IDamageable, int> OnDoDamage;
        void DoDamage(IDamageable target, int damage);
    }
}