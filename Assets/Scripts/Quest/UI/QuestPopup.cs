using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestPopup : MonoBehaviour
{
    [Header("Components")] 
    [SerializeField] private TextMeshProUGUI popupText;
    // serializefiled tez do UI Image
    [SerializeField] private Image popupImage;
    private void OnEnable()
    {
        QuestEvents.OnQuestStateChange += EnablePopup;
    }
    private void OnDisable()
    {
        QuestEvents.OnQuestStateChange -= EnablePopup;
    }

    private void EnablePopup(Quest quest)
    {
        StartCoroutine(ShowPopup(quest));
    }
    
    private IEnumerator ShowPopup(Quest quest)
    {
        if(quest.State == QuestState.InProgress || quest.State == QuestState.Finished)
        {
            if (quest.State == QuestState.InProgress)
            {
                popupText.text = $"Zadanie rozpoczęte\n{quest.Info.displayName}";
            }
            else if (quest.State == QuestState.Finished)
            {
                popupText.text = $"Zadanie zakończone\n{quest.Info.displayName}";
            }
            popupText.gameObject.SetActive(true);
            popupImage.gameObject.SetActive(true);
            yield return new WaitForSeconds(2f);
            popupText.gameObject.SetActive(false);
            popupImage.gameObject.SetActive(false);
        }
    }
    
    
}
