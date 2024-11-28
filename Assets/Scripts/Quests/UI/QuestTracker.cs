using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Quests.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class QuestTracker : MonoBehaviour
    {
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
                title.text = $"{QuestManager.Instance.TrackedQuest.info.displayName}";
                description.text = $"{QuestManager.Instance.TrackedQuest.stepsDescriptions.Last()}";
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
