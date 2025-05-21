using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;

namespace Quests.UI
{
    public class QuestPopup : MonoBehaviour
    {
        [Header("Components")] 
        [SerializeField] private TextMeshProUGUI popupText;
        
        [Header("Fade in/out")] 
        [SerializeField] private int speed;
        
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
            PlayerEvents.OnLevelUp += AddLevelUpPopup;
        }
        private void OnDisable()
        {
            QuestEvents.OnQuestStart -= AddQuestStartedPopup;
            QuestEvents.OnQuestFinish -= AddQuestFinishedPopup;
            PlayerEvents.OnLevelUp -= AddLevelUpPopup;
        }
    
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
        
        private void AddLevelUpPopup()
        {
            _popupQueue.Enqueue($"Następny poziom");
            if (!_popupDisplayed)
            {
                StartCoroutine(ShowPopup());
            }
        }
        
        private IEnumerator ShowPopup()
        {
            while (!UserInterfaceManager.isInitialized)
            {
                yield return null;
            }
            while (UserInterfaceManager.Instance.CurrentStateType != UserInterfaceState.Exploration)
            {
                yield return null;
            }
            popupText.text = _popupQueue.Dequeue();
            yield return FadeIn();
            _popupDisplayed = true;
            yield return new WaitForSeconds(5f);
            yield return FadeOut();
            if (_popupQueue.Count > 0)
            {
                StartCoroutine(ShowPopup());
            }
            else
            {
                _popupDisplayed = false;
            }
        }
        
        private IEnumerator FadeIn()
        {
            while (_canvasGroup.alpha < 1)
            {
                _canvasGroup.alpha += speed * Time.deltaTime;
                yield return null;
            }
        }
        
        private IEnumerator FadeOut()
        {
            while (_canvasGroup.alpha > 0)
            {
                _canvasGroup.alpha -= speed * Time.deltaTime;
                yield return null;
            }
        }
    }
}

