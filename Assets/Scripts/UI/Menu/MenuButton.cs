using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent (typeof(Button))]
public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
 
    private TextMeshProUGUI _text;
    private Color _baseColor;
    private Button _button;
    private bool _interactableDelay;

    private void Awake()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _baseColor = _text.color;
        _button = GetComponent<Button>();
        _interactableDelay = _button.interactable;
    }

    private void Update()
    {
        if (_button.interactable != _interactableDelay){
            if (_button.interactable) {
                _text.color = _baseColor * _button.colors.normalColor * _button.colors.colorMultiplier;
            } else {
                _text.color = _baseColor * _button.colors.disabledColor * _button.colors.colorMultiplier;
            }
        }
        _interactableDelay = _button.interactable;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_button.interactable) {
            _text.color = _baseColor * _button.colors.highlightedColor * _button.colors.colorMultiplier;
        } else {
            _text.color = _baseColor * _button.colors.disabledColor * _button.colors.colorMultiplier;
        }
    }

    public void OnPointerDown (PointerEventData eventData)
    {
        if (_button.interactable) {
            _text.color = _baseColor * _button.colors.pressedColor * _button.colors.colorMultiplier;
        } else {
            _text.color = _baseColor * _button.colors.disabledColor * _button.colors.colorMultiplier;
        }
    }

    public void OnPointerUp (PointerEventData eventData)
    {
        if (_button.interactable) {
            _text.color = _baseColor * _button.colors.highlightedColor * _button.colors.colorMultiplier;
        } else {
            _text.color = _baseColor * _button.colors.disabledColor * _button.colors.colorMultiplier;
        }
    }

    public void OnPointerExit (PointerEventData eventData)
    {
        if (_button.interactable) {
            _text.color = _baseColor * _button.colors.normalColor * _button.colors.colorMultiplier;
        } else {
            _text.color = _baseColor * _button.colors.disabledColor * _button.colors.colorMultiplier;
        }
    }

}