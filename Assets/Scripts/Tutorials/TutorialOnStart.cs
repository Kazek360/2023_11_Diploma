using UI;
using UnityEngine;

namespace Tutorials
{
    public class TutorialOnStart : MonoBehaviour
    {
        [SerializeField] private TutorialInfo tutorial;
        private void Start()
        {
            TutorialUI.InvokeOnTutorialTrigger(tutorial);
            UserInterfaceManager.Instance.ChangeStateWithoutVisibilityChange(UserInterfaceState.Tutorial);
        }
    }
}
