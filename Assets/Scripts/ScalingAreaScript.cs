using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScalingAreaScript : MonoBehaviour
{
    [SerializeField] private GameObject FocusPoint;
    [SerializeField] private float ScaleAtFocusPoint;

    private EdgeCollider2D _collider2D;
    private List<ObjectWithTransform> _scaledObjects;
    

    private Vector2 _focusPosition;
    // Start is called before the first frame update
    void Start()
    {
        _scaledObjects = new List<ObjectWithTransform>();
        _collider2D = GetComponent<EdgeCollider2D>();
        _focusPosition = FocusPoint.transform.position;
    }
    
    // Update is called once per frame
    void Update()
    {
        foreach (ObjectWithTransform objectWithTransform in _scaledObjects)
        {
            Vector2 objectPos = objectWithTransform.GameObject.transform.position;
            
            Vector2 direction = (objectPos - _focusPosition).normalized;
            LayerMask layerMask = LayerMask.GetMask("TechnicalRaycastLayer");
            RaycastHit2D hit = Physics2D.Raycast(objectPos, direction, 1000f, layerMask);
            Debug.DrawRay(objectPos, direction * 1000, Color.green);
            if(hit.collider is not null)
            {
                Debug.DrawRay(objectPos, direction * 1000, Color.red);
                Debug.Log("HIT: " + hit.collider.gameObject.name);
                if(hit.collider.isTrigger)
                {
                    Debug.Log(hit.point);
                }
            }
            //objectWithTransform.GameObject = 
        }
    }
    

    public void OnTriggerExit2D(Collider2D other)
    {
        foreach (ObjectWithTransform objectWithTransform in _scaledObjects)
        { 
            if (objectWithTransform.GameObject.Equals(other.GameObject())) 
            { 
                _scaledObjects.Remove(objectWithTransform);
                return;
            }
        }
        ObjectWithTransform obj = new ObjectWithTransform(other.GameObject(), other.GameObject().transform.localScale);
        _scaledObjects.Add(obj);
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