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

        // Otwiera okno edytora
        [MenuItem("Window/Quest Creator")]
        public static void ShowWindow()
        {
            GetWindow<QuestEditorWindow>("Quest Creator");
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
                QuestIoUtilities.Initialize(_graphView, _fileNameTextField.value);
                QuestIoUtilities.Save();
            });
            toolbar.Add(_fileNameTextField);
            toolbar.Add(_saveButton);
            toolbar.AddStyleSheets("Quests/QuestToolbarStyles.uss");
            rootVisualElement.Add(toolbar);
        }
        
        // Tworzenie instancji ScriptableObject
        private void CreateScriptableObject()
        {
            // Tworzymy instancję ScriptableObject
            var newQuest = CreateInstance<QuestInfo>();

            if (requiredQuest != null)
            {
                newQuest.requiredQuests = new List<QuestInfo>();
                newQuest.requiredQuests.Add(requiredQuest);
            }

            // // Ścieżka, w której zapisujemy obiekt
            // var assetPath = "Assets/Resources/Quests/" + fileName + ".asset";

            // Zapisujemy obiekt w zasobach (Assets)
            // AssetDatabase.CreateAsset(newQuest, assetPath);
            // AssetDatabase.SaveAssets();

            // Otwieramy zasób w inspektorze
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = newQuest;

            // Debug.Log("Nowy quest został utworzony i zapisany w " + assetPath);
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
