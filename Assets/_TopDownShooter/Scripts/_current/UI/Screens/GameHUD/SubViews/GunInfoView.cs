using _current.Core.Pawns.Player;
using _current.UI.Core;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace _current.UI.Screens.GameHUD.SubViews
{
    public class GunInfoView : SubView<PlayerWeaponSpawner>
    {
        [SerializeField] private Image _ammoSlider;
        [SerializeField] private TMP_Text _clipText;
        [SerializeField] private PlayerWeaponSpawner _gun;

        protected override void Bind(PlayerWeaponSpawner gun)
        {
            _gun = gun;
            if (_gun.activeWeapon == null) 
                _gun.OnGunInit += AssignGun;
            else
                SubscribeToAmmo();
        }

        private void AssignGun(PlayerWeaponSpawner gun)
        {
            _gun = gun;
            SubscribeToAmmo();
        }

        private void SubscribeToAmmo()
        {
            _gun.activeWeapon.CurrentAmmo
                .Subscribe(x => _ammoSlider.fillAmount = x/_gun.activeWeapon.MaxAmmo)
                .AddTo(_disposables);

            _gun.activeWeapon.CurrentClip
                .Subscribe(x => _clipText.text = $"{x}/{_gun.activeWeapon.MaxClip}")
                .AddTo(_disposables);
        }

        protected override void Unbind()
        {
            if (_gun != null) 
                _gun.OnGunInit -= AssignGun;
        }
    }
}