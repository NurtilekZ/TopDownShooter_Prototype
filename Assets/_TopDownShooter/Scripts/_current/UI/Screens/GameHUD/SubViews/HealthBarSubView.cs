using _current.Core.Systems.DamageSystem;
using _current.UI.Core;
using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace _current.UI.Screens.GameHUD.SubViews
{
    public class HealthBarSubView : SubView<IHealth>
    {
        [SerializeField] private Image _valueSlider;
        [SerializeField] private TMP_Text _valueText;
        private IHealth _data;

        protected override void Bind(IHealth data)
        {
            _data = data;
            data.CurrentHealth.Subscribe(SetValue).AddTo(_disposables);
            SetValue(data.CurrentHealth.Value);
        }

        private void SetValue(float value)
        {
            if (_valueText) 
                _valueText.text = value.ToString();
            if (_valueSlider) 
                _valueSlider.DOFillAmount(value / _data.MaxHealth, 0.25f);
        }


        protected override void Unbind()
        {
            
        }
    }
}