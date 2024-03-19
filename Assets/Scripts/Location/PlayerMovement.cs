using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D _rigidbody;
    private bool _inDialogue;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _inDialogue = false;
    }
    private void OnEnable()
    {
        DialogueEvents.OnDialogueStarted += DisableMovement;
        DialogueEvents.OnDialogueEnd += EnableMovement;
    }
    
    private void OnDisable()
    {
        DialogueEvents.OnDialogueStarted -= DisableMovement;
        DialogueEvents.OnDialogueEnd -= EnableMovement;
    }
    private void Update()
    {
        if (!_inDialogue)
        {
            MovePlayer();
        }
        else
        {
            _rigidbody.velocity = Vector2.zero;
        }
    }

    private void DisableMovement(Dialogue dialogue)
    {
        _inDialogue = true;
    }
    private void EnableMovement()
    {
        _inDialogue = false;
    }

    void MovePlayer()
    {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {
            _rigidbody.velocity = new Vector2(xInput, yInput).normalized * speed;
        }
        else
        {
            _rigidbody.velocity = Vector2.zero;
        }
    }
}
