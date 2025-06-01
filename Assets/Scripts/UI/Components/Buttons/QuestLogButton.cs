using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class QuestLogButton : MonoBehaviour, ISelectHandler
{
    [NonSerialized] public Button button;
    private TextMeshProUGUI _buttonText;
    private UnityAction _onSelectAction;

    public void Initialize(string displayName, UnityAction selectAction)
    {
        button = GetComponent<Button>();
        _buttonText = GetComponentInChildren<TextMeshProUGUI>();
        _buttonText.text = displayName;
        _onSelectAction = selectAction;
    }
    
    public void OnSelect(BaseEventData eventData)
    {
        _onSelectAction();
    }
}
