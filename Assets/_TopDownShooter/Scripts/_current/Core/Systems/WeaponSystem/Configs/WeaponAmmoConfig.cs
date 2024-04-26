using UnityEngine;

namespace _current.Core.Systems.WeaponSystem.Configs
{
    [CreateAssetMenu(menuName = "StaticData/WeaponConfig", fileName = "AmmoConfig", order = 1)]
    public class WeaponAmmoConfig : ScriptableObject
    {
        public AnimatorOverrideController AnimatorReloadOverride;
        public float ReloadTime;
        public int MaxAmmo = 120;
        public int ClipSize = 30;
    }
}