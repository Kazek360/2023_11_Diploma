using UnityEngine;

namespace StorySystems.Dialogues.Trigger
{
    public class DialogueTrigger : MonoBehaviour
    {
        [Header("Dialogues info")]
        [SerializeField] private string triggerName;
        [SerializeField] private TextAsset dialogue;
        
        private void OnEnable()
        {
            DialogueEvent.OnDialogueTriggered += TriggerDialogue;
        }
        
        private void OnDisable()
        {
            DialogueEvent.OnDialogueTriggered -= TriggerDialogue;
        }

        private void TriggerDialogue(string providedTriggerName)
        {
            if (triggerName.Equals(providedTriggerName))
            {
                DialogueEvent.InitializeDialogue();
                DialogueEvent.StartDialogue(triggerName, dialogue);
            }
        }
    }
}