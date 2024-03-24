using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _current.Systems.DamageSystem
{
    
    [CreateAssetMenu(menuName = "Guns/Damage Config", fileName = "DamageConfig", order = 1)]
    public class DamageConfiguration : ScriptableObject
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