using UnityEngine;

namespace _current.Core.Systems.WeaponSystem.Configs
{
    [CreateAssetMenu(menuName = "StaticData/WeaponConfig", fileName = "ShootConfig", order = 0)]
    public class WeaponShootConfig : ScriptableObject
    {
        public LayerMask HitMask;
        public Vector3 Spread = new(0.1f,0.1f,0.1f);
        public float FireRate = 0.25f;
    }
}