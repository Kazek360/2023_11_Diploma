using DialogueSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueSceneChange : DialogueQuestStep
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
