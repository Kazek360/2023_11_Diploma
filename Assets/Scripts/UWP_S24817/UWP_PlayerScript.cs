using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UWP_PlayerScript : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float speed;
    private Rigidbody2D _rigidbody;
    
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        MovePlayer();
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }


    void MovePlayer()
    {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {
            _rigidbody.velocity = speed * Time.deltaTime * new Vector2(xInput, yInput).normalized;
        }
        else
        {
            _rigidbody.velocity = Vector2.zero;
        }
    }

    void Shoot()
    {
        var bullet = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log(mousePos);
        bullet.transform.LookAt(mousePos, Vector2.up);
    }
}
