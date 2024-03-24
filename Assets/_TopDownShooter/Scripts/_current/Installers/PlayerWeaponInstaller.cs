using _current.Systems.WeaponSystem;
using UnityEngine;
using Zenject;

namespace _current.Installers
{
    public class PlayerWeaponInstaller : MonoInstaller
    {
        [SerializeField] private Transform _weaponAttachPoint;
        [SerializeField] private Transform _targetPoint;

        public override void InstallBindings()
        {

        }
    }
}