using System.Collections.Generic;
using Cutscenes;
using DialogueSystem.Events;
using DialogueSystem.ScriptableObjects;
using InputSystem;
using SaveSystem;
using SaveSystem.Data;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour, IDataPersistence
    {
        [SerializeField] private float walkSpeed;

        [SerializeField] private float runSpeed;
        
        private SpriteRenderer _spriteRenderer;
        private Rigidbody2D _rigidbody;
        private Animator _animator;

        private bool _isPlayerSprinting;
        
        private static readonly int X = Animator.StringToHash("X");
        private static readonly int Y = Animator.StringToHash("Y");
        private static readonly int IsWalking = Animator.StringToHash("isWalking");

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
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _isPlayerSprinting = false;
            _animator = GetComponent<Animator>();
        }

        private void MovePlayer(Vector2 movementVector)
        {
            var currentSpeed = _isPlayerSprinting ? runSpeed : walkSpeed;
            _rigidbody.velocity = movementVector.normalized * currentSpeed;

            if (movementVector.x != 0 || movementVector.y != 0)
            {
                _animator.SetFloat(X, movementVector.x);
                _animator.SetFloat(Y, movementVector.y);

                _animator.SetBool(IsWalking, true);
            }
            else
            {
                _animator.SetBool(IsWalking, false);
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

        public void LoadData(GameData gameData)
        {
            transform.position = new Vector3(gameData.playerPosition[0], gameData.playerPosition[1], 0);
        }

        public void SaveData(ref GameData gameData)
        {
            gameData.playerPosition = new[] { transform.position.x, transform.position.y };
        }
    }
}