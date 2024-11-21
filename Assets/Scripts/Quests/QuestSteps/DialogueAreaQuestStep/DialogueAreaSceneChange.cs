using UnityEngine;
using Dialogue;
using Location;

public class DialogueAreaSceneChange : DialogueAreaQuestStep
{
    [SerializeField] private string sceneName;
    
    protected override void FinishStep(DialogueInfo dialogue)
    {
        if (dialoguesInfo.name == dialogue.name)
        {
            Finish();
            SceneLoader.InvokeOnSceneChanged(sceneName);
        } 
    }
}
