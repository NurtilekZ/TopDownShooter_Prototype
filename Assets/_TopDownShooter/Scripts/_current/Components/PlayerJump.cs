using System;
using _current.Player;
using Shaders;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _current.Components
{
    public class PlayerJump : PawnComponent<PlayerPawn>
    {
        [SerializeField] private CharacterController _controller;
        [SerializeField] private  AudioClip _landingAudioClip;
        
        [Space(10)]
        [SerializeField] private float _preLandingHeight;
        [SerializeField] private  float _jumpHeight = 1.2f;
        [SerializeField] private  float _gravity = -15.0f;
        
        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        [SerializeField] private  float _jumpTimeout = 0.50f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        [SerializeField] private  float _fallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        [SerializeField] private  bool _isGrounded = true;

        [Tooltip("Useful for rough ground")]
        [SerializeField] private  float _groundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        [SerializeField] private  float _groundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        [SerializeField] private LayerMask _groundLayers;
        [Space(10)]
        [SerializeField] [Range(0, 1)] private  float _footstepAudioVolume = 0.5f;
        
        // player
        private bool _isJumping;
        private float _terminalVelocity = 53.0f;
        
        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;
        
        public float VerticalVelocity { get; private set; }

        public override void BindPlayerInput()
        {
            if (_pawn.PlayerControls == null) return;
            _pawn.PlayerControls.Player.Jump.performed += AssignJump;
        }

        public override void UnbindPlayerInput()
        {
            if (_pawn.PlayerControls == null) return;
            _pawn.PlayerControls.Player.Jump.performed -= AssignJump;
        }

        private void AssignJump(InputAction.CallbackContext ctx)
        {
            if (!_isJumping) _isJumping = true;
        }

        protected override void Start()
        {
            base.Start();
            
            // reset our timeouts on start
            _jumpTimeoutDelta = _jumpTimeout;
            _fallTimeoutDelta = _fallTimeout;
        }

        private void Update()
        {
            JumpAndGravity();
            GroundedCheck();
        }
        
        private void JumpAndGravity()
        {
            if (_isGrounded)
            {
                _fallTimeoutDelta = _fallTimeout;

                _pawn.Animator.SetBool(AnimationStatics.FreeFall, false);
                
                if (VerticalVelocity < 0f)
                {
                    VerticalVelocity = -2f;
                }

                if (_isJumping && _jumpTimeoutDelta <= 0f)
                {
                    VerticalVelocity = Mathf.Sqrt(_jumpHeight * -2 * _gravity);

                    _pawn.Animator.SetTrigger(AnimationStatics.Jump);
                }

                if (_jumpTimeoutDelta >= 0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                _isJumping = false;
                _jumpTimeoutDelta = _jumpTimeout;

                if (_fallTimeoutDelta >= 0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    _pawn.Animator.SetBool(AnimationStatics.FreeFall, true);
                }
            }
            if (VerticalVelocity < _terminalVelocity)
            {
                VerticalVelocity += _gravity * Time.deltaTime;
            }
        }

        
        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - _groundedOffset,
                transform.position.z);
            _isGrounded = Physics.CheckSphere(spherePosition, _groundedRadius, _groundLayers,
                QueryTriggerInteraction.Ignore);
            
            _pawn.Animator.SetBool(AnimationStatics.Grounded, _isGrounded);
            
            if (!_isGrounded)
            {
                var ray = new Ray
                {
                    origin = transform.position,
                    direction = -transform.up
                };
                if (Physics.Raycast(ray, out var hit, _groundLayers))
                {
                    if (hit.distance < _preLandingHeight)
                    { 
                        _pawn.Animator.SetBool(AnimationStatics.PreGrounded, true);
                    }
                    else
                    {
                        _pawn.Animator.SetBool(AnimationStatics.PreGrounded, false);
                    }
                }
            }
            else
            {
                _pawn.Animator.SetBool(AnimationStatics.PreGrounded, true);
            }
        }
        
        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(_landingAudioClip, transform.TransformPoint(_controller.center), _footstepAudioVolume);
            }
        }
        
        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (_isGrounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - _groundedOffset, transform.position.z),
                _groundedRadius);
        }
    }
}