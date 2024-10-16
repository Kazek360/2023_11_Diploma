using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ButtonSoundEffect : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private AudioClip buttonSoundClip;
   
    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundFxManager.instance.PlaySoundFxClip(buttonSoundClip, transform, 1f);
    }
}