using System.Collections.Generic;
using Shared;
using UnityEngine;

namespace Editor.Quests.Save
{
    public class QuestGraphSaveDataSO : ScriptableObject
    {
        public string fileName;
        [SerializeField] public List<QuestGroupSaveData> groups;
        public List<QuestNodeSaveData> questNodes;
        public List<QuestStepNodeSaveData> questStepNodes;
        public List<string> oldGroupNames;
        public List<string> oldUngroupedNodeNames;
        public SerializableDictionary<string, List<string>> oldGroupedNodeNames;

        public void Initialize(string nameOfFile)
        {
            fileName = nameOfFile;
            groups = new List<QuestGroupSaveData>();
            questNodes = new List<QuestNodeSaveData>();
            questStepNodes = new List<QuestStepNodeSaveData>();
        }
    }
}
