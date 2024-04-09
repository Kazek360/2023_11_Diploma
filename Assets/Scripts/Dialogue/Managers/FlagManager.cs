using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagManager : MonoBehaviour
{
    private static Dictionary<string, DialogueMarker> _markerDictionary;
    
    private void Awake()
    {
        _markerDictionary = CreateQuestDictionary();
    } 
    private void OnEnable()
    {
        DialogueEvents.OnDialogueMarkerTrigger += TriggerMarker;
    }
    private void OnDisable()
    {
        DialogueEvents.OnDialogueMarkerTrigger -= TriggerMarker;
    }

    private void TriggerMarker(string id)
    {
        _markerDictionary[id]._isTriggered = true;
        DialogueEvents.ChangeDialogueMarkerState(id);
    }
    
    private Dictionary<string, DialogueMarker> CreateQuestDictionary()
    {
        FlagSO[] allMarkers = Resources.LoadAll<FlagSO>("Dialogues");
        Dictionary<string, DialogueMarker> idToDictionary = new Dictionary<string, DialogueMarker>();
        foreach (var dialogueMarkerInfo in allMarkers)
        {
            if (idToDictionary.ContainsKey(dialogueMarkerInfo.id))
            {
                Debug.LogWarning($"Found Duplicate ID: {dialogueMarkerInfo.id}");
            }
            idToDictionary.Add(dialogueMarkerInfo.id, new DialogueMarker(dialogueMarkerInfo));
        }
        return idToDictionary;
    }

    public static DialogueMarker GetMarkerById(string id)
    {
        var marker = _markerDictionary[id];
        if (marker == null)
        {
            Debug.LogError($"There is no quest with ID: {marker.info}");
        }
        return marker;
    }
    
    
}
