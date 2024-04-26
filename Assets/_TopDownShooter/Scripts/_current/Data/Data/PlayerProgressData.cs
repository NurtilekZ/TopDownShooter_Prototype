using System;
using System.Collections.Generic;

namespace _current.Data.Data
{
    [Serializable]
    public class PlayerProgressData
    {
        public WeaponData[] CurrentWeapons { get; set; }
        public HashSet<string> CompletedStages { get; set; }
    }
}