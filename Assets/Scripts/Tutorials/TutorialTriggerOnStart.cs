using System.Collections;
using UI;
using UnityEngine;

namespace Tutorials
{
    public class TutorialTriggerOnStart : MonoBehaviour
    {
        [SerializeField] private TutorialInfo tutorial;
        private IEnumerator Start()
        {
            while (!UserInterfaceManager.isInitialized)
            {
                yield return null;
            }
            TutorialManager.Instance.TriggerTutorial(tutorial);
        }
    }
}
