using System.Linq;
using TMPro;
using UnityEngine;

namespace Quests.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class QuestTracker : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _unvisibleText; 
        [SerializeField] private TextMeshProUGUI _visibleText;  
        private CanvasGroup _canvasGroup;
        public CanvasGroup CanvasGroup => _canvasGroup;

        public void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }
        
        public void OnEnable()
        {
            QuestEvents.OnQuestTracking += SetTrackQuestInfo;
        }
        
        public void OnDisable()
        {
            QuestEvents.OnQuestTracking -= SetTrackQuestInfo;
        }
        
        private void SetTrackQuestInfo()
        {
            if (QuestManager.Instance.TrackedQuest is null)
            {
                CanvasGroup.alpha = 0;
            }
            else
            {
                CanvasGroup.alpha = 1;
                string questInfo = $"{QuestManager.Instance.TrackedQuest.info.displayName}" +
                             $"\n{QuestManager.Instance.TrackedQuest.stepsDescriptions.Last()}";
                
                _unvisibleText.text = questInfo;
                _visibleText.text = questInfo;
            }
        }
    }
}
