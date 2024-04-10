using UnityEngine;

public class NestorDialogueAreaQuestStep : DialogueQuestStep
{
    [SerializeField] private string areaName;
    private GameObject _dialogueArea;
    private GameObject _nestor;
    private void OnEnable()
    {
        DialogueEvents.OnDialogueFinished += FinishStep;
        _dialogueArea = GameObject.Find(areaName);
        _nestor = GameObject.Find("Nestor");
        _nestor.SetActive(false);
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
            _nestor.SetActive(true);
            Destroy(_dialogueArea);
            Finish();
        } 
    }
}
