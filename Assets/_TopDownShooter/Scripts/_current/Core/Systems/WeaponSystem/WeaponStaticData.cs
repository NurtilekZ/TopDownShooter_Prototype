using System;
using _current.Core.Systems.ImpactSystem;
using _current.Core.Systems.WeaponSystem.Configs;
using _current.Core.Systems.WeaponSystem.Data;
using _current.Core.Systems.WeaponSystem.ImpactEffects;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _current.Core.Systems.WeaponSystem
{
    [CreateAssetMenu(menuName = "StaticData/Weapon", fileName = "WeaponData", order = 0)]
    public class WeaponStaticData : ScriptableObject
    {
        public ImpactType ImpactType;
        public WeaponTypeId WeaponTypeId;
        public string Name;
        public AssetReferenceGameObject PrefabAsset;
        public Vector3 SpawnPoint;
        public Vector3 SpawnRotation;

        public WeaponDamageConfig DamageConfig;
        public WeaponAmmoConfig AmmoConfig;
        public WeaponShootConfig ShootConfig;
        public WeaponTrailConfig TrailConfig;
        public WeaponAudioConfig AudioConfig;

        public ICollisionHandler[] bulletImpactEffects = Array.Empty<ICollisionHandler>();
    }
}