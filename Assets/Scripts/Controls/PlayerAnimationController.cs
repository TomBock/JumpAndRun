using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controls
{

    public class PlayerAnimationController : MonoBehaviour
    {
        private const float NearZeroFloat = 0.001f;

        [Header("Parameters")]
        [SerializeField] private string _speedParameter;
        [SerializeField] private string _verticalSpeedParameter;
        [SerializeField] private string _isGroundedParameter;

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            _animator.SetFloat(_speedParameter, PlayerData.speedSqr);
            _animator.SetFloat(_verticalSpeedParameter, PlayerData.velocity.y);
            _animator.SetBool(_isGroundedParameter, PlayerData.isGrounded);

            // Rotation to the left
            if (Mathf.Abs(PlayerData.velocity.x) > NearZeroFloat)
            {
                var scale = transform.localScale;
                transform.localScale = new Vector3(PlayerData.velocity.x > 0 ? 1f : -1f, scale.y, scale.z);
                //Debug.Log($"AHH: {transform.localScale} - {PlayerData.direction.x}");
            }
        }
    }

}
