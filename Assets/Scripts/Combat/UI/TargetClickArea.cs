using UnityEngine;
using UnityEngine.Serialization;

/**
 * Controller for Target click area UI, which allows player to select a target for an ability
 */
public class TargetClickArea : MonoBehaviour
{
    private IHittable _parent;
    private CombatUI _combatUI;
    [FormerlySerializedAs("indicator")] [SerializeField] private SpriteRenderer Indicator;
    
    public void EnableClick(CombatUI combatUI, IHittable parent)
    {
        gameObject.SetActive(true);
        _combatUI = combatUI;
        _parent = parent;
    }
    
    public void DisableClick()
    {
        gameObject.SetActive(false);
    }
    private void OnMouseDown()
    {
        _combatUI.SetTarget(_parent);
        //Indicator.color = Color.green;
    }

    public void Deselect()
    {
        Indicator.color = Color.white;
    }
}
