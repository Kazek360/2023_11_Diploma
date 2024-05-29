using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFilterApplier : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject filterPrefab;

    private GameObject filter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (filter is null)
        {
            filter = Instantiate(filterPrefab, canvas.transform);
            filter.transform.SetSiblingIndex(0);
        }
    }
}
