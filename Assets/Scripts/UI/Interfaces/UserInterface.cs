using UnityEngine;

namespace UI.Interfaces
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class UserInterface : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;

        public CanvasGroup CanvasGroup => _canvasGroup;

        public abstract UserInterfaceState State { get; }
        
        protected virtual void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }
        
        public virtual void OnEnter()
        {
            CanvasGroup.interactable = true; 
            CanvasGroup.alpha = 1;
            CanvasGroup.blocksRaycasts = true;
        }
        public virtual void OnExit()
        {
            CanvasGroup.interactable = false; 
            CanvasGroup.alpha = 0;
            CanvasGroup.blocksRaycasts = false;
        }
    }
}
