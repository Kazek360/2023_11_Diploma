using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    private GameObject _target;
    [SerializeField] private float speed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_target is null)
        {
            return;
        }
        Vector3 moveDir = (_target.transform.position - transform.position).normalized;
        transform.position +=  speed * Time.deltaTime * moveDir;
        if (Vector3.Distance(_target.transform.position, transform.position) < speed * Time.deltaTime * 2)
        {
            Destroy(gameObject);
        }
    }

    public void SetTarget(GameObject target)
    {
        _target = target;
    }
}
