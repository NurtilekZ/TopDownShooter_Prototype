using UnityEngine;

namespace _current.Core.Systems.WeaponSystem.ImpactEffects
{
    public interface ICollisionHandler
    {
        void HandleEffect(Collider collider, Vector3 hitPosition, Vector3 hitNormal, WeaponPawn weapon);
    }
}