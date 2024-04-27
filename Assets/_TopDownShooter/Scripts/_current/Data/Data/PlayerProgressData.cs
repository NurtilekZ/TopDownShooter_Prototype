using System;

namespace _current.Data.Data
{
    [Serializable]
    public class PlayerProgressData
    {
        public LevelData[] LevelsProgress { get; set; }
        public WeaponData[] AvailableWeapons { get; set; }
    }
}