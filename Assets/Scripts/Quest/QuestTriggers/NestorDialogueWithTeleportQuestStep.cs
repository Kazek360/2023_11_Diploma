using UnityEngine;

public class NestorDialogueWithTeleportQuestStep : DialogueQuestStep
{
    private GameObject _motherToTeleport;
    private GameObject _witToTeleport;
    private GameObject _placeToTeleportMother;
    private GameObject _placeToTeleportWit;
    private GameObject _motherArea;
    private GameObject _nestor;
    
    private void OnEnable()
    {
        DialogueEvents.OnDialogueFinished += FinishStep;
        requiredDialogue = ChooseRequiredDialogue();

        _motherToTeleport = GameObject.Find("Mother");
        _placeToTeleportMother = GameObject.Find("MotherTeleportPlace");
        _witToTeleport = GameObject.Find("Wit");
        _placeToTeleportWit = GameObject.Find("WitTeleportPlace");
        _nestor = GameObject.Find("Nestor");
        _motherArea = GameObject.Find("MotherArea");
        _motherArea.SetActive(false);
        _nestor.SetActive(false);
        
        DialogueEvents.UnlockDialogue(requiredDialogue.id);
    }
    private void OnDisable()
    {
        DialogueEvents.OnDialogueFinished-= FinishStep;
    }
    
    private void FinishStep(DialogueSO dialogue)
    {
        if (requiredDialogue.id == dialogue.id)
        {
            _nestor.SetActive(true);
            _motherArea.SetActive(true);
            _motherToTeleport.transform.position = _placeToTeleportMother.transform.position;
            _witToTeleport.transform.position = _placeToTeleportWit.transform.position;
            Finish();
        } 
    }
}
