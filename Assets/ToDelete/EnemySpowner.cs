namespace ToDelete
{
    using UnityEngine;

    public class EnemySpawner : MonoBehaviour
    {
        public GameObject enemyPrefab;
        [SerializeField] public float spawnRadius;
        private Transform player;

        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        void Update()
        {
            if (Vector2.Distance(transform.position, player.position) < spawnRadius)
            {
                Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            }
        }
    }

}