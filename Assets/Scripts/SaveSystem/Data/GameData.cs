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
        public Dictionary<string, Dictionary<string, Tuple<float, float>>> characterPositions;
        public Tuple<int, int>[,] inventory;
        public int[] equipment;
        public Dictionary<string, Quest> quests;
        public string trackedQuest;
        public string inkVariables;

        public GameData()
        {
            characterPositions = new Dictionary<string, Dictionary<string, Tuple<float, float>>>();
            inventory = new Tuple<int, int>[7, 5];
            equipment = new int[5];
            saveDate = "";
            currentScene = "Las";
        }
    }
}

