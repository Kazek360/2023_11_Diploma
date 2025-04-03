using System;
using System.Collections.Generic;
using Player.Inventory;
using Quests.ScriptableObjects;
using UnityEngine;

namespace Editor.Quests.Save
{
    [Serializable]
    public class QuestNodeSaveData
    {
        public string id;
        public string fileName;
        public string displayName;
        public QuestCategory category;
        [TextArea] public string description;
        public int experience;
        public List<ItemWithAmount> rewards;
        public string groupID;
        public List<string> connectedQuestNodes;
        public List<string> connectedQuestStepNodes;
        public Vector2 position;
    }
}
