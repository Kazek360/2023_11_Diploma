using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FitTextToParent : MonoBehaviour
{
    public Text textComponent;
    public RectTransform parentRectTransform;

    void Start()
    {
        if (textComponent == null || parentRectTransform == null)
        {
            Debug.LogError("TextComponent or ParentRectTransform is not set.");
            return;
        }

        AdjustTextSize();
    }

    void AdjustTextSize()
    {
        // Get the RectTransform of the text
        RectTransform textRectTransform = textComponent.GetComponent<RectTransform>();

        // Set the text RectTransform to match the parent size
        textRectTransform.sizeDelta = new Vector2(parentRectTransform.rect.width, parentRectTransform.rect.height);

        // Adjust the font size to fit within the parent bounds
        StartCoroutine(AdjustFontSize());
    }

    System.Collections.IEnumerator AdjustFontSize()
    {
        yield return null; // Wait for one frame to get correct size
        int minFontSize = 1;
        int maxFontSize = 100;
        textComponent.resizeTextForBestFit = true;
        textComponent.resizeTextMinSize = minFontSize;
        textComponent.resizeTextMaxSize = maxFontSize;
    }
}
