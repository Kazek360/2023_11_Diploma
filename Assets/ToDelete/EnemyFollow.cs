namespace ToDelete
{
    using UnityEngine;

    public class EnemyFollow : MonoBehaviour
    {
        [SerializeField] public float speed;
        private Transform target;

        void Start()
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        void Update()
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }

}