
using System.Collections;
using UnityEngine;

public class DialogueWithDelayedMovement : DialogueQuestStep
{
    [SerializeField] private FlagSO flagDelay;
    private GameObject _raceArea;
    private GameObject _cliffArea;
    
    private void OnEnable()
    {
        DialogueEvents.OnDialogueFinished += StartFinishingStep;
        requiredDialogue = ChooseRequiredDialogue();
        DialogueEvents.UnlockDialogue(requiredDialogue.id);
        _raceArea = GameObject.Find("RaceArea");
        _raceArea.SetActive(false);
        _cliffArea = GameObject.Find("CliffArea");
        _cliffArea.SetActive(false);
    }
    private void OnDisable()
    {
        DialogueEvents.OnDialogueFinished-= StartFinishingStep;
    }
    
    private void StartFinishingStep(DialogueSO dialogue)
    {
        StartCoroutine(FinishStep(dialogue));
    }
    
    private IEnumerator FinishStep(DialogueSO dialogue)
    {
        if (requiredDialogue.id == dialogue.id)
        {
            if (FlagManager.GetMarkerById(flagDelay.id)._isTriggered)
            {
                yield return new WaitForSeconds(1f);
            }
            _raceArea.SetActive(true);
            _cliffArea.SetActive(true);
            PathEvents.StartMovement();
            Finish();
        } 
    }
}