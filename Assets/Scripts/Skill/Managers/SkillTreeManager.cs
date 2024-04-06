using System.Collections.Generic;
using Skill.Events;
using UnityEngine;

namespace Skill.Managers
{
    public class SkillTreeManager : MonoBehaviour
    {
        private static Dictionary<string, SkillTreeNode> _skillTreeNodesDictionary;
        
        private void OnEnable()
        {
            SkillTreeEvents.OnNodeActivation += ActivateNode;
        }
        private void OnDisable()
        {
            SkillTreeEvents.OnNodeActivation -= ActivateNode;
        }
        private void Awake()
        {
            _skillTreeNodesDictionary = CreateSkillTreeNodesDictionary();
        }
        private void Start()
        {
            _skillTreeNodesDictionary = CreateSkillTreeNodesDictionary();
            foreach (var node in _skillTreeNodesDictionary.Values)
            {
                if (node.state == SkillTreeNode.State.CannotBeActivated && CheckRequirementsMet(node))
                {
                    node.state = SkillTreeNode.State.NotActivated;
                }
            }
        }
        
        private void ActivateNode(string id)
        {
            _skillTreeNodesDictionary[id].state = SkillTreeNode.State.Activated;
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
                if (GetNodeById(nodeRequirement.id).state != SkillTreeNode.State.Activated)
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
                if (idToDictionary.ContainsKey(nodeInfo.id))
                {
                    Debug.LogWarning($"Found Duplicate ID: {nodeInfo.id}");
                }
                idToDictionary.Add(nodeInfo.id, new SkillTreeNode(nodeInfo));
            }
            return idToDictionary;
        }
        
        public static SkillTreeNode GetNodeById(string id)
        {
            var skillTreeNode = _skillTreeNodesDictionary[id];
            if (skillTreeNode == null)
            {
                Debug.LogError($"There is no quest with ID: {skillTreeNode.Info}");
            }
            return skillTreeNode;
        }
    }
}

