using TMPro;
using UnityEngine;
using UnityEngine.UI;

/**
 * Controller for a container of entity in combat summary UI
 */
public class EntityDisplaySummaryScript : MonoBehaviour
{
    [SerializeField] private Image Image;
    [SerializeField] private TextMeshProUGUI Nameplate;
    [SerializeField] private TextMeshProUGUI HPDisplay;
    
    /**
     * Set info for display of an entity
     *
     * <param name="hittable">An entity displayed</param>
     */
    public void SetValues(IHittable hittable)
    {
        Image.sprite = hittable.GetImage();
        Nameplate.text = hittable.GetEntityName();
        HPDisplay.text = hittable.GetCurrentHealth() + "/" + hittable.GetMaxHealth() + " HP";
    }
}
