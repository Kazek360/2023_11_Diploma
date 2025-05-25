using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI Header;
    [SerializeField] private TextMeshProUGUI Desc;
    [SerializeField] private CanvasGroup _canvasGroup;
    private float width, height;
    private Coroutine _visibilityCoroutine;


    private float _visibilityChangeRate = 0.05f;
    private float _visibilityChangeTime = 0.5f;

    private bool _mouseIsInside;
    
    public void Setup(string header,  string desc)
    {
        Header.text = header;
        Desc.text = desc;
        gameObject.transform.SetParent(GameConfig.canvas.gameObject.transform);
        width = GetComponent<RectTransform>().rect.width;
        height = GetComponent<RectTransform>().rect.height;
        SetupPos();
        _visibilityCoroutine = StartCoroutine(visibilityCoroutine());
    }

    private IEnumerator visibilityCoroutine()
    {
        yield return StartCoroutine(WaitForRealSeconds(0.3f));
        for (int i = 0; i < 1f/_visibilityChangeRate; i++)
        {
            _canvasGroup.alpha += _visibilityChangeRate;
            yield return StartCoroutine(WaitForRealSeconds(_visibilityChangeTime * _visibilityChangeRate));
            //yield return new WaitForSeconds(_visibilityChangeTime * _visibilityChangeRate);
        }
    }
    
    IEnumerator WaitForRealSeconds (float seconds) {
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - startTime < seconds) {
            yield return null;
        }
    }

    private void SetupPos()
    {
        float x, y;
        if (Input.mousePosition.y < Screen.height - height)
        {
            y = Input.mousePosition.y + height / 2;
        }
        else
        {
            y = Input.mousePosition.y - height / 2;
        }

        if (Input.mousePosition.x < Screen.width - width)
        {
            x = Input.mousePosition.x + width / 2;
        }
        else
        {
            x = Input.mousePosition.x - width / 2;
        }
        
        gameObject.transform.position = new Vector2(x, y);
    }

    public void Delete()
    {
        // if (!_mouseIsInside)
        // {
        //     StopCoroutine(_visibilityCoroutine);
        //     Destroy(gameObject);
        // }
        StartCoroutine(StartDeleting());
    }
    
    private IEnumerator StartDeleting()
    {
        yield return StartCoroutine(WaitForRealSeconds(0.3f));
        if (!_mouseIsInside)
        {
            StopCoroutine(_visibilityCoroutine);
            Destroy(gameObject);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _mouseIsInside = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _mouseIsInside = false;
        Delete();
    }
}
