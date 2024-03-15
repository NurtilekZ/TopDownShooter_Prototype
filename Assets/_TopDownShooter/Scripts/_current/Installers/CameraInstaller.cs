using _old.Components;
using UnityEngine;
using Zenject;

namespace _old.Installers
{
    public class CameraInstaller : MonoInstaller
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private CrosshairUI _crosshairImage;

        public override void InstallBindings()
        {
            Container.BindInstance(_camera).AsSingle();
            Container.BindInstance(_crosshairImage).AsSingle();
        }
    }
}