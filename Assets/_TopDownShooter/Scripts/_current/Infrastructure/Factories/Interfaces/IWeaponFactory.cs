using System.Threading.Tasks;
using _current.Core.Systems.DamageSystem;
using _current.Core.Systems.WeaponSystem;
using _current.Data;
using UnityEngine;

namespace _current.Infrastructure.Factories.Interfaces
{
    public interface IWeaponFactory : IFactory
    {
        Task<WeaponPawn> Create(IDamageSender owner, Transform gunHolder, WeaponData weaponData);
    }
}