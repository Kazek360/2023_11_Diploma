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
        public Dictionary<string, Dictionary<string, Tuple<float, float>>> charactersPosition;
        public Dictionary<string, Dictionary<string, bool>> collectiblesState;
        public Tuple<int, int>[,] inventory;
        public int[] equipment;
        public Dictionary<string, Quest> quests;
        public string trackedQuest;
        public string inkVariables;

        public GameData()
        {
            charactersPosition = new Dictionary<string, Dictionary<string, Tuple<float, float>>>();
            collectiblesState = new Dictionary<string, Dictionary<string, bool>>();
            inventory = new Tuple<int, int>[7, 5];
            equipment = new int[5];
            saveDate = "";
            currentScene = "Las";
        }
    }
}

