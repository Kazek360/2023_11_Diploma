using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathScript : MonoBehaviour
{
    private int _currentTarget = 0;
    [SerializeField] private List<GameObject> Children;
    private List<Vector3> _targets;


    public void OnEnable()
    {
        _targets = new List<Vector3>();

        foreach (GameObject gameObject in Children)
        {
            _targets.Add(gameObject.transform.position);
        }
    }

    // Start is called before the first frame update
    public Vector3 NexTarget()
    {
        Debug.Log("aaa");
        if (_currentTarget < _targets.Count)
        {
            Debug.Log("aaa");
            Vector3 result = _targets[_currentTarget];
            _currentTarget++;
            Debug.Log("bbb");
            return result;
        }
        Debug.Log("ccc");
        return new Vector3();
    }

    public List<Vector3> GetTargets()
    {
        return _targets;
    }
}
