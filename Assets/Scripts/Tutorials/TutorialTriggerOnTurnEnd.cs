using Combat;
using UnityEngine;

namespace Tutorials
{
    public class TutorialTriggerOnTurnEnd : MonoBehaviour
    {
        [SerializeField] private TutorialInfo tutorial;
        
        private void OnEnable()
        {
            CombatEvents.OnTurnEnd += Trigger;
        }
        private void OnDisable()
        {
            CombatEvents.OnTurnEnd -= Trigger;
        }
        
        private void Trigger()
        {
            Debug.Log("Elo żelo");
            TutorialManager.Instance.TriggerTutorial(tutorial);
        }
    }
}