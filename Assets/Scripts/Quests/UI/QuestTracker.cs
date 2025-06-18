using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Quests.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class QuestTracker : MonoBehaviour
    {
        [SerializeField] private RectTransform panelTransform; 
        [SerializeField] private RectTransform textContainerTransform; 
        [SerializeField] private TextMeshProUGUI title; 
        [SerializeField] private TextMeshProUGUI description;  
        private CanvasGroup _canvasGroup;
        public CanvasGroup CanvasGroup => _canvasGroup;

        public void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }
        
        public void OnEnable()
        {
            QuestEvents.OnQuestTracking += SetTrackQuestInfo;
            QuestEvents.OnQuestTrackingStop += SetTrackQuestInfo;
        }
        
        public void OnDisable()
        {
            QuestEvents.OnQuestTracking -= SetTrackQuestInfo;
            QuestEvents.OnQuestTrackingStop -= SetTrackQuestInfo;
        }
        
        private void SetTrackQuestInfo()
        {
            if (QuestManager.Instance.TrackedQuest is null)
            {
                CanvasGroup.alpha = 0;
            }
            else
            {
                if (QuestManager.Instance.TrackedQuest.stepsDescriptions.Count == 0)
                {
                    CanvasGroup.alpha = 0;
                    return;
                }
                title.text = $"{QuestManager.Instance.TrackedQuest.info.displayName}";
                description.text = $"{QuestManager.Instance.TrackedQuest.stepsDescriptions.Last()}";
                LayoutRebuilder.ForceRebuildLayoutImmediate(textContainerTransform);
                panelTransform.sizeDelta  = new Vector2(panelTransform.sizeDelta.x, textContainerTransform.rect.height);
                StartCoroutine(ShowTracker());
            }
        }


        private IEnumerator ShowTracker()
        {
            yield return new WaitForSeconds(0.125f);
            CanvasGroup.alpha = 1;
        }
    }
}
