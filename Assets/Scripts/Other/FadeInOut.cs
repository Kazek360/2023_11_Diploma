using System;
using System.Collections;
using UnityEngine;

namespace Other
{
    [RequireComponent(typeof(CanvasGroup))]
    public class FadeInOut : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        [SerializeField] private float transitionTime;

        private void OnEnable()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void StartFadeInOut(Action onComplete = null)
        {
            if (_canvasGroup.alpha == 1f)
            {
                StartCoroutine(FadeOut(onComplete));
            }
            else if (_canvasGroup.alpha == 0f)
            {
                StartCoroutine(FadeIn(onComplete));
            }
        }

        private IEnumerator FadeIn(Action onComplete)
        {
            while (_canvasGroup.alpha < 1)
            {
                _canvasGroup.alpha += transitionTime * Time.deltaTime;
                yield return null;
            }
            onComplete?.Invoke();
        }
        private IEnumerator FadeOut(Action onComplete)
        {
            while (_canvasGroup.alpha > 0)
            {
                _canvasGroup.alpha -= transitionTime * Time.deltaTime;
                yield return null;
            }
            
            onComplete?.Invoke();
        }
    }
}
