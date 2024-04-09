using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueAreaIntoCombatQuestStep : DialogueQuestStep
{
    [SerializeField] private string areaName;
    [SerializeField] private string combatSceneName;
    private GameObject _dialogueArea;
    
    private void OnEnable()
    {
        DialogueEvents.OnDialogueFinished += FinishStep;
        _dialogueArea = GameObject.Find(areaName);
        requiredDialogue = ChooseRequiredDialogue();
        DialogueEvents.UnlockDialogue(requiredDialogue.id);
    }
    private void OnDisable()
    {
        DialogueEvents.OnDialogueFinished-= FinishStep;
    }
    
    private void FinishStep(DialogueSO dialogue)
    {
        if (requiredDialogue.id == dialogue.id)
        {
            Destroy(_dialogueArea);
            Finish();
            SceneManager.LoadScene(combatSceneName);
        } 
    }
}
