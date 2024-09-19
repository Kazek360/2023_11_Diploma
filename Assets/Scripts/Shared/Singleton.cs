using UnityEngine;

namespace Shared {
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        [SerializeField] protected bool dontDestroyOnLoad;
        
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance) return _instance;
                return _instance = FindObjectOfType<T>();
            }
            protected set => _instance = value;
        }

        protected void Awake()
        {
            if (_instance is null && dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject); 
            }
            else
            {
                if (dontDestroyOnLoad)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

}
