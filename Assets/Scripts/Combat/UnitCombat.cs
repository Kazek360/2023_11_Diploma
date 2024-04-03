using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UnitCombat : MonoBehaviour
{
    public enum State
    {
        Idling,
        Moving
    }
    
    [FormerlySerializedAs("speed")] [SerializeField] private float Speed;
    private Rigidbody2D _rigidbody;
    public State currentState;
    private Vector3 _currentDestination;
    [FormerlySerializedAs("gridX")] [SerializeField] private int GridX;
    [FormerlySerializedAs("gridY")] [SerializeField] private int GridY;

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
            _rigidbody.position = Vector2.MoveTowards(transform.position, _currentDestination, Speed * Time.deltaTime);
        }
    }

    public Vector2 GetGridLocation()
    {
        return new Vector2(GridX, GridY);
    }

    public void SetGridLocation(int x, int y)
    {
        GridX = x;
        GridY = y;
    }
    
}
