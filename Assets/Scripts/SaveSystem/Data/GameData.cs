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
        public Dictionary<string, Quest> quests;

        public GameData()
        {
            playerPosition = new [] {-1423.72f, 609.44f};
            currentScene = "MainMenu";
        }
    }
}

