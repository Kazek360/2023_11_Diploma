using UnityEngine;

namespace StorySystems.Dialogues.Trigger
{
    //This class triggers dialogue by entering area
    public class AreaDialogueEntity : DialogueEntity
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            AreaEvent.OnCorrectAreaEnter += TriggerDialogue;
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
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

