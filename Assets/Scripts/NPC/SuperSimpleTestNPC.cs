using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperSimpleTestNPC : MonoBehaviour
{
    
    [SerializeField] private GameObject dialogueWindow;
    [SerializeField] private DialogueScriptableObject dialogue;


    public void OnEnable()
    {
        StartCoroutine(StartDialogue());
    }

    private IEnumerator StartDialogue()
    {
        yield return new WaitForSeconds(3f);
        dialogueWindow.GetComponent<DialogueWindowScript>().StartDialogue(dialogue);
    }
}
