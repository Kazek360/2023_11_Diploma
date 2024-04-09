using UnityEngine;

public class Area : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("NPC") )
        {
            AreaEvent.EnterArea(other.name);
            Destroy(gameObject);
        }
    }
}
