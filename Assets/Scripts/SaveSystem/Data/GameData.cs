using System;
using System.Collections.Generic;
using QuestSystem;

namespace SaveSystem.Data
{
    [Serializable]
    public class GameData
    {
        public long lastUpdated;
        public string saveDate;
        public string currentScene;
        public float[] playerPosition;
        public float[] hrycPosition;
        public Tuple<int, int>[,] inventory;
        public int[] equipment;
        public Dictionary<string, Quest> quests;
        public string trackedQuest;
        public string inkVariables;

        public GameData()
        {
            hrycPosition = new[] {-1409.893f, 592.45f};
            playerPosition = new[] {-1423.72f, 609.44f};
            inventory = new Tuple<int, int>[7, 5];
            equipment = new int[5];
            saveDate = "";
            currentScene = "Las";
        }
    }
}

