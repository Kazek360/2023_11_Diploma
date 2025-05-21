using System;
using System.Collections;
using Other;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Location
{
    public class SceneLoader : MonoBehaviour
    {
        public static event Action<string> OnSceneChanged;

        public static void InvokeOnSceneChanged(string sceneName)
        {
            OnSceneChanged?.Invoke(sceneName);
        }

        private FadeInOut _fadeInOut;
        private Slider _progressSlider;
        
        private void Awake()
        {
            _fadeInOut = GetComponent<FadeInOut>();
            _progressSlider = GetComponentInChildren<Slider>();
            _progressSlider.value = 0;
            _progressSlider.interactable = false;
        }

        private void OnEnable()
        {
            OnSceneChanged += LoadScene;
        }

        private void OnDisable()
        {
            OnSceneChanged -= LoadScene;
        }

        private void LoadScene(string sceneName)
        {
            UserInterfaceManager.Instance.ChangeState(UserInterfaceState.LoadingScreen);
            StartCoroutine(LoadSceneAsync(sceneName));
        }

        IEnumerator LoadSceneAsync(string sceneName)
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName);
            
            while (!loadOperation.isDone)
            {
                var progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);
                _progressSlider.value = progressValue;
                yield return null;
            }
        }
    }
}