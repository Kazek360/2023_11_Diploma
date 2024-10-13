using System.Collections.Generic;
using Quests;
using UnityEditor;
using UnityEngine;

public class QuestEditorWindow : EditorWindow
{
    // Zmienna do przechowywania nazwy pliku
    private string fileName = "";
    private string displayName = "";
    private string description = "";
    private QuestInfo requiredQuest;
    
    // Otwiera okno edytora
    [MenuItem("Window/Quest Creator")]
    public static void ShowWindow()
    {
        GetWindow<QuestEditorWindow>("Quest Creator");
    }
    
    // Rysowanie GUI okna
    private void OnGUI()
    {
        // Pole tekstowe dla nazwy pliku
        fileName = EditorGUILayout.TextField("File Name", fileName);
        
        displayName = EditorGUILayout.TextField("Display Name", displayName);
        
        GUILayout.Label("Description", EditorStyles.label);
        description = EditorGUILayout.TextArea(description, GUILayout.Height(60));
        
        GUILayout.Label("Required Quest");
        requiredQuest = (QuestInfo)EditorGUILayout.ObjectField(requiredQuest, typeof(QuestInfo), false);
        
        if (GUILayout.Button("Create new Quest"))
        {
            CreateScriptableObject();
        }
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
        newQuest.displayName = displayName;
        newQuest.description = description;
    
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
}