using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * A simple NPC script that simply displays a dialogue based on an attached dialogue chooser
 */
public class SuperSimpleTestNPC : MonoBehaviour
{
    
    [SerializeField] private GameObject dialogueWindow;
    [SerializeField] private DialogueChooserScriptableObject dialogueChooser;
    [SerializeField] private List<string> flags;    //TODO: Replace with some real flag system


    public void OnEnable()
    {
        StartCoroutine(StartDialogue());
    }

    private IEnumerator StartDialogue()
    {
        yield return new WaitForSeconds(3f);
        DialogueScriptableObject dialogue = dialogueChooser.ChooseDialogue(flags);
        dialogueWindow.GetComponent<DialogueWindowScript>().StartDialogue(dialogue);
    }
}
