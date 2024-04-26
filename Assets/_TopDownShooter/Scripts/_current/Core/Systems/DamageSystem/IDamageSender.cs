using System;

namespace _current.Core.Systems.DamageSystem
{
    public interface IDamageSender : IDisposable
    {
        public event Action<IDamageSender, float> OnAttack;
    }
}