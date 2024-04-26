using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace _current.UI.Overlays.HealthBar
{
    public class HealthOverlayView : BaseOverlayView<HealthOverlayViewModel>
    {
        [SerializeField] private Image _healthSlider;
        
        protected override void Bind()
        { 
            _viewModel.health.CurrentHealth.Subscribe(x => 
                _healthSlider.fillAmount = x / _viewModel.health.MaxHealth).AddTo(_disposables);
        }

        protected override void Unbind()
        {
            throw new System.NotImplementedException();
        }
    }
}