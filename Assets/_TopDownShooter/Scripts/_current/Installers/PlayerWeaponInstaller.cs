using _old.Weapon;
using UnityEngine;
using Zenject;

namespace _old.Installers
{
    public class PlayerWeaponInstaller : MonoInstaller
    {
        [SerializeField] private Transform _weaponAttachPoint;
        [SerializeField] private Transform _targetPoint;
        [SerializeField] private WeaponPawn _weaponPawn;

        public override void InstallBindings()
        {
            var weaponView = Container.InstantiatePrefabForComponent<WeaponPawn>(
                _weaponPawn,
                _weaponAttachPoint.position,
                _weaponAttachPoint.rotation,
                _weaponAttachPoint);

            Container.BindInstance(weaponView).AsSingle();
        }
    }
}