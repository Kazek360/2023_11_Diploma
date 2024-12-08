using UnityEngine;

namespace Tutorials
{
    public class TutorialTriggerOnLevelUp : MonoBehaviour
    {
        [SerializeField] private TutorialInfo tutorial;
        private void OnEnable()
        {
            PlayerEvents.OnLevelUp += Trigger;
        }
        private void OnDisable()
        {
            PlayerEvents.OnLevelUp -= Trigger;
        }

        private void Trigger()
        {
            TutorialManager.Instance.TriggerTutorial(tutorial);
        }
    }
}