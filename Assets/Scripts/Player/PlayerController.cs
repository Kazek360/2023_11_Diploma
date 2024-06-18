using Cutscenes;
using DialogueSystem.Events;
using DialogueSystem.ScriptableObjects;
using InputSystem;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float walkSpeed;

        [SerializeField] private float runSpeed;
        
        private Rigidbody2D _rigidbody;
        private Animator _animator;

        private bool _isPlayerSprinting;
        
        private static readonly int X = Animator.StringToHash("X");
        private static readonly int Y = Animator.StringToHash("Y");
        private static readonly int IsWalking = Animator.StringToHash("isWalking");
        private static readonly int IsRunning = Animator.StringToHash("isRunning");

        private void OnEnable()
        {
            InputEvents.OnSprintInput += ChangeSprintState;
            InputEvents.OnMovementInput += MovePlayer;

            DialogueEvents.OnDialogueStarted += StopMovement;
            CutsceneEvents.OnCutsceneStarted += StopMovement;
        }

        private void OnDisable()
        {
            InputEvents.OnSprintInput -= ChangeSprintState;
            InputEvents.OnMovementInput -= MovePlayer;

            DialogueEvents.OnDialogueStarted -= StopMovement;
            CutsceneEvents.OnCutsceneStarted -= StopMovement;
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _isPlayerSprinting = false;
            _animator = GetComponent<Animator>();
        }

        private void MovePlayer(Vector2 movementVector)
        {
            bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftShift);
            var currentSpeed = _isPlayerSprinting ? runSpeed : walkSpeed;
            _rigidbody.velocity = movementVector.normalized * currentSpeed;

            if (movementVector.x != 0 || movementVector.y != 0)
            {
                _animator.SetFloat(X, movementVector.x);
                _animator.SetFloat(Y, movementVector.y);

                _animator.SetBool(IsWalking, true);
                _animator.SetBool(IsRunning, isRunning);
            }
            else
            {
                _animator.SetBool(IsWalking, false);
                _animator.SetBool(IsRunning, false);
            }
        }

        private void ChangeSprintState(bool isPlayerSprinting)
        {
            _isPlayerSprinting = isPlayerSprinting;
        }

        private void StopMovement(DialogueSO dialogue)
        {
            _rigidbody.velocity = Vector2.zero;
        }

        private void StopMovement(CutsceneSO cutscene)
        {
            _rigidbody.velocity = Vector2.zero;
        }
    }
}