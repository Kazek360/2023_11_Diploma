using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Quests.UI
{
    public class QuestPopup : MonoBehaviour
    {
        [Header("Components")] 
        [SerializeField] private TextMeshProUGUI popupText;
        [SerializeField] private Image popupImage;
    
        private Queue<string> _popupQueue;
        private bool _popupDisplayed;
        
        private void Awake()
        {
            popupText.gameObject.SetActive(false);
            popupImage.gameObject.SetActive(false);
            _popupQueue = new Queue<string>();
            _popupDisplayed = false;
        }
        private void OnEnable()
        {
            QuestEvents.OnQuestStart += AddStartQuestPopup;
            QuestEvents.OnQuestFinish += AddFinishQuestPopup;
        }
        private void OnDisable()
        {
            QuestEvents.OnQuestStart -= AddStartQuestPopup;
            QuestEvents.OnQuestFinish -= AddFinishQuestPopup;
        }
    
        private void LateUpdate()
        {
            if (_popupQueue.Count > 0 && !_popupDisplayed)
            {
                StartCoroutine(ShowPopup());
            }
        }
    
        private void AddStartQuestPopup(string questID)
        {
            var quest = QuestManager.Instance.GetQuestById(questID);
            _popupQueue.Enqueue($"Zadanie rozpoczęte\n{quest.info.displayName}");
        }
        
        private void AddFinishQuestPopup(string questID)
        {
            var quest = QuestManager.Instance.GetQuestById(questID);
            _popupQueue.Enqueue($"Zadanie zakończone\n{quest.info.displayName}");
        }
        
        private IEnumerator ShowPopup()
        {
            _popupDisplayed = true;
            popupText.text = _popupQueue.Dequeue();
            popupText.gameObject.SetActive(true);
            popupImage.gameObject.SetActive(true);
            yield return new WaitForSeconds(3f);
            popupText.gameObject.SetActive(false);
            popupImage.gameObject.SetActive(false);
            _popupDisplayed = false;
        }
    }
}

