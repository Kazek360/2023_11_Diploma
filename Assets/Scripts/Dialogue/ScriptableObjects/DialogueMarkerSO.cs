using UnityEngine;

[CreateAssetMenu(fileName = "DialogueMarker", menuName = "ScriptableObjects/Dialogues/DialogueMarker")]
public class DialogueMarkerSO : ScriptableObject
{
    public string id;
    private void OnValidate()
    {
    #if UNITY_EDITOR
        id = name;
        UnityEditor.EditorUtility.SetDirty(this);
    #endif
    }
}
