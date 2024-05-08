using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DialogueSystem.Events;

public class DialogueAreaSceneChange : DialogueAreaQuestStep
{
    [SerializeField] private int sceneNumber;
    
    private void OnEnable()
    {
        AreaEvent.OnAreaEnter += AreaEntered;
        DialogueEvents.OnDialogueFinished += StartFinishingStep;
        area = GameObject.Find(areaToEnterName);
        DialogueEvents.UnlockDialogue(dialoguesInfo.name);
    }
    private void OnDisable()
    {
        AreaEvent.OnAreaEnter -= AreaEntered;
        DialogueEvents.OnDialogueFinished-= StartFinishingStep;
    }
    
    private void StartFinishingStep(DialogueSO dialogue)
    {
        StartCoroutine(FinishStep(dialogue));
    }
    
    private IEnumerator FinishStep(DialogueSO dialogue)
    {
        if (dialoguesInfo.name == dialogue.name)
        {
            QuestEvents.FinishQuest(dialogue.relatedQuest.id);
            Destroy(area);
            Destroy(gameObject); 
            SceneManager.LoadScene(sceneNumber);
            yield return new WaitForSeconds(1f);
        } 
    }
}
