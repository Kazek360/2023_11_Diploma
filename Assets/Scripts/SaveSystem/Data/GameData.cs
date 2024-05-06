using System;
using UnityEngine;

namespace SaveSystem.Data
{
    [Serializable]
    public class GameData
    {
        public Vector3 PlayerPosition;

        public GameData()
        {
            PlayerPosition = new Vector3(-1423.72f, 609.44f, 0);
        }
    }
}

