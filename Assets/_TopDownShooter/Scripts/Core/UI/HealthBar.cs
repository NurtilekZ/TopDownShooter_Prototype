using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private float _damageTweenDuration;
        [SerializeField] private bool _alwaysVisible;
        [SerializeField] private Image _barImage;

        private Sequence _sequence;

        private void OnDestroy() => 
            _sequence.Kill();

        public void SetValue(float current, float max)
        {
            var normalizedHp = 1f * current / max;
            
            _sequence.Complete();
            _sequence = DOTween.Sequence()
                .AppendCallback(() => gameObject
                    .SetActive(true))
                .Append(_barImage
                    .DOFillAmount(normalizedHp, _damageTweenDuration))
                .Join(_barImage.rectTransform
                    .DOLocalRotate(SelectRotation(normalizedHp, _barImage), _damageTweenDuration))
                .AppendCallback(() => gameObject
                    .SetActive(_alwaysVisible == true))
                .Play();
        }

        private Vector3 SelectRotation(float normalizedValue, Image image) =>
            image.fillMethod switch
            {
                Image.FillMethod.Horizontal or Image.FillMethod.Vertical => Vector3.zero,
                Image.FillMethod.Radial360 => new Vector3(0, 0, 45 + (1 - normalizedValue) * 180),
                Image.FillMethod.Radial180 => new Vector3(0, 0, (1 - normalizedValue) * 90),
                _ => throw new ArgumentOutOfRangeException()
            };
    }
}