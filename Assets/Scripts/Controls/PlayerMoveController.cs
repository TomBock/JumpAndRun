using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controls
{

    public class PlayerMoveController : MonoBehaviour
    {
        [SerializeField] private InputActionProperty _walkAction;
        [SerializeField] private InputActionProperty _dashAction;
        [SerializeField] private InputActionProperty _jumpAction;

        [SerializeField] private float _walkSpeed = 70f;
        [SerializeField] private float _dashSpeed = 300f;
        [SerializeField] private float _jumpHeight = 5;
        [SerializeField] private float _jumpCount = 2;
        [SerializeField] private float _jumpButtonMaxTime = 0.3f;
        
        private float _currentSpeed;
        private Vector3 _direction;
        
        private float _jumpingTime;
        private short _jumpsUsed;
        private bool _jumping;

        private bool _dashing;
        
        private Rigidbody _rigidbody;
        
        private void Awake()
        {
            _rigidbody = GetComponent <Rigidbody>();
            _currentSpeed = _walkSpeed;
        }

        private void OnEnable()
        {
            _jumpAction.action.started += OnJumpStarted;
            _jumpAction.action.performed += OnJumpEnded;
            _dashAction.action.started += OnDash;
        }
        private void OnDisable()
        {
            _jumpAction.action.started -= OnJumpStarted;
            _jumpAction.action.performed -= OnJumpEnded;
            _dashAction.action.started += OnDash;
        }

        private void Update()
        {
            var input = _walkAction.action.ReadValue <Vector2>();
            _direction = new Vector3(input.x, _direction.y, input.y);
        }

        private void FixedUpdate()
        {
            var velocity = _direction * _currentSpeed * Time.deltaTime;
            
            // Jumping
            if (_jumping)
            {
                // Decrease the jump amount over time
                velocity.y = _jumpHeight * (1 - _jumpingTime / _jumpButtonMaxTime);
                _jumpingTime += Time.deltaTime;
                PlayerData.isGrounded = false;
            }
            if (_jumpingTime > _jumpButtonMaxTime)
            {
                _jumping = false;
            }
            
            PlayerData.speedSqr = velocity.sqrMagnitude;
            PlayerData.velocity = velocity;

            if (!_jumping && !_dashing)
            {
                // Set gravity if not jumping
                velocity.y = _rigidbody.velocity.y;
                
                if(Physics.Raycast(transform.position, Vector3.down, out var hit) && hit.distance < 0.5f)
                {
                    PlayerData.isGrounded = true;
                    _jumpsUsed = 0;
                }
            }
            _rigidbody.velocity = velocity;
        }

        private void OnDash(InputAction.CallbackContext obj)
        {
            ResetToNormalSpeed(_walkSpeed);
            _currentSpeed = _dashSpeed;
            _dashing = true;
        }
        
        private async void ResetToNormalSpeed(float originalSpeed)
        {
            await Task.Delay(500);
            _currentSpeed = originalSpeed;
            _dashing = false;
        }

        private void OnJumpStarted(InputAction.CallbackContext obj)
        {
            if (_jumpsUsed >= _jumpCount)
                return;
            
            _jumping = true;
            _jumpingTime = 0;
            _jumpsUsed++;
        }
        
        private void OnJumpEnded(InputAction.CallbackContext obj)
        {
            _jumping = false;
        }
    }

}
