using System;

namespace StaticData
{
    [Serializable]
    public record HeroStaticData
    {
        public float Health { get; set; }
        public float AttackDamage { get; set; }
        public int Resistance { get; set; }
    }
}