using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnQueue : MonoBehaviour
{
    [SerializeField] private GameObject turnIconPrefab; 
    private List<TurnIcon> _turnIcons;
    [SerializeField] private float iconWidth;
    public void Initialize()
    {
        _turnIcons = new List<TurnIcon>();
    }

    public void AddTurnIcon(IHittable entity)
    {
        GameObject tmp = Instantiate(turnIconPrefab,new Vector3( _turnIcons.Count * iconWidth, 0), new Quaternion(), gameObject.transform);
        tmp.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(_turnIcons.Count * iconWidth, 0);
        TurnIcon iconScript = tmp.GetComponent<TurnIcon>();
        iconScript.SetEntity(_turnIcons.Count ,entity);
        _turnIcons.Add(iconScript);
    }

    public void SetActiveEntity(int n)
    {
        foreach (var turnIcon in _turnIcons)
        {
            if (turnIcon.EntityOrder == n)
            {
                turnIcon.SwitchTurnIndicator(true);
            }
            else
            {
                turnIcon.SwitchTurnIndicator(false);
            }
        }
    }

    public void DisableEntity(int n)
    {
        foreach (var turnIcon in _turnIcons)
        {
            if (turnIcon.EntityOrder == n)
            {
                turnIcon.SwitchDefeat(true);
            }
        }
    }
}
