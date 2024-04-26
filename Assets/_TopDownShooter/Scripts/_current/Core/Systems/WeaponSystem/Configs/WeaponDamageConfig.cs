using UnityEngine;
using Random = UnityEngine.Random;

namespace _current.Core.Systems.WeaponSystem.Configs
{
    [CreateAssetMenu(menuName = "StaticData/WeaponConfig", fileName = "DamageConfig", order = 2)]
    public class WeaponDamageConfig : ScriptableObject
    {
        public ParticleSystem.MinMaxCurve DamageCurve;

        private void Reset()
        {
            DamageCurve.mode = ParticleSystemCurveMode.Curve;
        }

        public int GetDamage(float distance = 0)
        {
            return Mathf.CeilToInt(DamageCurve.Evaluate(distance, Random.value));
        }
    }
}