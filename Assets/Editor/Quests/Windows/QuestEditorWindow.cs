using System.IO;
using Editor.Quests.Utilities;
using Quests;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Editor.Quests.Windows
{
    public class QuestEditorWindow : EditorWindow
    {
        private QuestGraphView _graphView;
        private Button _saveButton;
        private static TextField _fileNameTextField;
        private QuestInfo requiredQuest;
        
        [MenuItem("Window/Quest Graph")]
        public static void ShowWindow()
        {
            GetWindow<QuestEditorWindow>("Quest Graph");
        }

        private void OnEnable()
        {
            AddGraphView();
            AddToolbar();
            AddStyles();
        }

        private void AddGraphView()
        {
            _graphView = new QuestGraphView(this);
            _graphView.StretchToParentSize();
            rootVisualElement.Add(_graphView);
        }

        private void AddToolbar()
        {
            var toolbar = new Toolbar();
            _fileNameTextField = QuestGraphUtilities.CreateTextField("", "File Name:");
            
            _saveButton = QuestGraphUtilities.CreateButton("Save", () => Save());
            var loadButton = QuestGraphUtilities.CreateButton("Load", () => Load());
            var clearButton = QuestGraphUtilities.CreateButton("Clear", () => _graphView.ClearGraph());
            var minimapButton = QuestGraphUtilities.CreateButton("Toggle Minimap", () => _graphView.ToggleMinimap());
            
            toolbar.Add(_fileNameTextField);
            toolbar.Add(_saveButton);
            toolbar.Add(loadButton);
            toolbar.Add(clearButton);
            toolbar.Add(minimapButton);
            toolbar.AddStyleSheets("Quests/QuestToolbarStyles.uss");
            rootVisualElement.Add(toolbar);
        }
        
        private void AddStyles()
        {
            rootVisualElement.AddStyleSheets("Quests/QuestWindowVariables.uss");
        }
        
        public void EnableSaving()
        {
            _saveButton.SetEnabled(true);
        }

        public void DisableSaving()
        {
            _saveButton.SetEnabled(false);
        }

        private void Save()
        {
            if (string.IsNullOrEmpty(_fileNameTextField.value))
            {
                EditorUtility.DisplayDialog("Invalid File Name", "Cannot Save Graph with blank name", "Ok");
                return;
            }
            QuestIoUtilities.Initialize(_graphView, _fileNameTextField.value);
            QuestIoUtilities.Save();
        }

        private void Load()
        {
            var filePath = EditorUtility.OpenFilePanel("Quest Graphs", "Assets/Editor/Quests/Graphs", "asset");
            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            _graphView.ClearGraph();
            QuestIoUtilities.Initialize(_graphView, Path.GetFileNameWithoutExtension(filePath));
            QuestIoUtilities.Load();
        }
        
        public static void UpdateFileName(string fileName)
        {
            _fileNameTextField.value = fileName;
        }
    }
}
