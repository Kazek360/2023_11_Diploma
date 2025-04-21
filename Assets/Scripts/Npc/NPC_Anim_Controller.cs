using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Anim_Controller : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    
    private static readonly int X = Animator.StringToHash("X");
    private static readonly int Y = Animator.StringToHash("Y");
    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    private static readonly int IsWalking = Animator.StringToHash("isWalking");

    // Start is called before the first frame update
    public void OnEnable()
    {
        _rigidbody = GetComponentInParent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void Awake()
    {

    }
    

    public void Update()
    {
        if (_rigidbody.linearVelocity.x != 0 || _rigidbody.linearVelocity.y != 0)
        {
            _animator.SetFloat(X, _rigidbody.linearVelocity.x);
            _animator.SetFloat(Y, _rigidbody.linearVelocity.y);
                
            _animator.SetBool(IsRunning, true);
        }
        else
        {

            _animator.SetBool(IsRunning, false);
        }
    }
}