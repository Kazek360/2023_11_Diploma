using UnityEngine;
using UnityEngine.SceneManagement;
using DialogueSystem.Events;

public class DialogueAreaIntoCombatQuestStep : DialogueQuestStep
{
    [SerializeField] private string areaName;
    [SerializeField] private string combatSceneName;
    private GameObject _dialogueArea;
    
    private void OnEnable()
    {
        DialogueEvents.OnDialogueFinished += FinishStep;
        _dialogueArea = GameObject.Find(areaName);
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
            Destroy(_dialogueArea);
            SceneManager.LoadScene(2);
            Finish();
        } 
    }
}
