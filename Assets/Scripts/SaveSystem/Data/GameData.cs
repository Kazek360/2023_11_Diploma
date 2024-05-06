using System;
using UnityEngine;

namespace SaveSystem.Data
{
    [Serializable]
    public class GameData
    {
        public float[] PlayerPosition;

        public GameData()
        {
            PlayerPosition = new [] {-1423.72f, 609.44f};
        }
    }
}

