using System;
using System.Collections.Generic;
using UnityEngine;
using Quests.ScriptableObjects;

namespace Editor.Quests.Save
{
    [Serializable]
    public class QuestStepTransitionNodeSaveData
    {
        public string id;
        public string fileName;
        public List<FlagInfo> flags;
        public string groupID;
        public List<string> connectedNodes;
        public Vector2 position;
    }
}