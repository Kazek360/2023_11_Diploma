using System;
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
        public string description;
        public string groupID;
        public Vector2 position;
    }
}
