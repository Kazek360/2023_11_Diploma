using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float existTime;
    
    private Rigidbody2D _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
        StartCoroutine(DestroyCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        _rb.velocity = speed * Time.deltaTime * transform.up;
    }
    
    private IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(existTime);
        Destroy(gameObject);
    }
}
