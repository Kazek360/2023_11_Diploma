using System;
using System.Collections.Generic;
using UnityEngine;

namespace Location
{
    public class CharacterPosition : MonoBehaviour
    {
        public static List<CharacterPosition> allPositions = new List<CharacterPosition>();

        [NonSerialized] public string characterName;
        [NonSerialized] public Transform characterTransform;
        
        private void Awake()
        {
            allPositions.Add(this);
            characterName = gameObject.name;
            characterTransform = gameObject.transform;
        }

        private void OnDestroy()
        {
            allPositions.Remove(this);
        }
    }
}