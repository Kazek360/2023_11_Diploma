using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCombat : MonoBehaviour
{
    private enum State
    {
        Idling,
        Moving
    }
    
    [SerializeField] private float speed;
    private Rigidbody2D _rigidbody;
    private State _currentState;
    private Vector3 _currentDestination;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    
    private void Start()
    {
        _currentState = State.Idling;
    }
    private void Update()
    {
        switch (_currentState)
        {
            case State.Idling:
                break;
            case State.Moving:
                MoveTo();
                break;
        }
    }
    public void StartMoving(Vector3 movePosition)
    {
        if (_currentState == State.Idling)
        {
            _currentDestination = movePosition; 
            _currentState = State.Moving;
        }
    }
    private void MoveTo()
    {
        if (Vector2.Distance(transform.position, _currentDestination) < .2f)
        {
            _rigidbody.position = _currentDestination;
            _currentState = State.Idling;
        }
        else
        {
            _rigidbody.position = Vector2.MoveTowards(transform.position, _currentDestination, speed * Time.deltaTime);
        }
    }
    
}
