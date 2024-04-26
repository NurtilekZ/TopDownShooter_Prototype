using _current.Core.Systems.ImpactSystem;
using _current.Core.Systems.WeaponSystem.ImpactEffects;
using UnityEngine;

namespace _current.Core.Systems.WeaponSystem.Modifier
{
    public class GunModifierApplier : MonoBehaviour
    {
        [SerializeField] private ImpactType _impactTypeOverride;
        [SerializeField] private WeaponStaticData _weapon;

        private void Start()
        {
            var impactTypeModifier = new ImpactTypeModifier()
            {
                Amount = _impactTypeOverride
            };
            impactTypeModifier.Apply(_weapon);

            _weapon.bulletImpactEffects = new ICollisionHandler[]
            {
                new Explode(
                    1.5f,
                    new AnimationCurve(new Keyframe[] {new Keyframe(0, 1), new Keyframe(1, 0.25f)}), 
                    10, 
                    10)
            };
            
            DamageModifier damageModifier = new DamageModifier()
            {
                Amount = 1.5f,
                AttributeName = "DamageConfig/DamageCurve"
            };
            damageModifier.Apply(_weapon);
        }
    }
}