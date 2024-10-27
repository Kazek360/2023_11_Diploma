using System;
using UnityEngine;

namespace Editor.Quests.Save
{
    [Serializable]
    public class QuestNodeSaveData
    {
        public string ID { get; set; }
        public string FileName { get; set; }
        public string DisplayName { get; set; }
        public QuestCategory Category { get; set; }
        public string Description { get; set; }
        public string GroupID { get; set; }
        public Vector2 Position { get; set; }
    }
}
