using UnityEngine;
using UnityEngine.SceneManagement;
using DialogueSystem.Events;

public class DialogueAreaSceneChange : DialogueAreaQuestStep
{
    [SerializeField] private int sceneNumber;
    
    private void OnEnable()
    {
        AreaEvent.OnAreaEnter += AreaEntered;
        DialogueEvents.OnDialogueFinished += FinishStep;
        area = GameObject.Find(areaToEnterName);
        DialogueEvents.UnlockDialogue(dialoguesInfo.name);
    }
    private void OnDisable()
    {
        AreaEvent.OnAreaEnter -= AreaEntered;
        DialogueEvents.OnDialogueFinished-= FinishStep;
    }
    
    private void FinishStep(DialogueSO dialogue)
    {
        if (dialoguesInfo.name == dialogue.name)
        {
            Destroy(area);
            SceneManager.LoadScene(sceneNumber);
            Finish();
        } 
    }
}
