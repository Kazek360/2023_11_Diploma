using System.Collections.Generic;
using UnityEngine;
using DialogueSystem.Events;
using DialogueSystem.ScriptableObjects;

public class DialogueWithTeleport : DialogueQuestStep
{
    [SerializeField] private List<string> objectToTeleportNames;
    private List<GameObject> objectsToTeleport = new List<GameObject>();
    
    [SerializeField] private List<string> placeToTeleportNames;
    private List<GameObject> placesToTeleport = new List<GameObject>();
    
    private void OnEnable()
    {
        DialogueEvents.OnDialogueFinished += FinishStep;

        for (var i = 0; i < objectToTeleportNames.Count; i++)
        {
            objectsToTeleport.Add(GameObject.Find(objectToTeleportNames[i]));
            placesToTeleport.Add(GameObject.Find(placeToTeleportNames[i]));
        }
        DialogueEvents.UnlockDialogue(dialoguesInfo.name);
    }
    private void OnDisable()
    {
        DialogueEvents.OnDialogueFinished-= FinishStep;
    }
    
    private void FinishStep(DialogueSO dialogue)
    {
        if (dialoguesInfo.name == dialogue.name)
        {
            for (var i = 0; i < objectToTeleportNames.Count; i++)
            {
                objectsToTeleport[i].transform.position = placesToTeleport[i].transform.position;
            }
            FinishAndDelete();
        } 
    }
}
