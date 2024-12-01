using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BangScript : MonoBehaviour
{
    [SerializeField] private float Duration;
    void Start()
    {
        StartCoroutine(DelayedDestroy());
    }

    // Update is called once per frame
    void Update()
    {
    }

    private IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(Duration);
        Destroy(gameObject);
    }
}
