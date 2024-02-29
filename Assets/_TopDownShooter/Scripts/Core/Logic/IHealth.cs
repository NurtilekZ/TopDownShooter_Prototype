using UniRx;

namespace Core.Logic
{
    public interface IHealth
    {
        FloatReactiveProperty CurrentHP { get; set; }
        float MaxHP { get; set; }

        void TakeDamage(float damage);
    }
}