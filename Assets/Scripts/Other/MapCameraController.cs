using System;
using System.Collections;
using UnityEngine;

public class MapCameraController : MonoBehaviour
{
    [SerializeField] private float dragSpeed;
    [SerializeField] private Vector2 minPosition;
    [SerializeField] private Vector2 maxPosition;
    private Camera _camera;
    private Vector3 _dragOrigin;
    private bool _isDragging;
    private Vector3 _originalPosition;

    public static event Action OnMapDragging;
    
    public static void InvokeOnMapDragging()
    {
        OnMapDragging?.Invoke();
    }
    public static event Action OnMapDraggingStop;
    public static void InvokeOnMapDraggingStop()
    {
        OnMapDraggingStop?.Invoke();
    }
    public static event Action OnMapModeEnter;
    public static void InvokeOnMapModeEnter()
    {
        OnMapModeEnter?.Invoke();
    }

    private void OnEnable()
    {
        OnMapDragging += DragMap;
        OnMapDraggingStop += StopDragging;
        OnMapModeEnter += SetCameraPositionToOriginalPosition;
    }
    private void OnDisable()
    {
        OnMapDragging -= DragMap;
        OnMapDraggingStop -= StopDragging;
        OnMapModeEnter -= SetCameraPositionToOriginalPosition;
    }

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _originalPosition = _camera.transform.position;
    }

    private void SetCameraPositionToOriginalPosition()
    {
        _camera.transform.position = _originalPosition;
    }
    
    private void DragMap()
    {
        _dragOrigin = _camera.ScreenToWorldPoint(Input.mousePosition);
        _isDragging = true;
        StartCoroutine(HandleCameraDrag());
    }
    
    private void StopDragging()
    {
        _isDragging = false;
    }
    
    private IEnumerator HandleCameraDrag()
    {
        while (_isDragging)
        {
            var currentMousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            var difference = _dragOrigin - currentMousePosition;
            if (difference.sqrMagnitude < 0.001f) difference = Vector3.zero;
            var newPosition = _camera.transform.position + difference * dragSpeed;
            newPosition.x = Mathf.Clamp(newPosition.x, minPosition.x, maxPosition.x);
            newPosition.y = Mathf.Clamp(newPosition.y, minPosition.y, maxPosition.y);
            _camera.transform.position = newPosition;
            yield return null;
        }
    }
}
