using System;
using _current.Systems.WeaponSystem;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _current.UI
{
    public class CrosshairUI : MonoBehaviour
    {
        [SerializeField] private Image _crosshairImage;
        [SerializeField] private Sprite _crosshairSprite;
        [SerializeField] private Sprite _reloadSprite;
        [SerializeField] private PlayerGunSelector _gunSelector;
        
        private void Start()
        {
            _gunSelector.OnReload += OnReload;
        }

        private void OnReload(float time)
        {
            _crosshairImage.fillMethod = Image.FillMethod.Radial360;
            _crosshairImage.sprite = _reloadSprite;
            _crosshairImage.fillAmount = 0;
            _crosshairImage.DOFillAmount( 1, time).SetEase(Ease.Linear).OnComplete(() =>
            {
                _crosshairImage.sprite = _crosshairSprite;
            });
        }
    }
}