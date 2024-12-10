using System.Collections.Generic;
using UnityEngine;
using Dialogues;

public class DialogueWithTeleport : DialogueQuestStep
{
    [SerializeField] private List<string> objectToTeleportNames;
    private List<GameObject> objectsToTeleport = new List<GameObject>();
    
    [SerializeField] private List<string> placeToTeleportNames;
    private List<GameObject> placesToTeleport = new List<GameObject>();
    
    protected override void OnEnable()
    {
        base.OnEnable();
        for (var i = 0; i < objectToTeleportNames.Count; i++)
        {
            objectsToTeleport.Add(GameObject.Find(objectToTeleportNames[i]));
            placesToTeleport.Add(GameObject.Find(placeToTeleportNames[i]));
        }
    }
    
    protected override void FinishStep(DialogueInfo dialogue)
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
