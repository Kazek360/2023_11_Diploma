using UnityEngine;

public class PlayerTeleporter : MonoBehaviour
{
    [SerializeField] private GameObject objectToTeleport;
    void Start()
    {
        objectToTeleport.transform.position = gameObject.transform.position;
        Destroy(gameObject);
    }

    
}
