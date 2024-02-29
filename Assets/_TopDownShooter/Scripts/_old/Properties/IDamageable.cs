using System;

namespace _old.Properties
{
    public interface IDamageable
    {
        event Action<DamageSender, float> OnTakeDamage;

        void TakeDamage(DamageSender sender, float damage);
    }
}