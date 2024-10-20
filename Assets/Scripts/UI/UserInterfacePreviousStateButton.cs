using UnityEngine;
using UnityEngine.UI;


namespace UI
{
    [RequireComponent(typeof(Button))]
    public class UserInterfacePreviousStateButton : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                UserInterfaceManager.Instance.ChangeState(UserInterfaceManager.Instance.PreviousState);
            });
        }
    }
}
