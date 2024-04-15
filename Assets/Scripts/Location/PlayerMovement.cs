using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    private Rigidbody2D _rigidbody;
    private bool _isInterfaceOpened;
    private float _currentSpeed;
    
    public Sprite northSprite;
    public Sprite southSprite;
    public Sprite westSprite;
    public Sprite eastSprite;
    public Sprite northWestSprite;
    public Sprite northEastSprite;
    public Sprite southWestSprite;
    public Sprite southEastSprite;
    
    private Direction _currentDirection = Direction.South;
    public enum Direction
    {
        North,
        South,
        West,
        East,
        NorthWest,
        NorthEast,
        SouthWest,
        SouthEast
    }
    private void OnEnable()
    {
        InterfaceEvents.OnInterfaceOpened += DisableMovement;
        InterfaceEvents.OnInterfaceClosed += EnableMovement;
    }
    private void OnDisable()
    {
        InterfaceEvents.OnInterfaceOpened -= DisableMovement;
        InterfaceEvents.OnInterfaceClosed -= EnableMovement;
    }
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _isInterfaceOpened = false;
        _currentSpeed = walkSpeed;
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _currentSpeed = runSpeed;
        }
        else
        {
            _currentSpeed = walkSpeed;
        }
        // Sprawdź, które klawisze są obecnie naciśnięte
        var up = Input.GetKey(KeyCode.W);
        var down = Input.GetKey(KeyCode.S);
        var left = Input.GetKey(KeyCode.A);
        var right = Input.GetKey(KeyCode.D);
        
        // Ustaw kierunek postaci na podstawie naciśniętych klawiszy
        _currentDirection = (up, down, left, right) switch
        {
            (true, false, false, false) => Direction.North,
            (false, true, false, false) => Direction.South,
            (false, false, true, false) => Direction.West,
            (false, false, false, true) => Direction.East,
            (true, false, true, false) => Direction.NorthWest,
            (true, false, false, true) => Direction.NorthEast,
            (false, true, true, false) => Direction.SouthWest,
            (false, true, false, true) => Direction.SouthEast,
            _ => _currentDirection
        };

        // Ustaw odpowiednią grafikę postaci w zależności od kierunku
        SetCharacterSprite();
        
        if (!_isInterfaceOpened)
        {
            MovePlayer();
        }
        else
        {
            _rigidbody.velocity = Vector2.zero;
        }
    }

    private void DisableMovement(string interfaceName)
    {
        _isInterfaceOpened = true;
    }
    private void EnableMovement()
    {
        _isInterfaceOpened = false;
    }
    
    private void SetCharacterSprite()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        // Wybierz odpowiednią grafikę na podstawie kierunku
        switch (_currentDirection)
        {
            case Direction.North:
                        spriteRenderer.sprite = northSprite;
                        break;
                    case Direction.South:
                        spriteRenderer.sprite = southSprite;
                        break;
                    case Direction.West:
                        spriteRenderer.sprite = westSprite;
                        break;
                    case Direction.East:
                        spriteRenderer.sprite = eastSprite;
                        break;
                    case Direction.NorthWest:
                        spriteRenderer.sprite = northWestSprite;
                        break;
                    case Direction.NorthEast:
                        spriteRenderer.sprite = northEastSprite;
                        break;
                    case Direction.SouthWest:
                        spriteRenderer.sprite = southWestSprite;
                        break;
                    case Direction.SouthEast:
                        spriteRenderer.sprite = southEastSprite;
                        break;
        }
    }
    
    void MovePlayer()
    {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {
            _rigidbody.velocity = new Vector2(xInput, yInput).normalized * _currentSpeed;
        }
        else
        {
            _rigidbody.velocity = Vector2.zero;
        }
    }
}
