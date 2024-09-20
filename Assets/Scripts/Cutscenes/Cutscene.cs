using UnityEngine;
using UnityEngine.Video;

namespace Cutscenes
{
    public class Cutscene : MonoBehaviour
    {
        [SerializeField] private GameObject contentParent;
        [SerializeField] private VideoPlayer videoPlayer;
        
        private void OnEnable()
        {
            CutsceneEvents.OnCutsceneStarted += StartCutscene;
            videoPlayer.loopPointReached += FinishCutscene;
        }
        
        private void OnDisable()
        {
            CutsceneEvents.OnCutsceneStarted -= StartCutscene;
            videoPlayer.loopPointReached -= FinishCutscene;
        }
        
        private void Start()
        {
            contentParent.SetActive(false);
        }

        private void StartCutscene(CutsceneSO cutsceneInfo)
        {
            videoPlayer.clip = cutsceneInfo.clip;
            contentParent.SetActive(true);
            videoPlayer.Play();
        }
        
        private void FinishCutscene(VideoPlayer player)
        {
            contentParent.SetActive(false);
            CutsceneEvents.FinishCutscene();
        }
    }
}

