using System;
using _current.Core.Systems.WeaponSystem.Data;

namespace _current.Data
{
    [Serializable]
    public class WeaponData
    {
        public WeaponTypeId typeId;
        public int ammoCount;
        public int clipCount;

        public WeaponData(WeaponTypeId typeId, int ammoCount, int clipCount)
        {
            this.typeId = typeId;
            this.ammoCount = ammoCount;
            this.clipCount = clipCount;
        }
    }
}