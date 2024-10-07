using UnityEngine;
using UnityEngine.UI;

public class TurnIcon : MonoBehaviour
{
    [SerializeField] private GameObject turnIndicator;
    [SerializeField] private Image entityImage;

    private int _entityOrder;

    public void SetEntity(int entityOrder, IHittable entity)
    {
        _entityOrder = entityOrder;
        entityImage.sprite = entity.GetImage();
    }

    public void SwitchTurnIndicator(bool isEnabled)
    {
        turnIndicator.SetActive(isEnabled);
    }
}
