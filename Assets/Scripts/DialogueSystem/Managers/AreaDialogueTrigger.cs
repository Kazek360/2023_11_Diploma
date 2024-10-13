using UnityEngine;
using DialogueSystem.Events;

namespace DialogueSystem.Managers
{
    //This class triggers dialogue by entering area
    public class AreaDialogueTrigger : MonoBehaviour
    {
        [Header("Name")]
        [SerializeField] private string triggerName;

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
            DialogueEvents.TriggerDialogue(triggerName);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            AreaEvent.EnterArea(gameObject.name, other.name);
        }
    }
}

