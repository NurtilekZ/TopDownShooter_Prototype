using _current.UI;
using _current.Units.Components;
using UnityEngine;
using Zenject;

namespace _current.Installers
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