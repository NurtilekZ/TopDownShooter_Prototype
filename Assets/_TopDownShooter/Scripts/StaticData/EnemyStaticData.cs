using System;
using Core.Enemy;

namespace StaticData
{
    [Serializable]
    public record EnemyStaticData
    {
        public EnemyType EnemyType { get; set;}
        public int Health { get; set; }
        public AttackType AttackType { get; set; }
        public float AttackDamage { get; set; }
        public int Resistance { get; set; }
        public float Cooldown { get; set; }
    }
}