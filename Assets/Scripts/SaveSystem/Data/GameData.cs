using System;
using System.Collections.Generic;
using Quests;
using Quests.Flags;
using Tutorials;

namespace SaveSystem.Data
{
    [Serializable]
    public class GameData
    {
        public SaveData save;
        public SceneData scene;
        public CharacterPositionData charactersPosition;
        public ResourcesData resources;
        public Dictionary<string, Dictionary<string, bool>> collectiblesState;
        public Dictionary<string, bool> skillTreeNodesState;
        public InventoryData inventory;
        public Dictionary<string, Quest> Quests;
        public string trackedQuest;
        public Dictionary<string, Flag> Flags;
        public Dictionary<string, Tutorial> tutorials;
        
        public GameData()
        {
            save = new SaveData();
            scene = new SceneData();
            charactersPosition = new CharacterPositionData
            {
                npcPositions = new Dictionary<string, List<CharacterPositionData.Position>>()
            };
            resources = new ResourcesData();
            inventory = new InventoryData
            {
                items = new Tuple<string, int>[6, 9],
                equipment = new string[5]
            };
            trackedQuest = "";
            collectiblesState = new Dictionary<string, Dictionary<string, bool>>();
            skillTreeNodesState = new Dictionary<string, bool>();
            tutorials = new Dictionary<string, Tutorial>();
        }
    }
}

