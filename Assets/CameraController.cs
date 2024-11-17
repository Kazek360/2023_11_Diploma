using System;
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
    
    private float _minZoom = 5.0f; 
    private float _maxZoom = 15.0f; 
    [SerializeField] private float zoomSpeed = 1.0f; 
    private float _defaultZoom = 10.0f; 

    public static event Action OnCameraCentered;
    public static void InvokeOnCameraCentered()
    {
        OnCameraCentered?.Invoke();
    }

    public static event Action OnCameraDragged;
    public static void InvokeOnCameraDragged()
    {
        OnCameraDragged?.Invoke();
    }
    public static event Action OnCameraDragStop;
    public static void InvokeOnCameraDragStop()
    {
        OnCameraDragStop?.Invoke();
    }
    public static event Action OnCameraFollow;
    public static void InvokeOnCameraFollow()
    {
        OnCameraFollow?.Invoke();
    }

    private void OnEnable()
    {
        OnCameraCentered += MoveCameraToPlayer;
        OnCameraDragged += DragCamera;
        OnCameraDragStop += StopCameraDrag;
        OnCameraFollow += EnableFollowingPlayer;
    }
    private void OnDisable()
    {
        OnCameraCentered -= MoveCameraToPlayer;
        OnCameraDragged += DragCamera;
        OnCameraDragStop -= StopCameraDrag;
        OnCameraFollow -= EnableFollowingPlayer;
    }
    
    void Start()
    {
        _camera = Camera.main; 
        cameraTransform.parent = player;
        _camera.orthographicSize = _defaultZoom; 
        _targetZoom = _camera.orthographicSize; // Początkowa wartość zoomu
    }

    void Update()
    {
        if (_isDragging)
        {
            HandleCameraDrag();
        }
        HandleZoom();
        
        if (_isFollowingPlayer)
        {
            FollowPlayer();
        }

        FixCameraZPosition();
    }

    private void HandleCameraDrag()
    {
        Vector3 currentMousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 difference = _dragOrigin - currentMousePosition;

        cameraTransform.position += difference * cameraMoveSpeed;

        ConstrainCameraToPlayer();
    }

    private void EnableFollowingPlayer()
    {
        _isFollowingPlayer = true;
        cameraTransform.parent = player; 
    }
    
    private void FollowPlayer()
    {
        cameraTransform.position = new Vector3(player.position.x, player.position.y, FixedZPosition);
    }

    private void MoveCameraToPlayer()
    {
        _isFollowingPlayer = true; 
        cameraTransform.parent = player; 
        _camera.orthographicSize = _defaultZoom; 
        _targetZoom = _defaultZoom;
    }

    private void DragCamera()
    {
        _dragOrigin = _camera.ScreenToWorldPoint(Input.mousePosition);
        _isDragging = true;
        _isFollowingPlayer = false; 
        cameraTransform.parent = null; 
    }
    
    private void StopCameraDrag()
    {
        _isDragging = false;
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
    
    private float _targetZoom; 
    private float _zoomVelocity = 0.0f; 

    private void HandleZoom()
    {
        float scrollData = Input.GetAxis("Mouse ScrollWheel");
        if (scrollData != 0f)
        {
            _targetZoom -= scrollData * zoomSpeed;
            _targetZoom = Mathf.Clamp(_targetZoom, _minZoom, _maxZoom); 
        }

        _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, _targetZoom, ref _zoomVelocity, 0.2f);
    }

}
