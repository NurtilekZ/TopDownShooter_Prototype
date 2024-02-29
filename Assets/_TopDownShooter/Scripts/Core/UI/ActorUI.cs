using Core.Logic;
using UniRx;
using UnityEngine;

namespace Core.UI
{
    public class ActorUI : MonoBehaviour
    {
        [SerializeField] private HealthBar _healthBar;

        public void Initialize(IHealth health, bool skipInitAnim = true)
        {
            health.CurrentHP
                .Skip(skipInitAnim ? 1 : 0)
                .Subscribe(current => _healthBar.SetValue(current, health.MaxHP));

            _healthBar.gameObject.SetActive(false);
        }
    }
}