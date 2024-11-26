using System;
using System.Collections.Generic;
using Quests;
using Quests.Flags;

namespace SaveSystem.Data
{
    [Serializable]
    public class GameData
    {
        public long lastUpdated;
        public string saveDate;
        public SceneData scene;
        public Tuple<float, float> playerPosition;
        public Dictionary<string, Dictionary<string, Tuple<float, float>>> charactersPosition;
        public Dictionary<string, Dictionary<string, bool>> collectiblesState;
        public Dictionary<string, bool> skillTreeNodesState;
        public Dictionary<string, int> resources;
        public Tuple<int, int>[,] inventory;
        public int[] equipment;
        public Dictionary<string, Quest> Quests;
        public string trackedQuest;
        public Dictionary<string, Flag> Flags;
        public string inkVariables;

        public GameData()
        {
            scene = new SceneData();
            charactersPosition = new Dictionary<string, Dictionary<string, Tuple<float, float>>>();
            collectiblesState = new Dictionary<string, Dictionary<string, bool>>();
            resources = new Dictionary<string, int>();
            skillTreeNodesState = new Dictionary<string, bool>();
            inventory = new Tuple<int, int>[7, 5];
            equipment = new int[5];
            saveDate = "";
        }
    }
}

