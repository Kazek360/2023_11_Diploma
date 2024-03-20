using UnityEngine;

namespace ToDelete
{
    public class TestPlayerMove : MonoBehaviour
    {
        [SerializeField] private float speed;
        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }
        
        private void Update()
        {
            if (true)
            {
                MovePlayer();
            }
        }

            void MovePlayer()
            {
                float xInput = Input.GetAxis("Horizontal");
                float yInput = Input.GetAxis("Vertical");
        
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
                {
                    _rigidbody.velocity = new Vector2(xInput, yInput).normalized * speed;
                }
                else
                {
                    _rigidbody.velocity = Vector2.zero;
                }
            }
    }
}