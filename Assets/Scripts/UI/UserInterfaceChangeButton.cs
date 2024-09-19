using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class UserInterfaceChangeButton : MonoBehaviour
    {
        [SerializeField] private GameState stateToChange;
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                UserInterfaceManager.Instance.ChangeState(stateToChange);
            });
        }
    }
}
