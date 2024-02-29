using Services.Input;
using UnityEngine;
using Zenject;

namespace Core.Hero
{
    public class HeroMove : MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private HeroAnimator _animator;
        [SerializeField] private float _movementSpeed;
        
        private IInputService _inputService;
        private Camera _camera;
        
        [Inject]
        private void Construct(IInputService inputService) => 
            _inputService = inputService;

        private void Start() => 
            _camera = Camera.main;

        private void Update() => 
            Move();

        private void Move()
        {
            var movementVector = Vector3.zero;

            if (_inputService.MoveAxis.sqrMagnitude > 0.001f)
            {
                movementVector = _camera.transform.TransformDirection(_inputService.MoveAxis);
                movementVector.y = 0;
                movementVector.Normalize();
            }

            movementVector += Physics.gravity;

            _characterController.Move(movementVector * (_movementSpeed * Time.deltaTime));
            _animator.PlayMove(_characterController.velocity.magnitude);
        }
    }
}