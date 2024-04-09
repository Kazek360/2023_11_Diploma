using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScalingAreaScript : MonoBehaviour
{
    [SerializeField] private GameObject FocusPoint;
    [SerializeField] private float ScaleAtFocusPoint;

    private PolygonCollider2D _edgeCollider2D;
    private List<ObjectWithTransform> _scaledObjects;
    

    private Vector3 _focusTransform;
    // Start is called before the first frame update
    void Start()
    {
        _scaledObjects = new List<ObjectWithTransform>();
        _edgeCollider2D = GetComponent<PolygonCollider2D>();
        _focusTransform = FocusPoint.transform.position;
    }
    
    // Update is called once per frame
    void Update()
    {
        foreach (ObjectWithTransform objectWithTransform in _scaledObjects)
        {
            Debug.Log(_edgeCollider2D.ClosestPoint(objectWithTransform.OriginalTransform));
            //objectWithTransform.GameObject = 
        }
    }
    

    public void OnTriggerEnter2D(Collider2D other)
    {
        ObjectWithTransform obj = new ObjectWithTransform(other.GameObject(), other.GameObject().transform.localScale);
        _scaledObjects.Add(obj);
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        foreach (ObjectWithTransform objectWithTransform in _scaledObjects)
        {
            if (objectWithTransform.GameObject.Equals(other.GameObject()))
            {
                _scaledObjects.Remove(objectWithTransform);
            }
        }
    }
}

class ObjectWithTransform
{
    private GameObject _gameObject;
    private Vector3 _originalTransform;

    public ObjectWithTransform(GameObject gameObject, Vector3 originalTransform)
    {
        _gameObject = gameObject;
        _originalTransform = originalTransform;
    }

    public GameObject GameObject
    {
        get => _gameObject;
        set => _gameObject = value;
    }

    public Vector3 OriginalTransform
    {
        get => _originalTransform;
        set => _originalTransform = value;
    }
}