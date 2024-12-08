using UnityEngine;

namespace Tutorials
{
    public class TutorialTriggerOnStart : MonoBehaviour
    {
        [SerializeField] private TutorialInfo tutorial;
        private void Start()
        {
            TutorialManager.Instance.TriggerTutorial(tutorial);
        }
    }
}
