using DialogueSystem.Events;
using DialogueSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueSceneChange : DialogueQuestStep
{
    [SerializeField] private int sceneNumber;
    
    private void OnEnable()
    {
        DialogueEvents.OnDialogueFinished += FinishStep;
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
            SceneManager.LoadScene(sceneNumber);
            Finish();
        } 
    }
}
