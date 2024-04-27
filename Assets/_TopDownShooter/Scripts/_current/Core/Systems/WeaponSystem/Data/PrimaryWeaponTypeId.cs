using System;

namespace _current.Core.Systems.WeaponSystem.Data
{
    [Serializable]
    public enum PrimaryWeaponTypeId
    {
        Null,
        AssaultRifle,
        HandGun,
        ShotGun,
        MiniGun,
        Flamethrower,
    }
    
    [Serializable]
    public enum SecondaryWeaponTypeId
    {
        Null,
        RocketLauncher,
        LaserGun,
        Flamethrower,
    }
}