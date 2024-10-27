using System.Collections;
using System.Collections.Generic;
using Editor.Quests.Elements;
using Editor.Quests.Save;
using Editor.Quests.Windows;
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

        public static void Initialize(QuestGraphView graphView, string graphName)
        {
            _graphView = graphView;
            _graphFileName = graphName;
            _containerFolderPath = $"Assets/Resources/Quests/{_graphFileName}";

            _groups = new List<QuestGroup>();
            _nodes = new List<QuestNode>();
        }
        
        public static void Save()
        {
            CreateStaticFolders();
            GetElementsFromGraph();
            var graphData = CreateAsset<QuestGraphSaveDataSO>("Assets/Editor/Quests/Graphs", _graphFileName);
            graphData.Initialize(_graphFileName);
            
            SaveGroups(graphData);
            
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
        
        private static void CreateStaticFolders()
        {
            CreateFolder("Assets/Editor/Quests", "Graphs");
            CreateFolder("Assets/Resources", "Quests");
            CreateFolder("Assets/Resources/Quests", _graphFileName);
            CreateFolder(_containerFolderPath, "Global");
            CreateFolder($"{_containerFolderPath}/Global", "Quests");
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
