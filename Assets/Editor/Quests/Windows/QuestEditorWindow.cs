using System.Collections.Generic;
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
        private TextField _fileNameTextField;
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
            _saveButton = QuestGraphUtilities.CreateButton("Save", () =>
            {
                if (string.IsNullOrEmpty(_fileNameTextField.value))
                {
                    EditorUtility.DisplayDialog("Invalid File Name", "Cannot Save Graph with blank name", "Ok");
                    return;
                }
                QuestIoUtilities.Initialize(_graphView, _fileNameTextField.value);
                QuestIoUtilities.Save();
            });
            toolbar.Add(_fileNameTextField);
            toolbar.Add(_saveButton);
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
    }
}
