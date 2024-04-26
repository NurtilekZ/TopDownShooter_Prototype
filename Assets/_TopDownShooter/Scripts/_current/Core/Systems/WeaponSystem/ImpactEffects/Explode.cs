using _current.Core.Systems.DamageSystem;
using UnityEngine;

namespace _current.Core.Systems.WeaponSystem.ImpactEffects
{
    public class Explode : ICollisionHandler
    {
        public float Radius = 1;
        public AnimationCurve DamageFalloff;
        public int BaseDamage = 10;
        public int MaxEnemiesAffected = 10;

        private Collider[] _hitObjects;

        public Explode(float radius, AnimationCurve damageFalloff, int baseDamage, int maxEnemiesAffected)
        {
            Radius = radius;
            DamageFalloff = damageFalloff;
            BaseDamage = baseDamage;
            MaxEnemiesAffected = maxEnemiesAffected;
            _hitObjects = new Collider[maxEnemiesAffected];
        }

        public void HandleEffect(Collider collider, Vector3 hitPosition, Vector3 hitNormal, WeaponPawn gun)
        {
            int hits = Physics.OverlapSphereNonAlloc(hitPosition, Radius, _hitObjects, gun.HitMask);
            for (int i = 0; i < hits; i++)
            {
                if (_hitObjects[i].TryGetComponent(out IHealth damageable))
                {
                    float distance = Vector3.Distance(hitPosition, _hitObjects[i].ClosestPoint(hitPosition));

                    damageable.TakeDamage(
                        Mathf.CeilToInt(BaseDamage * DamageFalloff.Evaluate(distance / Radius)),
                        gun.Owner);
                }
            }
        }
    }
}