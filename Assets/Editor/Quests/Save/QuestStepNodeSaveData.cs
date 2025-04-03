using System;
using System.Collections.Generic;
using UnityEngine;

namespace Editor.Quests.Save
{
    [Serializable]
    public class QuestStepNodeSaveData
    {
        public string id;
        public string fileName;
        [TextArea] public string description;
        public bool isHidden;
        public int repeatAmount;
        public GameObject prefab;
        public string groupID;
        public List<string> connectedNodes;
        public Vector2 position;
    }
}