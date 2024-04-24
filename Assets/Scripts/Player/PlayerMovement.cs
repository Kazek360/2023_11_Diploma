using System.Collections.Generic;
using Cutscenes;
using DialogueSystem.Events;
using InputSystem;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float walkSpeed;
        [SerializeField] private float runSpeed;
        [SerializeField] private List<Sprite> playerSprites;
        
        private SpriteRenderer _spriteRenderer;
        private Rigidbody2D _rigidbody;
        
        private bool _isPlayerSprinting;
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
        }

        private void SetCharacterSprite()
        {
            var north = Input.GetKey(KeyCode.W);
            var south = Input.GetKey(KeyCode.S);
            var east = Input.GetKey(KeyCode.A);
            var west = Input.GetKey(KeyCode.D);

            _spriteRenderer.sprite = (north, south, east, west) switch
            {
                (true, false, false, false) => playerSprites[0],
                (true, false, false, true) => playerSprites[1],
                (false, false, false, true) => playerSprites[2],
                (false, true, false, true) => playerSprites[3],
                (false, true, false, false) => playerSprites[4],
                (false, true, true, false) => playerSprites[5],
                (false, false, true, false) => playerSprites[6],
                (true, false, true, false) => playerSprites[7],
                _ => _spriteRenderer.sprite
            };
        }

        private void MovePlayer(Vector2 movementVector)
        {
            SetCharacterSprite();
            var currentSpeed = _isPlayerSprinting ? runSpeed : walkSpeed;
            _rigidbody.velocity = movementVector.normalized * currentSpeed;
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
