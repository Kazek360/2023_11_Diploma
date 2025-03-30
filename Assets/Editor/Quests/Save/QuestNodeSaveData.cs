using System;
using System.Collections.Generic;
using Quests.ScriptableObjects;
using UnityEngine;

namespace Editor.Quests.Save
{
    [Serializable]
    public class QuestNodeSaveData
    {
        public string id;
        public string fileName;
        public List<FlagInfo> flags;
        public string displayName;
        public QuestCategory category;
        [TextArea] public string description;
        public int experience;
        public string groupID;
        public List<string> connectedQuestNodes;
        public List<string> connectedQuestStepNodes;
        public Vector2 position;
    }
}
