using System;
using UniRx;
using UnityEngine;

namespace _current.Systems.WeaponSystem.Configs
{
    [CreateAssetMenu(menuName = "Guns/Ammo Configs", fileName = "Ammo Configs", order = 3)]
    public class AmmoConfiguration : ScriptableObject
    {
        public AnimatorOverrideController AnimatorReloadOverride;
        public float ReloadTime;
        public int MaxAmmo = 120;
        public int ClipSize = 30;

        public int CurrentAmmo = 120;
        public IntReactiveProperty CurrentClipAmmo = new IntReactiveProperty(30);


        private void OnEnable()
        {
            CurrentClipAmmo.Value = ClipSize;
            CurrentAmmo = MaxAmmo;
        }

        public void Reload()
        {
            int maxReloadAmount = Mathf.Min(ClipSize, CurrentAmmo);
            int availableBulletsInCurrentClip = ClipSize - CurrentClipAmmo.Value;
            int reloadAmount = Mathf.Min(maxReloadAmount, availableBulletsInCurrentClip);
            
            CurrentClipAmmo.Value += reloadAmount;
            CurrentAmmo -= reloadAmount;
        }

        public bool CanReload()
        {
            return CurrentClipAmmo.Value < ClipSize && CurrentAmmo > 0;
        }
    }
}