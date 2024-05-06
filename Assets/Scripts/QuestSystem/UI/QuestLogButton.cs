using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestLogButton : MonoBehaviour, ISelectHandler
{
    public Button button;
    private TextMeshProUGUI _buttonText;

    public void Initialize(string displayName, UnityAction selectAction)
    {
        button = GetComponent<Button>();
        _buttonText = GetComponentInChildren<TextMeshProUGUI>();
        _buttonText.text = displayName;
        _onSelectAction = selectAction;
    }

    private UnityAction _onSelectAction;
    public void OnSelect(BaseEventData eventData)
    {
        _onSelectAction();
    }
}
