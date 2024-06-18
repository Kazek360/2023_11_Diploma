using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

/**
 * Largely redundant controller for old target cards, superseded by TargetClickArea
 */
public class TargetCard : MonoBehaviour
{
    private IHittable _target;
    [FormerlySerializedAs("targetNameField")] [SerializeField] private GameObject TargetNameField;
    [FormerlySerializedAs("targetImage")] [SerializeField] private GameObject TargetImage;

    // Start is called before the first frame update
    public void SetTarget(IHittable script)
    {
        _target = script;
        TargetNameField.GetComponent<TextMeshProUGUI>().text = _target.GetEntityName();
        TargetImage.GetComponent<Image>().sprite = _target.GetImage();
    }

    public IHittable GetTargetScript()
    {
        return _target;
    }
}
