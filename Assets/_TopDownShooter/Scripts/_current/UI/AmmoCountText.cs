using _current.Systems.WeaponSystem;
using TMPro;
using UniRx;
using UnityEngine;

namespace _current.UI
{
    public class AmmoCountText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _ammoText;
        [SerializeField] private PlayerGunSelector _gunSelector;


        private void Start()
        {
            _gunSelector.ActiveGun.ammoConfig.CurrentClipAmmo.Subscribe(x =>
                _ammoText.text = $"{x} / {_gunSelector.ActiveGun.ammoConfig.CurrentAmmo}");
        }
    }
}