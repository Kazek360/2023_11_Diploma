using System.Collections;
using QuestSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestPopup : MonoBehaviour
{
    [Header("Components")] 
    [SerializeField] private TextMeshProUGUI popupText;
    [SerializeField] private Image popupImage;
    private void OnEnable()
    {
        QuestEvents.OnQuestStart += EnableQuestStartPopup;
        QuestEvents.OnQuestFinish += EnableQuestFinishPopup;
    }
    private void OnDisable()
    {
        QuestEvents.OnQuestStart -= EnableQuestStartPopup;
        QuestEvents.OnQuestFinish -= EnableQuestFinishPopup;
    }

    private void EnableQuestStartPopup(string questID)
    {
        Quest quest = QuestManager.GetQuestById(questID);
        StartCoroutine(ShowQuestStartPopup(quest));
    }
    private IEnumerator ShowQuestStartPopup(Quest quest)
    {
        popupText.text = $"Zadanie rozpoczęte\n{quest.Info.displayName}";
        popupText.gameObject.SetActive(true);
        popupImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        popupText.gameObject.SetActive(false);
        popupImage.gameObject.SetActive(false);
    }
    
    private void EnableQuestFinishPopup(string questID)
    {
        Quest quest = QuestManager.GetQuestById(questID);
        StartCoroutine(ShowQuestFinishPopup(quest));
    }
    private IEnumerator ShowQuestFinishPopup(Quest quest)
    {
        popupText.text = $"Zadanie zakończone\n{quest.Info.displayName}";
        popupText.gameObject.SetActive(true);
        popupImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        popupText.gameObject.SetActive(false);
        popupImage.gameObject.SetActive(false);
    }
}
