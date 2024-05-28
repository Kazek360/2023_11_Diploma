using System.Linq;
using TMPro;
using UnityEngine;

namespace QuestSystem.UI
{
    public class QuestTracker : MonoBehaviour
    {
        public Quest trackedQuest;
        private TextMeshProUGUI _text;
        
        public void Awake()
        {
            GameConfig.QuestTracker = this;
            _text = GetComponentInChildren<TextMeshProUGUI>();
            DisableText();
        }

        public void TrackQuest(string id)
        {
            if (trackedQuest is not null)
            {
                trackedQuest.isTracked = false; 
            }
            trackedQuest = QuestManager.GetQuestById(id);
            trackedQuest.isTracked = true;
            UpdateText();
            EnableText();
        }
        
        public void EnableText()
        {
            if (trackedQuest is not null)
            {
                _text.gameObject.SetActive(true);
            }
        }
        public void DisableText()
        {
            _text.gameObject.SetActive(false);
        }

        public void UpdateText()
        {
            _text.text = $"{trackedQuest.Info.displayName}\n{trackedQuest.stepsDescriptions.Last()}";
        }
    }
}
