using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
 * A scriptable object containing a list of dialogues available to an NPC.
 */
[CreateAssetMenu(fileName = "DialogueChooserScriptableObject", menuName = "ScriptableObjects/DialoguesOld/DialogueChooser")]
public class DialogueChooserScriptableObject : ScriptableObject
{
    [SerializeField] private List<DialogueScriptableObject> dialogues;

    /**
     * A simple function, that chooses a dialogue for an NPC based off of flags given to the function as well as the priority of dialogues.
     * When two available dialogues have the same priority, the first in the list will be chosen.
     */
    public DialogueScriptableObject ChooseDialogue(List<string> flags)
    {
        DialogueScriptableObject result = null;
        foreach (var dialogue in dialogues)
        {
            if (dialogue.RequiredFlags.All(tag2 => flags.Any(tag1 => tag1 == tag2)))
            {
                if (result is null)
                {
                    result = dialogue;
                    continue;
                }

                if (result.Priority < dialogue.Priority)
                {
                    result = dialogue;
                }
            }
        }

        return result;
    }

}