using UnityEngine;
using Dialogues;
using Location;

public class AreaDialogueAreaSceneChange : AreaDialogueStep
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
