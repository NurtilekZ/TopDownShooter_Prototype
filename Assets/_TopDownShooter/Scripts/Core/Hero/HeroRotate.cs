using Services.Input;
using UnityEngine;
using Zenject;

namespace Core.Hero
{
    public class HeroRotate : MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private HeroAnimator _animator;
        [SerializeField] private float _movementSpeed;
        
        private IInputService _inputService;
        private Camera _camera;
        
        [Inject]
        private void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            Rotate();
        }

        private void Rotate()
        {
            var aimVector = Vector3.zero;

            if (_inputService.AimAxis.sqrMagnitude > 0.001f)
            {
                aimVector.y = transform.TransformDirection(_inputService.AimAxis).y;
                aimVector.Normalize();

                transform.forward = aimVector;
            }
        }
    }
}