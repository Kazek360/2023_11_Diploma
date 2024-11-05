using System;
using System.Collections.Generic;
using Quests.Flags;
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
        public string description;
        public int experience;
        public string groupID;
        public List<string> connectedNodes;
        public Vector2 position;
    }
}
