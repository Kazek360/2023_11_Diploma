using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/**
 * A script for scaling area allowing game to modify objects scale based on
 * distance from predefined points using a method based on Phong shading
 */
public class ScalingAreaScript : MonoBehaviour
{
    private List<ScalingPoint> _scalingPoints;
    private List<ObjectWithTransform> _scaledObjects;
    
    // Start is called before the first frame update
    void Start()
    {
        _scaledObjects = new List<ObjectWithTransform>();
        _scalingPoints = new List<ScalingPoint>();
        foreach (Transform child in transform)
        {
            ScalingPoint tmp = child.GetComponent<ScalingPoint>();
            if (tmp is not null)
            {
                _scalingPoints.Add(tmp);
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        foreach (ObjectWithTransform objectWithTransform in _scaledObjects)
        {
            RescaleObject(objectWithTransform);
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
                objectWithTransform.GameObject.transform.localScale = objectWithTransform.OriginalScale;
                _scaledObjects.Remove(objectWithTransform);
                return;
            }
        }
    }

    /**
     * Rescale object based on distance from objects with modified Phong shading algorithm, which uses
     * ScalingPoints in place of shape corners and their predefined scale modifiers instead
     * of light value
     *
     * <param name="objectWithTransform">Object to be re-scaled</param>
     */
    private void RescaleObject(ObjectWithTransform objectWithTransform)
    {
        Vector2 objectPos = objectWithTransform.GameObject.transform.position;
        float scaleModifier = 1;
        
        float sumOfReversedDistances = 0;
        
        foreach (ScalingPoint scalingPoint in _scalingPoints)
        {
            Vector2 scalingPointPos = scalingPoint.gameObject.transform.position;
            sumOfReversedDistances += 1 / Vector2.Distance(scalingPointPos, objectPos);
        }

        //Debug.Log("START");
        foreach (ScalingPoint scalingPoint in _scalingPoints)
        {
            Vector2 scalingPointPos = scalingPoint.gameObject.transform.position;
            float scaledDistance = (1 / Vector2.Distance(scalingPointPos, objectPos)) / sumOfReversedDistances;
            scaleModifier += ((scalingPoint.DesiredScalingVal - scaleModifier) * scaledDistance);
        }
        objectWithTransform.GameObject.transform.localScale = objectWithTransform.OriginalScale * scaleModifier;
        
    }

}

class ObjectWithTransform
{
    public ObjectWithTransform(GameObject gameObject, Vector3 originalScale)
    {
        GameObject = gameObject;
        OriginalScale = originalScale;
    }

    public GameObject GameObject { get; }

    public Vector3 OriginalScale { get; }
}