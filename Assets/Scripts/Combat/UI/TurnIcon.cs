using UnityEngine;
using UnityEngine.UI;

public class TurnIcon : MonoBehaviour
{
    [SerializeField] private GameObject turnIndicator;
    [SerializeField] private Image entityImage;
    [SerializeField] private GameObject defeatedImage;

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

    public void SwitchDefeat(bool isDefeated)
    {
        defeatedImage.SetActive(isDefeated);
    }

    public int EntityOrder => _entityOrder;
}
