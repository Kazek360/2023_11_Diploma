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

    public void OnEnable()
    {
        PathEvents.OnMovementStarted += StartMovement;
        _parentTransform = transform.parent;
        _rigidbody = GetComponentInParent<Rigidbody2D>();
    }
    public void OnDisable()
    {
        PathEvents.OnMovementStarted -= StartMovement;
    }

    private IEnumerator Movement()
    {
        List<Vector3> targets = PathScript.GetTargets();
        foreach (Vector3 target in targets)
        {
            while (Vector3.Distance(target, _parentTransform.position) > DistanceTolerance)
            {
                var position = _parentTransform.position;
                Vector3 moveDir = (target - position).normalized;
                _rigidbody.velocity = moveDir * Speed;
                
                yield return new WaitForSeconds(0.01f);
            }
        }
        _rigidbody.velocity = new Vector2(0, 0);
        PathEvents.EndMovement();
    }

    public void StartMovement()
    {
        StartCoroutine(Movement());
    }
    
}
