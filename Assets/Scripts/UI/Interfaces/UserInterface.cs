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

        protected virtual void OnEnable()
        {
            UserInterfaceManager.onInterfacePinged += SubscribeToManager;
        }
        protected virtual void OnDisable()
        {
            UserInterfaceManager.onInterfacePinged -= SubscribeToManager;
        }

        private void SubscribeToManager()
        {
            UserInterfaceManager.SubscribeToManager(State, this);
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
