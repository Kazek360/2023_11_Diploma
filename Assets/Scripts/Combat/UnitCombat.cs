using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCombat : MonoBehaviour
{
    public enum State
    {
        Idling,
        Moving
    }
    
    [SerializeField] private float speed;
    private Rigidbody2D _rigidbody;
    public State currentState;
    private Vector3 _currentDestination;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    
    private void Start()
    {
        currentState = State.Idling;
    }
    private void Update()
    {
        switch (currentState)
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
        if (currentState == State.Idling)
        {
            _currentDestination = movePosition; 
            currentState = State.Moving;
        }
    }
    private void MoveTo()
    {
        if (Vector2.Distance(transform.position, _currentDestination) < .2f)
        {
            _rigidbody.position = _currentDestination;
            currentState = State.Idling;
        }
        else
        {
            _rigidbody.position = Vector2.MoveTowards(transform.position, _currentDestination, speed * Time.deltaTime);
        }
    }
    
}
