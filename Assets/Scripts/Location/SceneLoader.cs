using System;
using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Location
{
    public class SceneLoader : MonoBehaviour
    {
        public static event Action<string> OnSceneChanged;

        public static void InvokeOnSceneChanged(string sceneName)
        {
            OnSceneChanged?.Invoke(sceneName);
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
        
        private IEnumerator LoadSceneAsync(string sceneName)
        {
            var asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            asyncLoad.allowSceneActivation = false;
            
            while (asyncLoad.progress < 0.9f)
            {
                yield return null;
            }
            
            yield return null;
            
            yield return new WaitForSeconds(2f);

            asyncLoad.allowSceneActivation = true;
        }
    }
}