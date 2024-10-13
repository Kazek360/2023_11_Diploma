using Unity.VisualScripting;
using UnityEngine;

public class TooltipObject : MonoBehaviour
{
    private ITooltippable _target;
    private bool _triggered;
    private TooltipUI _tooltip;
    [SerializeField] private GameObject TooltipPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        _target = GetComponentInParent<ITooltippable>();
    }

    // Update is called once per frame
    void OnMouseOver()
    {
        if (_triggered)
        {
            return;
        }

        _tooltip = Instantiate(TooltipPrefab).GetComponent<TooltipUI>();
        if (_target is not null)
        {
            _tooltip.Setup(_target.GetTooltipHeader(), _target.GetTooltipDesc());
        }

        _triggered = true;
    }

    private void OnMouseExit()
    {
        _triggered = false;
        _tooltip.Delete();
    }

    public void OnDisable()
    {
        if (_tooltip is null || _tooltip.IsDestroyed())
        {
            return;
        }
        if (_tooltip.isActiveAndEnabled)
        {
            Destroy(_tooltip.gameObject);
        }
    }
}
