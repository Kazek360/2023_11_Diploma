using System.Collections;
using System.Collections.Generic;
using Editor.Quests.Elements;
using Editor.Quests.Save;
using Editor.Quests.Windows;
using Quests;
using UnityEditor;
using UnityEngine;

namespace Editor.Quests.Utilities
{
    public static class QuestIoUtilities
    {
        private static QuestGraphView _graphView;
        private static string _graphFileName;
        private static string _containerFolderPath;

        private static List<QuestGroup> _groups;
        private static List<QuestNode> _nodes;
        private static Dictionary<string, QuestInfo> _createdQuests;

        public static void Initialize(QuestGraphView graphView, string graphName)
        {
            _graphView = graphView;
            _graphFileName = graphName;
            _containerFolderPath = $"Assets/Resources/Quests/{_graphFileName}";

            _groups = new List<QuestGroup>();
            _nodes = new List<QuestNode>();
            _createdQuests = new Dictionary<string, QuestInfo>();
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
            foreach (var group in _groups)
            {
                SaveGroupToGraph(group, graphData);
                CreateGroupFolder(group);
            }
        }

        private static void SaveNodes(QuestGraphSaveDataSO graphData)
        {
            foreach (var node in _nodes)
            {
                SaveNodeToGraph(node, graphData);
                SaveNodeToScriptableObject(node);
            }
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

        private static void SaveNodeToGraph(QuestNode node, QuestGraphSaveDataSO graphData)
        {
            var nodeData = new QuestNodeSaveData
            {
                id = node.ID,
                fileName = node.fileName,
                displayName = node.displayName,
                category = node.category,
                description = node.description,
                groupID = node.group?.ID,
                position = node.GetPosition().position
            };
            graphData.nodes.Add(nodeData);
        }

        private static void SaveNodeToScriptableObject(QuestNode node)
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
            _createdQuests.Add(node.ID, quest);
            SaveAsset(quest);
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

        private static void GetElementsFromGraph()
        {
            _graphView.graphElements.ForEach(graphElement =>
            {
                if (graphElement is QuestNode node)
                {
                    _nodes.Add(node);
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

        private static void SaveAsset(UnityEngine.Object asset)
        {
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

}
