using System;
using System.Collections.Generic;
using Quests.ScriptableObjects;
using UnityEngine;

namespace Editor.Quests.Save
{
    [Serializable]
    public class QuestStepNodeSaveData
    {
        public string id;
        public string fileName;
        public List<FlagInfo> flags;
        public string description;
        public string groupID;
        public List<string> connectedNodes;
        public Vector2 position;
    }
}