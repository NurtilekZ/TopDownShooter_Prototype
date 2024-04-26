using System.Threading.Tasks;
using _current.Core.Pawns.Player;
using _current.Infrastructure.Factories.Interfaces;
using _current.Services.StaticData;
using Cinemachine;
using UnityEngine;
using Zenject;

namespace _current.Infrastructure.Factories
{
    public class CameraFactory : ICameraFactory
    {
        private readonly DiContainer _container;
        private readonly IStaticDataService _staticDataService;

        public CameraFactory(DiContainer container, IStaticDataService staticDataService)
        {
            _container = container;
            _staticDataService = staticDataService;
        }

        public Task WarmUp()
        {
            throw new System.NotImplementedException();
        }

        public void CleanUp()
        {
            throw new System.NotImplementedException();
        }

        public async Task<GameObject> CreateHeroCamera(Transform heroTransform)
        {
            var heroData = _staticDataService.ForHero();
            var playerCamera = Object.Instantiate(heroData.PlayerVirtualCamera).GetComponent<CinemachineVirtualCamera>();
            var composer = Object.Instantiate(heroData.GroupComposer).GetComponent<CinemachineTargetGroup>();
            if (playerCamera != null && composer != null)
            {
                var targetPawn = heroTransform.GetComponent<PlayerRotation>().TargetTransform;
                composer.AddMember(heroTransform, 1, 0);
                composer.AddMember(targetPawn.transform, 0.01f, 0);
                var composerTransform = composer.transform;
                playerCamera.Follow = composerTransform;
                playerCamera.LookAt = composerTransform;
            }
            
            _container.Inject(playerCamera);

            return playerCamera != null ? playerCamera.gameObject : null;
        }
    }
}