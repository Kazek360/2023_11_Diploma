using System.Collections.Generic;
using SaveSystem;
using SaveSystem.Data;
using SaveSystem.Interfaces;
using Shared;
using Skill.Events;
using UnityEngine;

namespace Skill.Managers
{
    public class SkillTreeManager : Singleton<SkillTreeManager>, ISavable, IFromFileLoadable, IBetweenScenesLoadable
    {
        private static Dictionary<string, SkillTreeNode> _skillTreeNodesDictionary;
        
        private void OnEnable()
        {
            SaveSystemEvents.OnFromFileLoad += LoadDataFromSaveFile;
            SaveSystemEvents.OnBetweenScenesLoad += LoadDataBetweenScenes;
            
            SkillTreeEvents.OnNodeActivation += ActivateNode;
        }
        private void OnDisable()
        {
            SaveSystemEvents.OnFromFileLoad -= LoadDataFromSaveFile;
            SaveSystemEvents.OnBetweenScenesLoad -= LoadDataBetweenScenes;
            
            SkillTreeEvents.OnNodeActivation -= ActivateNode;
        }
        protected override void Awake()
        {
            base.Awake();
            _skillTreeNodesDictionary = CreateSkillTreeNodesDictionary();
            EnableActivation();
        }
        
        private void ActivateNode(string id)
        {
            _skillTreeNodesDictionary[id].state = SkillTreeNode.State.Activated;
            EnableActivation();
        }

        private void EnableActivation()
        {
            foreach (var node in _skillTreeNodesDictionary.Values)
            {
                if (node.state == SkillTreeNode.State.CannotBeActivated && CheckRequirementsMet(node))
                {
                    node.state = SkillTreeNode.State.NotActivated;
                }
            }
        }
        
        private bool CheckRequirementsMet(SkillTreeNode node)
        {
            bool meetsRequirements = true;
            foreach (var nodeRequirement in node.Info.requiredNodes)
            {
                if (GetNodeByName(nodeRequirement.name).state != SkillTreeNode.State.Activated)
                {
                    meetsRequirements = false;
                }
            }
            return meetsRequirements;
        }
        
        private Dictionary<string, SkillTreeNode> CreateSkillTreeNodesDictionary()
        {
            SkillTreeNodeSO[] allNodes = Resources.LoadAll<SkillTreeNodeSO>("SkillTreeNodes");
            Dictionary<string, SkillTreeNode> idToDictionary = new Dictionary<string, SkillTreeNode>();
            foreach (var nodeInfo in allNodes)
            {
                if (idToDictionary.ContainsKey(nodeInfo.name))
                {
                    Debug.LogWarning($"Found Duplicate ID: {nodeInfo.name}");
                }
                idToDictionary.Add(nodeInfo.name, new SkillTreeNode(nodeInfo));
            }
            return idToDictionary;
        }
        
        public static SkillTreeNode GetNodeByName(string id)
        {
            var skillTreeNode = _skillTreeNodesDictionary[id];
            if (skillTreeNode == null)
            {
                Debug.LogError($"There is no quest with ID: {skillTreeNode.Info}");
            }
            return skillTreeNode;
        }
        
        public void LoadDataFromSaveFile(GameData gameData)
        {
            foreach (var skillTreeNode in gameData.skillTreeNodesState)
            {
                if (skillTreeNode.Value)
                {
                    _skillTreeNodesDictionary[skillTreeNode.Key].state = SkillTreeNode.State.Activated;
                }
            }
            EnableActivation();
        }

        public void LoadDataBetweenScenes(GameData gameData)
        {
            EnableActivation();
        }

        public void SaveData(ref GameData gameData)
        {
            foreach (var skillTreeNode in _skillTreeNodesDictionary)
            {
                var isActivated = skillTreeNode.Value.state == SkillTreeNode.State.Activated;
                if (!gameData.skillTreeNodesState.TryAdd(skillTreeNode.Key, isActivated))
                {
                    gameData.skillTreeNodesState[skillTreeNode.Key] = isActivated;
                }
            }
        }
    }
}

