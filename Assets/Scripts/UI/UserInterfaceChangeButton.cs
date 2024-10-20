using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class UserInterfaceChangeButton : MonoBehaviour
    {
        [SerializeField] private UserInterfaceState stateToChange;
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                UserInterfaceManager.Instance.ChangeState(stateToChange);
            });
        }
    }
}
