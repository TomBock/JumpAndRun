using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controls
{

    public class PlayerBattleController : MonoBehaviour
    {
        [SerializeField] private InputActionProperty _attack1Action;
        [SerializeField] private InputActionProperty _attack2Action;
        [SerializeField] private InputActionProperty _attack3Action;
        
        [SerializeField] private string _attack1Animation;
        [SerializeField] private string _attack2Animation;
        [SerializeField] private string _attack3Animation;

        [SerializeField] private Collider _attackTargetCollider;

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _attackTargetCollider.enabled = false;
        }
        
        private void OnEnable()
        {
            _attack1Action.action.started += OnAttack1;
            _attack2Action.action.started += OnAttack2;
            _attack3Action.action.started += OnAttack3;
        }

        private void OnDisable()
        {
            _attack1Action.action.started -= OnAttack1;
            _attack2Action.action.started -= OnAttack2;
            _attack3Action.action.started -= OnAttack3;
        }
        
        private void OnAttack1(InputAction.CallbackContext obj) => PlayAttack(_attack1Animation);
        private void OnAttack2(InputAction.CallbackContext obj) => PlayAttack(_attack2Animation);
        private void OnAttack3(InputAction.CallbackContext obj) => PlayAttack(_attack3Animation);

        private void PlayAttack(string animationName)
        {
            _animator.Play(animationName);
            _attackTargetCollider.enabled = true;
            ResetAttackTargetCollider();
        }
        
        private async void ResetAttackTargetCollider()
        {
            await Task.Delay(500);
            _attackTargetCollider.enabled = false;
        }
    }

}
