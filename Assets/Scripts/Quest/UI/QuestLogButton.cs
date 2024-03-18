using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestLogButton : MonoBehaviour, ISelectHandler
{
    public Button button;
    private TextMeshProUGUI buttonText;
    private UnityAction onSelectAction;

    public void Initialize(string displayName, UnityAction selectAction)
    {
        button = GetComponent<Button>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = displayName;
        onSelectAction = selectAction;
    }
    
    public void OnSelect(BaseEventData eventData)
    {
        onSelectAction();
    }
}
