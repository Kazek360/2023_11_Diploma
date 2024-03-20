using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float reloadTime;
    [SerializeField] private int enemiesToSpawn;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SpawnCoroutine()
    {
        while (enemiesToSpawn > 0)
        {
            Instantiate(enemyPrefab, transform.position, transform.rotation);
            yield return new WaitForSeconds(reloadTime);
            enemiesToSpawn--;
        }
    }
}
