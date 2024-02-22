using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private string locationName;
    [SerializeField] private string characterName;
    private Dictionary<string, DialogueSO> _dialogueDictionary;
    
    private void Awake()
    {
        _dialogueDictionary = CreateDialogueDictionary();
    }

    private DialogueSO CurrentDialogue()
    {
        DialogueSO result = null;
        foreach (var dialogue in _dialogueDictionary.Values)
        {
            if (result is null)
            {
                result = dialogue;
                continue;
            }

            if (result.priority < dialogue.priority)
            {
                result = dialogue;
            }
        }

        return result;
    }
    
    private Dictionary<string, DialogueSO> CreateDialogueDictionary()
    {
        DialogueSO[] allDialogues = Resources.LoadAll<DialogueSO>($"Dialogues/{locationName}/{characterName}");
        
        Dictionary<string, DialogueSO> idToMap = new Dictionary<string, DialogueSO>();
        // foreach (var dialogueInfo in allDialogues)
        // {
        //     if (idToMap.ContainsKey(dialogueInfo.id))
        //     {
        //         Debug.LogWarning($"Found Duplicate ID: {dialogueInfo.id}");
        //     }
        //     idToMap.Add(dialogueInfo.id, new Dialogue(dialogueInfo));
        // }

        return idToMap;
    }

    private DialogueSO GetDialogueById(string id)
    {
        DialogueSO dialogue = _dialogueDictionary[id];
        if (dialogue == null)
        {
            //Debug.LogError($"There is no quest with ID: {dialogue.Info}");
        }
        
        return dialogue;
    }
    
}
