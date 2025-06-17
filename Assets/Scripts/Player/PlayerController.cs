using System.Collections;
using StorySystems.Cutscenes;
using InputSystem;
using StorySystems.Dialogues;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float walkSpeed;
        [SerializeField] private float runSpeed;

        private Rigidbody2D _rigidbody;
        private Animator _animator;

        private bool _canMove;
        private Vector2 _targetPosition;
        private bool _isPlayerSprinting;
        private bool _isMouseMoving;

        private Coroutine _moveRoutine;
        
        private static readonly int X = Animator.StringToHash("X");
        private static readonly int Y = Animator.StringToHash("Y");
        private static readonly int IsWalking = Animator.StringToHash("isWalking");
        private static readonly int IsRunning = Animator.StringToHash("isRunning");
        
        private void OnEnable()
        {
            InputEvents.OnSprintInput += ChangeSprintState;
            InputEvents.OnMovementInput += MovePlayer;
            InputEvents.OnMovementStop += StopPlayerMovement;
            InputEvents.OnMouseMovement += OnMouseClick;
            
            DialogueEvent.OnDialogueStarted += StopMovement;
            CutsceneEvents.OnCutsceneStarted += StopMovement;
            DialogueEvent.OnDialogueFinished += EnableMovement;
            CutsceneEvents.OnCutsceneFinished += EnableMovement;
        }

        private void OnDisable()
        {
            InputEvents.OnSprintInput -= ChangeSprintState;
            InputEvents.OnMovementInput -= MovePlayer;
            InputEvents.OnMovementStop -= StopPlayerMovement;
            InputEvents.OnMouseMovement -= OnMouseClick;
            
            DialogueEvent.OnDialogueStarted -= StopMovement;
            CutsceneEvents.OnCutsceneStarted -= StopMovement;
            DialogueEvent.OnDialogueFinished -= EnableMovement;
            CutsceneEvents.OnCutsceneFinished -= EnableMovement;
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _isPlayerSprinting = false;
            _animator = GetComponent<Animator>();
            _isMouseMoving = false;
            
            _canMove = true;
        }

        private void MovePlayer(Vector2 movementVector)
        {
            if (_canMove)
            {
                if (_moveRoutine != null) StopCoroutine(_moveRoutine);
                var currentSpeed = _isPlayerSprinting ? runSpeed : walkSpeed;
                _isMouseMoving = false;
                _rigidbody.linearVelocity = movementVector * currentSpeed;
                SetAnimationParameters(movementVector);
            }
        }
        
        private void OnMouseClick()
        {
            var clickPosition = Camera.main.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
            
            if (_moveRoutine != null) StopCoroutine(_moveRoutine);
            _moveRoutine = StartCoroutine(MoveToTarget(clickPosition));
            
            _isMouseMoving = true;
        }
        
        private IEnumerator MoveToTarget(Vector2 target)
        {
            while ((_rigidbody.position - target).sqrMagnitude > 0.01f)
            {
                var currentSpeed = _isPlayerSprinting ? runSpeed : walkSpeed;
                
                var dir = (target - _rigidbody.position).normalized;
                _rigidbody.linearVelocity = dir * currentSpeed;
                SetAnimationParameters(dir);
                yield return null;
            }

            _rigidbody.linearVelocity = Vector2.zero;
            SetAnimationParameters(Vector2.zero);
        }
        
        
        private void ChangeSprintState(bool isPlayerSprinting, Vector2 movementVector)
        {
            _isPlayerSprinting = isPlayerSprinting;
            if (!_isMouseMoving)
            {
                MovePlayer(movementVector);
            }
        }
        
        private void StopPlayerMovement()
        {
            _rigidbody.linearVelocity = Vector2.zero;
            _animator.SetBool("isWalking", false);
            _animator.SetBool("isRunning", false);
            
            // zatrzymaj animacje przed czasem
            _animator.Play("Idle");
        }

        private void EnableMovement(DialogueInfo dialogue)
        {
            _canMove = true;
        }
        
        private void EnableMovement(Cutscene cutscene)
        {
            _canMove = true;
        }
        
        private void StopMovement(DialogueInfo dialogue)
        {
            _canMove = false;
            StopPlayerMovement();
        }

        private void StopMovement(Cutscene cutscene)
        {
            _canMove = false;
            StopPlayerMovement();
        }
        
        private void SetAnimationParameters(Vector2 direction)
        {
            if (direction == Vector2.zero)
            {
                _animator.SetBool("isWalking", false);
                _animator.SetBool("isRunning", false);
            }
            else
            {
                _animator.SetBool("isWalking", true);
                _animator.SetBool("isRunning", _isPlayerSprinting);

                if (direction.x != 0)
                {
                    _animator.SetFloat("X", Mathf.Sign(direction.x)); 
                    _animator.SetFloat("Y", 0); 
                }
                else if (direction.y != 0)
                {
                    _animator.SetFloat("X", 0);
                    _animator.SetFloat("Y", Mathf.Sign(direction.y)); 
                }
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            _rigidbody.linearVelocity = Vector2.zero;
        }
        
    }
}