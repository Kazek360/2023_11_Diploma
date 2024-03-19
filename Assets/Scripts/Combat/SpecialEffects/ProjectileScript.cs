using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ProjectileScript : MonoBehaviour
{
    private GameObject _target;
    [FormerlySerializedAs("speed")] [SerializeField] private float Speed;
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
        transform.position +=  Speed * Time.deltaTime * moveDir;
        if (Vector3.Distance(_target.transform.position, transform.position) < Speed * Time.deltaTime * 2)
        {
            Destroy(gameObject);
        }
    }

    public void SetTarget(GameObject target)
    {
        _target = target;
    }
}
