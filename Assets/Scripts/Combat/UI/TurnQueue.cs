using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnQueue : MonoBehaviour
{
    [SerializeField] private GameObject turnIconPrefab; 
    private List<TurnIcon> _turnIcons;
    [SerializeField] private float iconWidth;
    // Start is called before the first frame update
    public void Initialize()
    {
        _turnIcons = new List<TurnIcon>();
    }

    public void AddTurnIcon(IHittable entity)
    {
        GameObject tmp = Instantiate(turnIconPrefab,new Vector3(_turnIcons.Count * iconWidth, 0), new Quaternion(), gameObject.transform);
        TurnIcon iconScript = tmp.GetComponent<TurnIcon>();
        iconScript.SetEntity(_turnIcons.Count ,entity);
        _turnIcons.Add(iconScript);
    }
}
