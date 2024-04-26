using _current.Core.Pawns.Player;
using _current.UI.Core;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _current.UI.Screens.GameHUD.SubViews
{
    public class CrosshairView : SubView<PlayerWeaponSpawner>
    {
        [SerializeField] private Sprite _reloadSprite;
        [SerializeField] private Image _crosshairImage;
        [SerializeField] private RectTransform _crosshairRect;
        [SerializeField] private RectTransform _nominalCrosshairRect;

        private PlayerWeaponSpawner _weaponSpawner;
        private Sprite _crosshairSprite;
        private PlayerRotation _rotation;
        private Camera _camera;

        protected override void Bind(PlayerWeaponSpawner data)
        {
            _camera = Camera.main;
            _weaponSpawner = data;
            _rotation = data.GetComponent<PlayerRotation>();
            _weaponSpawner.OnReload += AnimateReload;
            _crosshairSprite = _crosshairImage.sprite;
        }

        protected override void Unbind()
        {
            if (_weaponSpawner != null) 
                _weaponSpawner.OnReload -= AnimateReload;
        }

        private void Update()
        {
            _crosshairRect.position = _rotation.LookAxis;
            _nominalCrosshairRect.position = _camera.WorldToScreenPoint(_rotation.TargetTransform.position);
        }

        private void AnimateReload(float time)
        {
            _crosshairImage.fillMethod = Image.FillMethod.Radial360;
            _crosshairImage.sprite = _reloadSprite;
            _crosshairImage.fillAmount = 0;
            _crosshairImage.DOFillAmount( 1, time).SetEase(Ease.Linear).OnComplete(() =>
            {
                _crosshairImage.sprite = _crosshairSprite;
            });
        }

        private void OnDestroy()
        {
            Unbind();
        }
    }
}