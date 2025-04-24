using Dialogues;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueSceneChange : DialogueStep
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
