using System;
using UnityEngine;

namespace UI.Tutorials
{
    public class TutorialSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject TutorialPanelPrefab;
    
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
            if (other.CompareTag("Player"))
            {
                Instantiate(TutorialPanelPrefab);
            }
            Destroy(gameObject);
        }
    }
}
