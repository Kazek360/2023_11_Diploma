using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class FixedScrollbar : MonoBehaviour
{
    private Scrollbar _scrollSlider;

    private void Awake()
    {
        _scrollSlider = GetComponentInChildren<Scrollbar>();
        _scrollSlider.size = 0;
    }

    public void SetFixedSize()
    {
        _scrollSlider.size = 0;
    }
}
