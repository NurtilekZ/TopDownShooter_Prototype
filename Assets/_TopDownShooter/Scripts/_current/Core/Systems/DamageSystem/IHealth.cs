using System;
using UniRx;

namespace _current.Core.Systems.DamageSystem
{
    public interface IHealth : IDisposable
    {
        public IReadOnlyReactiveProperty<float> CurrentHealth { get; }
        public float MaxHealth { get; }
        event Action<float> OnTakeDamage;
        void TakeDamage(float damage, IDamageSender sender);
        void Initialize(int hp);
    }
}