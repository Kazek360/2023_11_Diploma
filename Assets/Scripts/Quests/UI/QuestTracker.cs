using System.Linq;
using TMPro;
using UnityEngine;

namespace Quests.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class QuestTracker : MonoBehaviour
    {
        private TextMeshProUGUI _text;
        private CanvasGroup _canvasGroup;
        public CanvasGroup CanvasGroup => _canvasGroup;

        public void Awake()
        {
            _text = GetComponentInChildren<TextMeshProUGUI>();
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
                _text.text = $"{QuestManager.Instance.TrackedQuest.info.displayName}" +
                             $"\n{QuestManager.Instance.TrackedQuest.stepsDescriptions.Last()}";
            }
        }
    }
}
