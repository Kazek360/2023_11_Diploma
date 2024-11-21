using UnityEngine;

namespace Dialogue.Trigger
{
    //This class triggers dialogue by entering area
    public class AreaDialogueEntity : DialogueEntity
    {
        private void OnEnable()
        {
            AreaEvent.OnCorrectAreaEnter += TriggerDialogue;
        }
        
        private void OnDisable()
        {
            AreaEvent.OnCorrectAreaEnter -= TriggerDialogue;
        }
        
        private void TriggerDialogue(string areaName)
        {
            if (!areaName.Equals(gameObject.name))
            {
                return;
            }
            StartDialogue();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            AreaEvent.EnterArea(gameObject.name, other.name);
        }
    }
}

