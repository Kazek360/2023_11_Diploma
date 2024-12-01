using System;
using UnityEngine;

namespace Location
{
    public class PlayerPosition : MonoBehaviour
    {
        public static PlayerPosition instance;
        [NonSerialized] public Transform playerTransform;
        
        private void Awake()
        {
            instance = this;
            playerTransform = gameObject.transform;
        }
        private void OnDestroy()
        {
            instance = null;
        }
    }
}