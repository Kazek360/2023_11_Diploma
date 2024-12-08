using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UI.Interfaces;
using UnityEngine;

namespace Quests.UI
{
    public class QuestPopup : MonoBehaviour
    {
        [Header("Components")] 
        [SerializeField] private TextMeshProUGUI popupText;

        private CanvasGroup _canvasGroup;
        private Queue<string> _popupQueue;
        private bool _popupDisplayed;
        
        private void Awake()
        {
            _popupQueue = new Queue<string>();
            _popupDisplayed = false;
            _canvasGroup = GetComponent<CanvasGroup>();
        }
        private void OnEnable()
        {
            QuestEvents.OnQuestStart += AddQuestStartedPopup;
            QuestEvents.OnQuestFinish += AddQuestFinishedPopup;
        }
        private void OnDisable()
        {
            QuestEvents.OnQuestStart -= AddQuestStartedPopup;
            QuestEvents.OnQuestFinish -= AddQuestFinishedPopup;
        }
    
        // private void LateUpdate()
        // {
        //     if (_popupQueue.Count > 0 && !_popupDisplayed)
        //     {
        //         StartCoroutine(ShowPopup());
        //     }
        // }
    
        private void AddQuestStartedPopup(string questID)
        {
            var quest = QuestManager.Instance.GetQuestById(questID);
            _popupQueue.Enqueue($"Zadanie rozpoczęte\n{quest.info.displayName}");
            if (!_popupDisplayed)
            {
                StartCoroutine(ShowPopup());
            }
        }
        
        private void AddQuestFinishedPopup(string questID)
        {
            var quest = QuestManager.Instance.GetQuestById(questID);
            _popupQueue.Enqueue($"Zadanie zakończone\n{quest.info.displayName}");
            if (!_popupDisplayed)
            {
                StartCoroutine(ShowPopup());
            }
        }
        
        private IEnumerator ShowPopup()
        {
            while (UserInterfaceManager.Instance.CurrentStateType != UserInterfaceState.Exploration)
            {
                yield return null;
            }
            popupText.text = _popupQueue.Dequeue();
            Debug.Log(popupText.text);
            _canvasGroup.alpha = 1;
            _popupDisplayed = true;
            yield return new WaitForSeconds(5f);
            _canvasGroup.alpha = 0;
            if (_popupQueue.Count > 0)
            {
                StartCoroutine(ShowPopup());
            }
            else
            {
                _popupDisplayed = false;
            }
        }
    }
}

