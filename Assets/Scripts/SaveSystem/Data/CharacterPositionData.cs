using System;
using System.Collections.Generic;

namespace SaveSystem.Data
{
    public class CharacterPositionData
    {
        [Serializable]
        public class Position
        {
            public string name;
            public float x;
            public float y;
        }
        public Position playerPosition;
        public Dictionary<string, List<Position>> npcPositions;
    }
}