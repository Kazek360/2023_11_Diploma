using System.Collections.Generic;
using System.Linq;
using Editor.Quests.Elements;
using Editor.Quests.Save;
using Editor.Quests.Windows;
using Quests.ScriptableObjects;
using Shared;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;

namespace Editor.Quests.Utilities
{
    public static class QuestIoUtilities
    {
        private static QuestGraphView _graphView;
        private static string _graphFileName;
        private static string _containerFolderPath;

        private static List<QuestGroup> _groups;
        private static List<QuestNode> _questNodes;
        private static List<QuestStepNode> _questStepNodes;
        
        private static Dictionary<string, QuestInfo> _createdQuests;
        private static Dictionary<string, QuestStepInfo> _createdQuestsSteps;

        private static Dictionary<string, QuestGroup> _loadedGroups;
        private static Dictionary<string, BaseNode> _loadedNodes;

        public static void Initialize(QuestGraphView graphView, string graphName)
        {
            _graphView = graphView;
            _graphFileName = graphName;
            _containerFolderPath = $"Assets/Resources/Quests/{_graphFileName}";

            _groups = new List<QuestGroup>();
            _questNodes = new List<QuestNode>();
            _questStepNodes = new List<QuestStepNode>();
            _createdQuests = new Dictionary<string, QuestInfo>();
            _createdQuestsSteps = new Dictionary<string, QuestStepInfo>();
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
            var groupedNodeNames = new SerializedDictionary<string, List<string>>();
            var ungroupedNodeNames = new List<string>();
            foreach (var node in _questNodes)
            {
                SaveQuestNodeToGraph(node, graphData);
                SaveQuestNodeToScriptableObject(node);

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
            foreach (var node in _questStepNodes)
            {
                SaveQuestStepNodeToGraph(node, graphData);
                SaveQuestStepNodeToScriptableObject(node);

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
            foreach (var node in _questNodes)
            {
                SaveQuestNodeConnectionsToScriptableObject(node);
            }
            foreach (var node in _questStepNodes)
            {
                SaveQuestStepNodeConnectionsToScriptableObject(node);
            }

            UpdateOldUngroupedNodes(ungroupedNodeNames, graphData);
            UpdateOldGroupedNodes(groupedNodeNames, graphData);
        }
        
        private static void SaveQuestNodeToGraph(QuestNode node, QuestGraphSaveDataSO graphData)
        {
            var connectedNodes = new List<string>();
            var connectedQuestStep = "";
            foreach (var edge in node.outputQuestPort.connections)
            {
                var inputNode = (QuestNode) edge.input.node;
                connectedNodes.Add(inputNode.ID);
            }
            foreach (var edge in node.outputQuestStepPort.connections)
            {
                var inputNode = (QuestStepNode) edge.input.node;
                connectedQuestStep = inputNode.ID;
            }

            var flagsCopy = new List<FlagInfo>();
            foreach (var flag in node.flags)
            {
                flagsCopy.Add(flag);
            }
            
            var nodeData = new QuestNodeSaveData
            {
                id = node.ID,
                fileName = node.fileName,
                flags = flagsCopy,
                displayName = node.displayName,
                category = node.category,
                description = node.description,
                experience = node.experience,
                groupID = node.group?.ID,
                connectedQuestNodes = connectedNodes,
                connectedQuestStepNode = connectedQuestStep,
                position = node.GetPosition().position
            };
            graphData.questNodes.Add(nodeData);
        }

        private static void SaveQuestStepNodeToGraph(QuestStepNode node, QuestGraphSaveDataSO graphData)
        {
            var connectedNodes = new List<string>();
            foreach (var edge in node.outputPort.connections)
            {
                var inputNode = (QuestStepNode) edge.input.node;
                connectedNodes.Add(inputNode.ID);
            }

            var flagsCopy = new List<FlagInfo>();
            foreach (var flag in node.flags)
            {
                flagsCopy.Add(flag);
            }
            
            var nodeData = new QuestStepNodeSaveData
            {
                id = node.ID,
                fileName = node.fileName,
                flags = flagsCopy,
                description = node.description,
                groupID = node.group?.ID,
                connectedNodes = connectedNodes,
                position = node.GetPosition().position
            };
            graphData.questStepNodes.Add(nodeData);
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
            quest.requiredFlags = new List<FlagInfo>(node.flags);
            quest.experience = node.experience;
            quest.requiredQuests = new List<QuestInfo>();
            quest.nextQuests = new List<QuestInfo>();
            _createdQuests.Add(node.ID, quest);
            SaveAsset(quest);
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
            questStep.requiredFlags = new List<FlagInfo>(node.flags);
            questStep.nextSteps = new List<QuestStepInfo>();
            _createdQuestsSteps.Add(node.ID, questStep);
            SaveAsset(questStep);
        }
        
        
        private static void SaveQuestNodeConnectionsToScriptableObject(QuestNode node)
        {
            var quest = _createdQuests[node.ID];
            foreach (var edge in node.inputPort.connections)
            {
                var outputNode = (QuestNode) edge.output.node;
                if (outputNode != null)
                {
                    quest.requiredQuests.Add(_createdQuests[outputNode.ID]);
                }
            }
            foreach (var edge in node.outputQuestPort.connections)
            {
                var inputNode = (QuestNode) edge.input.node;
                if (inputNode != null)
                {
                    quest.nextQuests.Add(_createdQuests[inputNode.ID]);
                }
            }
        }
        
        private static void SaveQuestStepNodeConnectionsToScriptableObject(QuestStepNode node)
        {
            var questStep = _createdQuestsSteps[node.ID];
            
            foreach (var edge in node.outputPort.connections)
            {
                var inputNode = (QuestStepNode) edge.input.node;
                if (inputNode != null)
                {
                    questStep.nextSteps.Add(_createdQuestsSteps[inputNode.ID]);
                }
            }
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
        
        private static void UpdateOldGroupedNodes(SerializedDictionary<string, List<string>> currentGroupedNodeNames, 
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
            LoadQuestStepNodes(graphData.questStepNodes);
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
                node.flags = nodeData.flags;
                node.displayName = nodeData.displayName;
                node.category = nodeData.category;
                node.description = nodeData.description;
                node.experience = nodeData.experience;
                node.connectedNodes = nodeData.connectedQuestNodes;
                node.connectedFirstStepNode = nodeData.connectedQuestStepNode;
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
                node.flags = nodeData.flags;
                node.description = nodeData.description;
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
                    if (!questNode.connectedFirstStepNode.Equals(""))
                    {
                        var stepPort = (Port) questNode.outputContainer.Children().ToList()[1];
                        var stepInputPort = (Port) _loadedNodes[questNode.connectedFirstStepNode].inputContainer.Children().First();
                        var edge = stepPort.ConnectTo(stepInputPort);
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
                else if(graphElement is QuestStepNode questStepNode)
                {
                    _questStepNodes.Add(questStepNode);
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
