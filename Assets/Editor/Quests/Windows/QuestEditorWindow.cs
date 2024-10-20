using System.Collections.Generic;
using Editor.Quests.Utilities;
using Quests;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.Quests.Windows
{
    public class QuestEditorWindow : EditorWindow
    {
        // Zmienna do przechowywania nazwy pliku
        private string fileName = "";
        
        private QuestInfo requiredQuest;

        // Otwiera okno edytora
        [MenuItem("Window/Quest Creator")]
        public static void ShowWindow()
        {
            GetWindow<QuestEditorWindow>("Quest Creator");
        }

        private void OnEnable()
        {
            var questGraphView = new QuestGraphView();
            questGraphView.StretchToParentSize();
            rootVisualElement.Add(questGraphView);
            AddStyles();
        }

        // Tworzenie instancji ScriptableObject
        private void CreateScriptableObject()
        {
            // Sprawdzamy, czy nazwa pliku nie jest pusta
            if (string.IsNullOrEmpty(fileName))
            {
                Debug.LogError("Nazwa pliku nie może być pusta!");
                return;
            }

            // Tworzymy instancję ScriptableObject
            var newQuest = CreateInstance<QuestInfo>();
            //newQuest.displayName = displayName;
            //newQuest.description = description;

            if (requiredQuest != null)
            {
                newQuest.requiredQuests = new List<QuestInfo>();
                newQuest.requiredQuests.Add(requiredQuest);
            }

            // Ścieżka, w której zapisujemy obiekt
            var assetPath = "Assets/Resources/Quests/" + fileName + ".asset";

            // Zapisujemy obiekt w zasobach (Assets)
            AssetDatabase.CreateAsset(newQuest, assetPath);
            AssetDatabase.SaveAssets();

            // Otwieramy zasób w inspektorze
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = newQuest;

            Debug.Log("Nowy quest został utworzony i zapisany w " + assetPath);
        }
        
        private void AddStyles()
        {
            rootVisualElement.AddStyleSheets("Quests/QuestWindowVariables.uss");
        }
    }
}
