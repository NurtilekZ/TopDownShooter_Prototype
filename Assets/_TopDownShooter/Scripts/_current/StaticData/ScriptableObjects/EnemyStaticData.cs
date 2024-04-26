using _current.Core.Pawns.Enemy;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _current.StaticData.ScriptableObjects
{
    [CreateAssetMenu(menuName = "StaticData/Enemy", fileName = "EnemyData")]
    public class EnemyStaticData : ScriptableObject
    {
        public EnemyTypeId EnemyTypeId;
        [Header("Health")]
        [Range(1, 100)] public int Hp;
        [Space]
        [Header("Attack")]
        public int MaxLoot;
        public int MinLoot;
        [Space]
        [Header("Attack")]
        [Range(1f, 30f)] public float Damage;
        [Range(0.5f, 1f)] public float EffectiveDistance = 0.5f;
        [Range(0.5f, 100f)] public float AggroRadius = 20;
        [Range(0.5f, 100f)] public float AttackRadius = 5;
        [Range(0.5f, 5f)] public float AttackCooldown = 1;
        [Space]
        [Header("Locomotion")]
        public float MoveSpeed;
        [Space]
        [Header("Prefab")]
        public AssetReferenceGameObject PrefabReference;
    }
}