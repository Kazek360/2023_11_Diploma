using System.Collections.Generic;
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

        private bool _isInterfaceOpened;
        private bool _isPlayerSprinting;
        private void OnEnable()
        {
            InputEvents.OnSprintInput += ChangeSprintState;
            InputEvents.OnMovementInput += MovePlayer;
            InterfaceEvents.OnInterfaceOpened += DisableMovement;
            InterfaceEvents.OnInterfaceClosed += EnableMovement;
        }

        private void OnDisable()
        {
            InputEvents.OnSprintInput -= ChangeSprintState;
            InputEvents.OnMovementInput -= MovePlayer;
            InterfaceEvents.OnInterfaceOpened -= DisableMovement;
            InterfaceEvents.OnInterfaceClosed -= EnableMovement;
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _isInterfaceOpened = false;
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
            if (_isInterfaceOpened)
            {
                _rigidbody.velocity = Vector2.zero;
                return;
            }

            SetCharacterSprite();
            var currentSpeed = _isPlayerSprinting ? runSpeed : walkSpeed;
            _rigidbody.velocity = movementVector.normalized * currentSpeed;
        }

        private void ChangeSprintState(bool isPlayerSprinting)
        {
            _isPlayerSprinting = isPlayerSprinting;
        }

        private void DisableMovement(string interfaceName)
        {
            _isInterfaceOpened = true;
        }

        private void EnableMovement()
        {
            _isInterfaceOpened = false;
        }
    }
}
