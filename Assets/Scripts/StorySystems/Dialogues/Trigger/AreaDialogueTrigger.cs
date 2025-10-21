using UnityEngine;

namespace StorySystems.Dialogues.Trigger
{
    //This class triggers dialogue by entering area
    public class AreaDialogueTrigger : MonoBehaviour
    {
        [Header("Dialogues info")]
        [SerializeField] private string triggerName;
        [SerializeField] private TextAsset dialogue;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            DialogueEvent.EnterDialogueArea(triggerName, dialogue, other.name);
        }
    }
}

