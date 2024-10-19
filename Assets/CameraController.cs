using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player; 
    [SerializeField] private float cameraMoveSpeed = 0.1f; 
    [SerializeField] private float maxDistanceFromPlayer = 5.0f; 
    [SerializeField] private Transform cameraTransform; 
    
    private Camera _camera; 
    private Vector3 _dragOrigin; 
    private bool _isDragging = false;
    private bool _isFollowingPlayer = true;
    private const float FixedZPosition = -10f;

    void Start()
    {
        _camera = Camera.main; 
        cameraTransform.parent = player; 
    }

    void Update()
    {
        HandleMouseInput();

      
        if (_isFollowingPlayer)
        {
            FollowPlayer();
        }

       
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MoveCameraToPlayer();
        }

        FixCameraZPosition();
    }

    private void HandleMouseInput()
    {
     
        if (Input.GetMouseButtonDown(1))
        {
            _dragOrigin = _camera.ScreenToWorldPoint(Input.mousePosition);
            _isDragging = true;
            _isFollowingPlayer = false; 
            cameraTransform.parent = null; 
        }

        if (Input.GetMouseButtonUp(1))
        {
            _isDragging = false;
        }

        if (_isDragging)
        {
            HandleCameraDrag();
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            _isFollowingPlayer = true;
            cameraTransform.parent = player; 
        }
    }

    private void HandleCameraDrag()
    {
        Vector3 currentMousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 difference = _dragOrigin - currentMousePosition;

        cameraTransform.position += difference * cameraMoveSpeed;

        ConstrainCameraToPlayer();
    }

    private void FollowPlayer()
    {
        cameraTransform.position = new Vector3(player.position.x, player.position.y, FixedZPosition);
    }

    private void MoveCameraToPlayer()
    {
        cameraTransform.position = new Vector3(player.position.x, player.position.y, FixedZPosition);
        _isFollowingPlayer = true; 
        cameraTransform.parent = player; 
    }

    private void FixCameraZPosition()
    {
        cameraTransform.position = new Vector3(cameraTransform.position.x, cameraTransform.position.y, FixedZPosition);
    }

    private void ConstrainCameraToPlayer()
    {
        Vector3 distanceFromPlayer = cameraTransform.position - player.position;

        if (distanceFromPlayer.magnitude > maxDistanceFromPlayer)
        {
            Vector3 directionToPlayer = distanceFromPlayer.normalized;
            cameraTransform.position = player.position + directionToPlayer * maxDistanceFromPlayer;
        }
    }
}
