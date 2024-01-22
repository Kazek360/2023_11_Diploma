using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> turnOrder;
    private List<IHittable> _turnOrder;

    private int _currentEntity;
    // Start is called before the first frame update
    void Start()
    {
        _currentEntity = -1;
        _turnOrder = new List<IHittable>();
        foreach (var entity in turnOrder)
        {
            _turnOrder.Add(entity.GetComponent<IHittable>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextTurn()
    {
        _currentEntity = (_currentEntity + 1) % _turnOrder.Count;
        _turnOrder[_currentEntity].ProcessTurn();
    }
}
