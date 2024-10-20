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

        private Vector2 _targetPosition;
        private bool _isPlayerSprinting;
        private bool _isMouseMoving;

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
            _isMouseMoving = false;
        }
        
        private void Update()
        {
            // Jeśli kliknięto myszką, ustaw cel ruchu
            if (Input.GetMouseButtonDown(0))
            {
                HandleMouseMovement();
            }

            // Jeśli gracz zaczyna poruszać się klawiaturą (WSAD), anulujemy ruch myszką
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                _isMouseMoving = false; // Przestajemy podążać do klikniętego celu
                Vector2 movementVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                MovePlayer(movementVector); // Sterowanie klawiaturą
            }

            FixPlayerZPosition();
        }

        private void MovePlayer(Vector2 movementVector)
        {
            bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftShift);
            var currentSpeed = _isPlayerSprinting ? runSpeed : walkSpeed;
            if (_isMouseMoving)
            {
                Vector2 direction = (_targetPosition - (Vector2)transform.position).normalized;
                _rigidbody.velocity = direction * currentSpeed;

                _animator.SetFloat(X, direction.x);
                _animator.SetFloat(Y, direction.y);
                _animator.SetBool(IsWalking, true);
                _animator.SetBool(IsRunning, isRunning);

                if (Vector2.Distance(transform.position, _targetPosition) < 0.1f)
                {
                    _isMouseMoving = false;
                    StopPlayerMovement();
                }
            }
            else
            {
                _rigidbody.velocity = movementVector * currentSpeed;
                if (movementVector != Vector2.zero)
                {
                    _animator.SetFloat(X, movementVector.x);
                    _animator.SetFloat(Y, movementVector.y);
                    _animator.SetBool(IsWalking, true);
                    _animator.SetBool(IsRunning, isRunning);
                }
                else
                {
                    StopPlayerMovement();
                }
            }

        }
        
        private void HandleMouseMovement()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _targetPosition = new Vector2(mousePosition.x, mousePosition.y);
                _isMouseMoving = true;
            }
        }

        

        private void ChangeSprintState(bool isPlayerSprinting)
        {
            _isPlayerSprinting = isPlayerSprinting;
        }
        
        private void StopPlayerMovement()
        {
            _rigidbody.velocity = Vector2.zero;
            _animator.SetBool(IsWalking, false);
            _animator.SetBool(IsRunning, false);
        }

        private void StopMovement(DialogueSO dialogue)
        {
            StopPlayerMovement();
        }

        private void StopMovement(CutsceneSO cutscene)
        {
            StopPlayerMovement();
        }
        
        private void FixPlayerZPosition()
        {
            Vector3 position = transform.position;
            position.z = 0;
            transform.position = position;
        }
    }
}