using UnityEngine;
using UnityEngine.SceneManagement;
using Dialogue;

public class DialogueAreaSceneChange : DialogueAreaQuestStep
{
    [SerializeField] private int sceneNumber;
    
    protected override void FinishStep(DialogueInfo dialogue)
    {
        if (dialoguesInfo.name == dialogue.name)
        {
            Finish();
            SceneManager.LoadScene(sceneNumber);
        } 
    }
}
