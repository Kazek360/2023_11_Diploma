using System;
using System.Collections.Generic;
using Quests.ScriptableObjects;
using UnityEngine;

namespace Editor.Quests.Save
{
    [Serializable]
    public class QuestTransitionNodeSaveData
    {
        public string id;
        public string fileName;
        public List<FlagInfo> flags;
        public string groupID;
        public List<string> connectedNodes;
        public Vector2 position;
    }
}