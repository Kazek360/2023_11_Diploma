using System;
using System.Collections.Generic;
using QuestSystem;

namespace SaveSystem.Data
{
    [Serializable]
    public class GameData
    {
        public string currentScene;
        public float[] playerPosition;
        public float[] hrycPosition;
        public Dictionary<string, Quest> quests;
        public string inkVariables;

        public GameData()
        {
            hrycPosition = new[] {-1409.893f, 592.45f};
            playerPosition = new[] {-1423.72f, 609.44f};
            currentScene = "MainMenu";
            inkVariables = "";
        }
    }
}

