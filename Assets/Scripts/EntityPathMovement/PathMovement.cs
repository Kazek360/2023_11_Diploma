using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathMovement : MonoBehaviour
{
    [SerializeField] private PathScript PathScript;

    [SerializeField] private float Speed;
    [SerializeField] private float DistanceTolerance;
    private Transform _parentTransform;
    private Rigidbody2D _rigidbody;
    private bool _isMoving;

    public void OnEnable()
    {
        _parentTransform = transform.parent;
        _rigidbody = GetComponentInParent<Rigidbody2D>();
        _isMoving = false;
        StartMovement();
    }


    private IEnumerator Movement()
    {
        List<Vector3> targets = PathScript.GetTargets();
        Debug.Log("AAA");
        foreach (Vector3 target in targets)
        {
            Debug.Log("BBB");
            
            Debug.Log("BBB");
            //_targetTransform = PathScript.NexTarget();


            while (Vector3.Distance(target, _parentTransform.position) > DistanceTolerance)
            {
                Debug.Log("DDD");
                var position = _parentTransform.position;
                Vector3 moveDir = (target - position).normalized;
                _rigidbody.velocity = moveDir * Speed;
                
                yield return new WaitForSeconds(0.01f);
            }
        }
        _rigidbody.velocity = new Vector2(0, 0);
        _isMoving = false;
    }

    public void StartMovement()
    {
        _isMoving = true;
        StartCoroutine(Movement());
    }
    
}
