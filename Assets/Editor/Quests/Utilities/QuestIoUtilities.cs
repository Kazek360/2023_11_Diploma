using System.Collections.Generic;
using System.Linq;
using Editor.Quests.Elements;
using Editor.Quests.Save;
using Editor.Quests.Windows;
using Player.Inventory;
using Quests.ScriptableObjects;
using Shared;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Editor.Quests.Utilities
{
    public static class QuestIoUtilities
    {
        private static QuestGraphView _graphView;
        private static string _graphFileName;
        private static string _containerFolderPath;

        private static List<QuestGroup> _groups;
        private static List<QuestNode> _questNodes;
        private static List<QuestTransitionNode> _questTransitionNodes;
        private static List<QuestStepNode> _questStepNodes;
        private static List<QuestStepTransitionNode> _questStepTransitionNodes;
        
        private static Dictionary<string, QuestInfo> _createdQuests;
        private static Dictionary<string, QuestTransition> _createdQuestsTransitions;
        private static Dictionary<string, QuestStepInfo> _createdQuestsSteps;
        private static Dictionary<string, QuestStepTransition> _createdQuestsStepsTransitions;

        private static Dictionary<string, QuestGroup> _loadedGroups;
        private static Dictionary<string, BaseNode> _loadedNodes;

        public static void Initialize(QuestGraphView graphView, string graphName)
        {
            _graphView = graphView;
            _graphFileName = graphName;
            _containerFolderPath = $"Assets/Resources/Quests/{_graphFileName}";

            _groups = new List<QuestGroup>();
            _questNodes = new List<QuestNode>();
            _questTransitionNodes = new List<QuestTransitionNode>();
            _questStepNodes = new List<QuestStepNode>();
            _questStepTransitionNodes = new List<QuestStepTransitionNode>();
            _createdQuests = new Dictionary<string, QuestInfo>();
            _createdQuestsTransitions = new Dictionary<string, QuestTransition>();
            _createdQuestsSteps = new Dictionary<string, QuestStepInfo>();
            _createdQuestsStepsTransitions = new Dictionary<string, QuestStepTransition>();
            _loadedGroups = new Dictionary<string, QuestGroup>();
            _loadedNodes = new Dictionary<string, BaseNode>();
        }
        
        public static void Save()
        {
            CreateStaticFolders();
            GetElementsFromGraph();
            var graphData = CreateAsset<QuestGraphSaveDataSO>("Assets/Editor/Quests/Graphs", _graphFileName);
            graphData.Initialize(_graphFileName);
            
            SaveGroups(graphData);
            SaveNodes(graphData);
            
            SaveAsset(graphData);
        }
        
        private static void SaveGroups(QuestGraphSaveDataSO graphData)
        {
            var groupNames = new List<string>();
            foreach (var group in _groups)
            {
                SaveGroupToGraph(group, graphData);
                CreateGroupFolder(group);
                groupNames.Add(group.title);
            }
            
            UpdateOldGroups(groupNames, graphData);
        }

        private static void SaveGroupToGraph(QuestGroup group, QuestGraphSaveDataSO graphData)
        {
            var groupData = new QuestGroupSaveData
            {
                id = group.ID,
                name = group.title,
                position = group.GetPosition().position
            };
            
            graphData.groups.Add(groupData);
        }
        
        private static void SaveNodes(QuestGraphSaveDataSO graphData)
        {
            var groupedNodeNames = new SerializableDictionary<string, List<string>>();
            var ungroupedNodeNames = new List<string>();
            
            SaveQuestStepNodes(graphData, groupedNodeNames, ungroupedNodeNames);
            SaveQuestStepTransitionNodes(graphData, groupedNodeNames, ungroupedNodeNames);

            SaveQuestNodes(graphData, groupedNodeNames, ungroupedNodeNames);
            SaveQuestTransitionNodes(graphData, groupedNodeNames, ungroupedNodeNames);
            
            foreach (var node in _questNodes)
            {
                SaveQuestNodeConnectionsToScriptableObject(node);
            }
            foreach (var node in _questStepNodes)
            {
                SaveQuestStepNodeConnectionsToScriptableObject(node);
            }

            foreach (var node in _questTransitionNodes)
            {
                SaveQuestTransitionNodeConnectionsToScriptableObject(node);
            }
            
            foreach (var node in _questStepTransitionNodes)
            {
                SaveQuestStepTransitionNodeConnectionsToScriptableObject(node);
            }

            UpdateOldUngroupedNodes(ungroupedNodeNames, graphData);
            UpdateOldGroupedNodes(groupedNodeNames, graphData);
        }
        
        private static void AddToNameGroup(BaseNode node, SerializableDictionary<string, List<string>> groupedNodeNames, 
            List<string> ungroupedNodeNames)
        {
            if (node.group == null)
            {
                ungroupedNodeNames.Add(node.fileName);
            }
            else
            {
                if (!groupedNodeNames.ContainsKey(node.group.title))
                {
                    groupedNodeNames.Add(node.group.title, new List<string>());
                }
                groupedNodeNames[node.group.title].Add(node.fileName);
            }
        }
        
        private static void SaveQuestNodes(QuestGraphSaveDataSO graphData, 
            SerializableDictionary<string, List<string>> groupedNodeNames, List<string> ungroupedNodeNames)
        {
            foreach (var node in _questNodes)
            {
                SaveQuestNodeToGraph(node, graphData);
                SaveQuestNodeToScriptableObject(node);
                AddToNameGroup(node, groupedNodeNames, ungroupedNodeNames);
            }
        }
        
        private static void SaveQuestTransitionNodes(QuestGraphSaveDataSO graphData, 
            SerializableDictionary<string, List<string>> groupedNodeNames, List<string> ungroupedNodeNames)
        {
            foreach (var node in _questTransitionNodes)
            {
                SaveQuestTransitionNodeToGraph(node, graphData);
                SaveQuestTransitionNodeToScriptableObject(node);
                AddToNameGroup(node, groupedNodeNames, ungroupedNodeNames);
            }
        }
        
        private static void SaveQuestStepNodes(QuestGraphSaveDataSO graphData, 
            SerializableDictionary<string, List<string>> groupedNodeNames, List<string> ungroupedNodeNames)
        {
            foreach (var node in _questStepNodes)
            {
                SaveQuestStepNodeToGraph(node, graphData);
                SaveQuestStepNodeToScriptableObject(node);
                AddToNameGroup(node, groupedNodeNames, ungroupedNodeNames);
            }
        }
        
        private static void SaveQuestStepTransitionNodes(QuestGraphSaveDataSO graphData, 
            SerializableDictionary<string, List<string>> groupedNodeNames, List<string> ungroupedNodeNames)
        {
            foreach (var node in _questStepTransitionNodes)
            {
                SaveQuestStepTransitionNodeToGraph(node, graphData);
                SaveQuestStepTransitionNodeToScriptableObject(node);
                AddToNameGroup(node, groupedNodeNames, ungroupedNodeNames);
            }
        }
        
        private static void SaveQuestNodeToGraph(QuestNode node, QuestGraphSaveDataSO graphData)
        {
            var connectedQuestNodes = new List<string>();
            var connectedQuestStepNodes = new List<string>();
            foreach (var edge in node.outputQuestPort.connections)
            {
                var inputNode = (BaseNode) edge.input.node;
                connectedQuestNodes.Add(inputNode.ID);
            }
            foreach (var edge in node.outputQuestStepPort.connections)
            {
                var inputNode = (BaseNode) edge.input.node;
                connectedQuestStepNodes.Add(inputNode.ID);
            }
            
            var nodeData = new QuestNodeSaveData
            {
                id = node.ID,
                fileName = node.fileName,
                displayName = node.displayName,
                category = node.category,
                description = node.description,
                experience = node.experience,
                rewards = node.rewards,
                groupID = node.group?.ID,
                connectedQuestNodes = connectedQuestNodes,
                connectedQuestStepNodes = connectedQuestStepNodes,
                position = node.GetPosition().position
            };
            graphData.questNodes.Add(nodeData);
        }

        private static void SaveQuestTransitionNodeToGraph(QuestTransitionNode node, QuestGraphSaveDataSO graphData)
        {
            var connectedNodes = new List<string>();
            foreach (var edge in node.outputPort.connections)
            {
                var inputNode = (BaseNode) edge.input.node;
                connectedNodes.Add(inputNode.ID);
            }
            
            var flagsCopy = new List<FlagInfo>();
            foreach (var flag in node.flags)
            {
                flagsCopy.Add(flag);
            }
            
            var nodeData = new QuestTransitionNodeSaveData
            {
                id = node.ID,
                fileName = node.fileName,
                flags = flagsCopy,
                groupID = node.group?.ID,
                connectedNodes = connectedNodes,
                position = node.GetPosition().position
            };
            graphData.questTransitionNodes.Add(nodeData);
        }
        
        private static void SaveQuestStepNodeToGraph(QuestStepNode node, QuestGraphSaveDataSO graphData)
        {
            var connectedNodes = new List<string>();
            foreach (var edge in node.outputPort.connections)
            {
                var inputNode = (BaseNode) edge.input.node;
                connectedNodes.Add(inputNode.ID);
            }
            
            var nodeData = new QuestStepNodeSaveData
            {
                id = node.ID,
                fileName = node.fileName,
                description = node.description,
                isHidden = node.isHidden,
                repeatAmount = node.repeatAmount,
                prefab = node.prefab,
                groupID = node.group?.ID,
                connectedNodes = connectedNodes,
                position = node.GetPosition().position
            };
            graphData.questStepNodes.Add(nodeData);
        }

        private static void SaveQuestStepTransitionNodeToGraph(QuestStepTransitionNode node, QuestGraphSaveDataSO graphData)
        {
            var connectedNodes = new List<string>();
            foreach (var edge in node.outputPort.connections)
            {
                var inputNode = (BaseNode) edge.input.node;
                connectedNodes.Add(inputNode.ID);
            }
            
            var flagsCopy = new List<FlagInfo>();
            foreach (var flag in node.flags)
            {
                flagsCopy.Add(flag);
            }
            
            var nodeData = new QuestStepTransitionNodeSaveData
            {
                id = node.ID,
                fileName = node.fileName,
                flags = flagsCopy,
                groupID = node.group?.ID,
                connectedNodes = connectedNodes,
                position = node.GetPosition().position
            };
            graphData.questStepTransitionNodes.Add(nodeData);
        }
        
        private static void SaveQuestNodeToScriptableObject(QuestNode node)
        {
            QuestInfo quest;
            
            if (node.group != null)
            {
                quest = CreateAsset<QuestInfo>($"{_containerFolderPath}/{node.group.title}", node.fileName);
            }
            else
            {
                quest = CreateAsset<QuestInfo>($"{_containerFolderPath}/Global", node.fileName);
            }

            quest.displayName = node.displayName;
            quest.questCategory = node.category;
            quest.description = node.description;
            quest.experience = node.experience;
            quest.items = new List<ItemWithAmount>(node.rewards);
            quest.nextQuestsTransitions = new List<QuestTransition>();
            quest.firstStepTransitions = new List<QuestStepTransition>();
            _createdQuests.Add(node.ID, quest);
            SaveAsset(quest);
        }
        
        private static void SaveQuestTransitionNodeToScriptableObject(QuestTransitionNode node)
        {
            QuestTransition questTransition;
            
            if (node.group != null)
            {
                questTransition = CreateAsset<QuestTransition>($"{_containerFolderPath}/{node.group.title}", node.fileName);
            }
            else
            {
                questTransition = CreateAsset<QuestTransition>($"{_containerFolderPath}/Global", node.fileName);
            }

            questTransition.requiredFlags = new List<FlagInfo>(node.flags);
            _createdQuestsTransitions.Add(node.ID, questTransition);
            SaveAsset(questTransition);
        }
        
        private static QuestTransition CreateQuestTransitionScriptableObject(BaseNode node)
        {
            QuestTransition questTransition;
            if (node.group != null)
            {
                questTransition = CreateAsset<QuestTransition>($"{_containerFolderPath}/{node.group.title}", 
                    $"{node.fileName}_transition");
            }
            else
            {
                questTransition = CreateAsset<QuestTransition>($"{_containerFolderPath}/Global", 
                    $"{node.fileName}_transition");
            }
            
            SaveAsset(questTransition);
            return questTransition;
        }
        
        private static void SaveQuestStepNodeToScriptableObject(QuestStepNode node)
        {
            QuestStepInfo questStep;
            
            if (node.group != null)
            {
                questStep = CreateAsset<QuestStepInfo>($"{_containerFolderPath}/{node.group.title}", node.fileName);
            }
            else
            {
                questStep = CreateAsset<QuestStepInfo>($"{_containerFolderPath}/Global", node.fileName);
            }
            
            questStep.description = node.description;
            questStep.isHidden = node.isHidden;
            questStep.repeatAmount = node.repeatAmount;
            questStep.prefab = node.prefab;
            questStep.transitions = new List<QuestStepTransition>();
            _createdQuestsSteps.Add(node.ID, questStep);
            SaveAsset(questStep);
        }

        private static void SaveQuestStepTransitionNodeToScriptableObject(QuestStepTransitionNode node)
        {
            QuestStepTransition questStepTransition;
            
            if (node.group != null)
            {
                questStepTransition = CreateAsset<QuestStepTransition>($"{_containerFolderPath}/{node.group.title}", node.fileName);
            }
            else
            {
                questStepTransition = CreateAsset<QuestStepTransition>($"{_containerFolderPath}/Global", node.fileName);
            }

            questStepTransition.requiredFlags = new List<FlagInfo>(node.flags);
            _createdQuestsStepsTransitions.Add(node.ID, questStepTransition);
            SaveAsset(questStepTransition);
        }

        private static QuestStepTransition CreateQuestStepTransitionScriptableObject(BaseNode node)
        {
            QuestStepTransition questStepTransition;
            if (node.group != null)
            {
                questStepTransition = CreateAsset<QuestStepTransition>($"{_containerFolderPath}/{node.group.title}", 
                    $"{node.fileName}_step_transition");
            }
            else
            {
                questStepTransition = CreateAsset<QuestStepTransition>($"{_containerFolderPath}/Global", 
                    $"{node.fileName}_step_transition");
            }
            
            SaveAsset(questStepTransition);
            return questStepTransition;
        }
        
        private static void SaveQuestNodeConnectionsToScriptableObject(QuestNode node)
        {
            var quest = _createdQuests[node.ID];
            
            foreach (var edge in node.outputQuestPort.connections)
            {
                var inputNode = edge.input.node;
                if (inputNode is QuestTransitionNode transitionNode)
                {
                    quest.nextQuestsTransitions.Add(_createdQuestsTransitions[transitionNode.ID]);
                }
                else if (inputNode is QuestNode questStepNode)
                {
                    var questTransition = CreateQuestTransitionScriptableObject(node);
                    questTransition.nextQuest = _createdQuests[questStepNode.ID];
                    SaveAsset(questTransition);
                    quest.nextQuestsTransitions.Add(questTransition);
                }
            }

            foreach (var edge in node.outputQuestStepPort.connections)
            {
                var inputNode = edge.input.node;
                if (inputNode is QuestStepTransitionNode transitionNode)
                {
                    quest.firstStepTransitions.Add(_createdQuestsStepsTransitions[transitionNode.ID]);
                }
                else if (inputNode is QuestStepNode questStepNode)
                {
                    var questStepTransition = CreateQuestStepTransitionScriptableObject(node);
                    questStepTransition.nextStep = _createdQuestsSteps[questStepNode.ID];
                    SaveAsset(questStepTransition);
                    quest.firstStepTransitions.Add(questStepTransition);
                }
            }
            SaveAsset(quest);
        }
        
        private static void SaveQuestTransitionNodeConnectionsToScriptableObject(QuestTransitionNode node)
        {
            var questTransition = _createdQuestsTransitions[node.ID];
            
            foreach (var edge in node.outputPort.connections)
            {
                var inputNode = (QuestNode) edge.input.node;
                if (inputNode != null)
                {
                    questTransition.nextQuest = _createdQuests[inputNode.ID];
                }
            }
            SaveAsset(questTransition);
        }
        
        private static void SaveQuestStepNodeConnectionsToScriptableObject(QuestStepNode node)
        {
            var questStep = _createdQuestsSteps[node.ID];
            
            foreach (var edge in node.outputPort.connections)
            {
                var inputNode = edge.input.node;
                
                if (inputNode is QuestStepTransitionNode transitionNode)
                {
                    questStep.transitions.Add(_createdQuestsStepsTransitions[transitionNode.ID]);
                }
                else if (inputNode is QuestStepNode questStepNode)
                {
                    var questStepTransition = CreateQuestStepTransitionScriptableObject(node);
                    questStepTransition.nextStep = _createdQuestsSteps[questStepNode.ID];
                    SaveAsset(questStepTransition);
                    questStep.transitions.Add(questStepTransition);
                }
            }
            SaveAsset(questStep);
        }

        private static void SaveQuestStepTransitionNodeConnectionsToScriptableObject(QuestStepTransitionNode node)
        {
            var questStepTransition = _createdQuestsStepsTransitions[node.ID];
            
            foreach (var edge in node.outputPort.connections)
            {
                var inputNode = (QuestStepNode) edge.input.node;
                if (inputNode != null)
                {
                    questStepTransition.nextStep = _createdQuestsSteps[inputNode.ID];
                }
            }
            SaveAsset(questStepTransition);
        }
        
        private static void UpdateOldGroups(List<string> currentGroupNames, QuestGraphSaveDataSO graphData)
        {
            if (graphData.oldGroupNames != null && graphData.oldGroupNames.Count > 0)
            {
                var groupsToRemove = graphData.oldGroupNames.Except(currentGroupNames).ToList();
                foreach (var groupToRemove in groupsToRemove)
                {
                    RemoveFolder($"{_containerFolderPath}/{groupToRemove}");
                }
            }
            graphData.oldGroupNames = new List<string>(currentGroupNames);
        }
        
        private static void UpdateOldUngroupedNodes(List<string> currentUngroupedNodeNames, QuestGraphSaveDataSO graphData)
        {
            if (graphData.oldUngroupedNodeNames != null && graphData.oldUngroupedNodeNames.Count > 0)
            {
                var nodesToRemove = graphData.oldUngroupedNodeNames.Except(currentUngroupedNodeNames).ToList();
                foreach (var nodeToRemove in nodesToRemove)
                {
                    RemoveAsset($"{_containerFolderPath}/Global", nodeToRemove);
                }
            }
            graphData.oldUngroupedNodeNames = new List<string>(currentUngroupedNodeNames);
        }
        
        private static void UpdateOldGroupedNodes(SerializableDictionary<string, List<string>> currentGroupedNodeNames, 
            QuestGraphSaveDataSO graphData)
        {
            if (graphData.oldGroupedNodeNames != null && graphData.oldGroupedNodeNames.Count > 0)
            {
                foreach (var oldGroupedNode in graphData.oldGroupedNodeNames)
                {
                    var nodesToRemove = new List<string>();
                    if (currentGroupedNodeNames.ContainsKey(oldGroupedNode.Key))
                    {
                        nodesToRemove = oldGroupedNode.Value.Except(currentGroupedNodeNames[oldGroupedNode.Key])
                            .ToList();

                        foreach (var nodeToRemove in nodesToRemove)
                        {
                            RemoveAsset($"{_containerFolderPath}/{oldGroupedNode.Key}", nodeToRemove);
                        }
                    }
                }
            }
            graphData.oldGroupedNodeNames = new SerializableDictionary<string, List<string>>(currentGroupedNodeNames);
        }

        public static void Load()
        {
            var graphData = AssetDatabase.LoadAssetAtPath<QuestGraphSaveDataSO>($"Assets/Editor/Quests/Graphs/{_graphFileName}.asset");
            if (graphData == null)
            {
                EditorUtility.DisplayDialog(
                    "Couldn't load the file",
                    "The file at the following path could not be found:\n\n" + 
                    $"Assets/Editor/Quests/Graphs/{_graphFileName}.asset",
                    "Ok"
                );
                return;
            }
            QuestEditorWindow.UpdateFileName(graphData.fileName);
            LoadGroups(graphData.groups);
            LoadQuestNodes(graphData.questNodes);
            LoadQuestTransitionNodes(graphData.questTransitionNodes);
            LoadQuestStepNodes(graphData.questStepNodes);
            LoadQuestStepTransitionNodes(graphData.questStepTransitionNodes);
            LoadNodeConnections();
        }

        private static void LoadGroups(List<QuestGroupSaveData> groups)
        {
            foreach (var groupData in groups)
            {
                var group = _graphView.CreateGroup(groupData.name, groupData.position);
                group.ID = groupData.id;
                _loadedGroups.Add(group.ID, group);
            }
        }
        
        private static void LoadQuestNodes(List<QuestNodeSaveData> nodes)
        {
            foreach (var nodeData in nodes)
            {
                var node = _graphView.CreateQuestNode(nodeData.fileName, nodeData.position, false);
                node.ID = nodeData.id;
                node.displayName = nodeData.displayName;
                node.category = nodeData.category;
                node.description = nodeData.description;
                node.experience = nodeData.experience;
                node.rewards = nodeData.rewards;
                node.connectedNodes = nodeData.connectedQuestNodes;
                node.connectedFirstStepNodes = nodeData.connectedQuestStepNodes;
                node.Draw();
                _graphView.AddElement(node);

                _loadedNodes.Add(node.ID, node);
                
                if (!string.IsNullOrEmpty(nodeData.groupID))
                {
                    var group = _loadedGroups[nodeData.groupID];
                    node.group = group;
                    group.AddElement(node);
                }
            }
        }

        private static void LoadQuestTransitionNodes(List<QuestTransitionNodeSaveData> nodes)
        {
            foreach (var nodeData in nodes)
            {
                var node = _graphView.CreateQuestTransitionNode(nodeData.fileName, nodeData.position, false);
                node.ID = nodeData.id;
                node.flags = nodeData.flags;
                node.connectedNodes = nodeData.connectedNodes;
                node.Draw();
                _graphView.AddElement(node);

                _loadedNodes.Add(node.ID, node);
                
                if (!string.IsNullOrEmpty(nodeData.groupID))
                {
                    var group = _loadedGroups[nodeData.groupID];
                    node.group = group;
                    group.AddElement(node);
                }
            }
        }
        
        private static void LoadQuestStepNodes(List<QuestStepNodeSaveData> nodes)
        {
            foreach (var nodeData in nodes)
            {
                var node = _graphView.CreateQuestStepNode(nodeData.fileName, nodeData.position, false);
                node.ID = nodeData.id;
                node.description = nodeData.description;
                node.isHidden = nodeData.isHidden;
                node.repeatAmount = nodeData.repeatAmount;
                node.prefab = nodeData.prefab;
                node.connectedNodes = nodeData.connectedNodes;
                node.Draw();
                _graphView.AddElement(node);

                _loadedNodes.Add(node.ID, node);
                
                 if (!string.IsNullOrEmpty(nodeData.groupID))
                 {
                     var group = _loadedGroups[nodeData.groupID];
                     node.group = group;
                     group.AddElement(node);
                 }
            }
        }
        
        private static void LoadQuestStepTransitionNodes(List<QuestStepTransitionNodeSaveData> nodes)
        {
            foreach (var nodeData in nodes)
            {
                var node = _graphView.CreateQuestStepTransitionNode(nodeData.fileName, nodeData.position, false);
                node.ID = nodeData.id;
                node.flags = nodeData.flags;
                node.connectedNodes = nodeData.connectedNodes;
                node.Draw();
                _graphView.AddElement(node);

                _loadedNodes.Add(node.ID, node);
                
                if (!string.IsNullOrEmpty(nodeData.groupID))
                {
                    var group = _loadedGroups[nodeData.groupID];
                    node.group = group;
                    group.AddElement(node);
                }
            }
        }
        
        private static void LoadNodeConnections()
        {
            foreach (var loadedNode in _loadedNodes)
            {
                var outputPort = (Port) loadedNode.Value.outputContainer.Children().First();
                foreach (var connectedNode in loadedNode.Value.connectedNodes)
                {
                    var inputPort = (Port) _loadedNodes[connectedNode].inputContainer.Children().First();
                    var edge = outputPort.ConnectTo(inputPort);
                    _graphView.AddElement(edge);
                    loadedNode.Value.RefreshPorts();
                }

                if (loadedNode.Value is QuestNode questNode)
                {
                    var stepPort = (Port) questNode.outputContainer.Children().ToList()[1];
                    
                    foreach (var connectedNode in questNode.connectedFirstStepNodes)
                    {
                        var inputPort = (Port) _loadedNodes[connectedNode].inputContainer.Children().First();
                        var edge = stepPort.ConnectTo(inputPort);
                        _graphView.AddElement(edge);
                        questNode.RefreshPorts();
                    }
                }
            }
        }
        
        private static void CreateStaticFolders()
        {
            CreateFolder("Assets/Editor/Quests", "Graphs");
            CreateFolder("Assets/Resources", "Quests");
            CreateFolder("Assets/Resources/Quests", _graphFileName);
            CreateFolder(_containerFolderPath, "Global");
        }

        private static void CreateGroupFolder(QuestGroup group)
        {
            CreateFolder(_containerFolderPath, group.title);
        }
        
        private static void CreateFolder(string path, string folderName)
        {
            if (AssetDatabase.IsValidFolder($"{path}/{folderName}"))
            {
                return;
            }
            AssetDatabase.CreateFolder(path, folderName);
        }

        private static void RemoveFolder(string fullPath)
        {
            FileUtil.DeleteFileOrDirectory($"{fullPath}.meta");
            FileUtil.DeleteFileOrDirectory($"{fullPath}/");
        }
        
        private static void GetElementsFromGraph()
        {
            _graphView.graphElements.ForEach(graphElement =>
            {
                if (graphElement is QuestNode questNode)
                {
                    _questNodes.Add(questNode);
                }
                else if(graphElement is QuestTransitionNode questTransitionNode)
                {
                    _questTransitionNodes.Add(questTransitionNode);
                }
                else if(graphElement is QuestStepNode questStepNode)
                {
                    _questStepNodes.Add(questStepNode);
                }
                else if(graphElement is QuestStepTransitionNode questStepTransitionNode)
                {
                    _questStepTransitionNodes.Add(questStepTransitionNode);
                }
                else if(graphElement is QuestGroup group)
                {
                    _groups.Add(group);
                }
            });
        }

        private static T CreateAsset<T>(string path, string assetName) where T : ScriptableObject
        {
            var fullPath = $"{path}/{assetName}.asset";
            var asset = AssetDatabase.LoadAssetAtPath<T>(fullPath);

            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(asset, fullPath);
            }

            return asset;
        }

        private static void RemoveAsset(string path, string assetName)
        {
            AssetDatabase.DeleteAsset($"{path}/{assetName}.asset");
        }
        
        private static void SaveAsset(UnityEngine.Object asset)
        {
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}