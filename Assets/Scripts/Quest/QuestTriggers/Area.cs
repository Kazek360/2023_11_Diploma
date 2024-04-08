using UnityEngine;

public class Area : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AreaEvent.EnterArea();
            Destroy(gameObject);
        }
    }
}
