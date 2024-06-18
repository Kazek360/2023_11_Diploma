using UnityEngine;


/**
 * Simple point used for object scaling in ScalingArea
 */
public class ScalingPoint : MonoBehaviour
{
    /**
     * Scaling desired at this point, from which scale is extrapolated in other points on map
     */
    [SerializeField] private float DesiredScaling;
    
    

    public float DesiredScalingVal => DesiredScaling;
}
