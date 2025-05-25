using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class ReplaceObjectsWithPrefab : EditorWindow
{
    public GameObject prefab;
    public string objectNameToReplace;

    [MenuItem("Tools/Replace With Prefab")]
    public static void ShowWindow()
    {
        GetWindow<ReplaceObjectsWithPrefab>("Replace With Prefab");
    }

    void OnGUI()
    {
        GUILayout.Label("Replace Objects with Prefab", EditorStyles.boldLabel);
        prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), false);
        objectNameToReplace = EditorGUILayout.TextField("Object Name to Replace", objectNameToReplace);

        if (GUILayout.Button("Replace"))
        {
            Replace();
        }
    }

    void Replace()
    {
        if (prefab == null || string.IsNullOrEmpty(objectNameToReplace))
        {
            Debug.LogError("Prefab or object name is not set!");
            return;
        }

        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.name == objectNameToReplace)
            {
                GameObject newInstance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                newInstance.transform.position = obj.transform.position;
                newInstance.transform.rotation = obj.transform.rotation;
                newInstance.transform.localScale = obj.transform.localScale;
                newInstance.transform.parent = obj.transform.parent;

                Undo.RegisterCompleteObjectUndo(obj, "Replace with Prefab");
                DestroyImmediate(obj);
            }
        }

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        Debug.Log("Replacement complete!");
    }
}