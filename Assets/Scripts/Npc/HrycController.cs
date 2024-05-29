using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HrycController : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    
    private static readonly int X = Animator.StringToHash("X");
    private static readonly int Y = Animator.StringToHash("Y");
    private static readonly int IsRunning = Animator.StringToHash("isRunning");

    // Start is called before the first frame update
    public void OneEnable()
    {
    }

    // Update is called once per frame
    public void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        
        if (_rigidbody.position.x != 0 || _rigidbody.velocity.y != 0)
            {
                _animator.SetFloat(X, _rigidbody.velocity.x);
                _animator.SetFloat(Y, _rigidbody.velocity.y);
                
                _animator.SetBool(IsRunning, true);
            }
            else
            {

                _animator.SetBool(IsRunning, false);
            }
    }
    
}