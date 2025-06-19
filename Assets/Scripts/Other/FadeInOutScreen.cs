using System;
using System.Collections;
using UnityEngine;

namespace Other
{
    [RequireComponent(typeof(CanvasGroup))]
    public class FadeInOutScreen : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        [SerializeField] private float transitionTime;

        public static event Action<Action> OnFade;
        public static void InvokeOnFade(Action onComplete = null)
        {
            OnFade?.Invoke(onComplete);
        }
        public static event Action<Action> OnFadeInOut;
        public static void InvokeOnFadeInOut(Action onComplete = null)
        {
            OnFadeInOut?.Invoke(onComplete);
        }
        public static event Action<Action> OnFadeIn;
        public static void InvokeOnFadeIn(Action onComplete = null)
        {
            OnFadeIn?.Invoke(onComplete);
        }
        public static event Action<Action> OnFadeOut;
        public static void InvokeOnFadeOut(Action onComplete = null)
        {
            OnFadeOut?.Invoke(onComplete);
        }
        
        private void OnEnable()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            OnFade += StartFade;
            OnFadeInOut += StartFadeInOut;
            OnFadeIn += StartFadeIn;
            OnFadeOut += StartFadeOut;
        }
        private void OnDisable()
        {
            OnFade -= StartFade;
            OnFadeInOut -= StartFadeInOut;
            OnFadeIn -= StartFadeIn;
            OnFadeOut -= StartFadeOut;
        }
        
        public void StartFade(Action onComplete = null)
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

        public void StartFadeInOut(Action onFadeIn = null)
        {
            StartCoroutine(FadeInOut(onFadeIn));
        }

        private IEnumerator FadeInOut(Action onFadeIn)
        {
            yield return StartCoroutine(FadeIn(onFadeIn));
            yield return new WaitForSeconds(0.25f);
            yield return StartCoroutine(FadeOut(null));
        }
        
        public void StartFadeIn(Action onComplete = null)
        {
            StartCoroutine(FadeIn(onComplete));
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
        
        public void StartFadeOut(Action onComplete = null)
        {
            StartCoroutine(FadeOut(onComplete));
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
