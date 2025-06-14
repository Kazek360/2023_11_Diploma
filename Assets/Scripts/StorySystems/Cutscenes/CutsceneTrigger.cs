using Cutscenes;
using Interactions;
using UnityEngine;


namespace StorySystems.Cutscenes
{
    public class CutsceneTrigger : MonoBehaviour, IInteractable
    {
        [SerializeField] private CutsceneSO cutscene;
        [SerializeField] private GameObject interactionKey;
    
        public void TriggerInteraction()
        {
            CutsceneEvents.StartCutscene(cutscene);
        }

        public Vector3 GetCoordinates()
        {
            return transform.position;
        }

        public void ShowIndicator()
        {
            interactionKey.SetActive(true);  
        }

        public void HideIndicator()
        {
            interactionKey.SetActive(false);  
        }
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("InteractionArea"))
            {
                InteractionEvents.EnterInteractionArea(this);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("InteractionArea"))
            {
                InteractionEvents.ExitInteractionArea(this);
            }
        }
    }
}
