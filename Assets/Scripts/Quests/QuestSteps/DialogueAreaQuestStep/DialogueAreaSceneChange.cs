using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DialogueSystem.Events;
using DialogueSystem.ScriptableObjects;

public class DialogueAreaSceneChange : DialogueAreaQuestStep
{
    [SerializeField] private int sceneNumber;
    
    protected override void FinishStep(DialogueSO dialogue)
    {
        if (dialoguesInfo.name == dialogue.name)
        {
            Finish();
            SceneManager.LoadScene(sceneNumber);
        } 
    }
}
