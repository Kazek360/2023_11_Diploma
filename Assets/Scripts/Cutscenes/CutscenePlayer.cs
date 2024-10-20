using UI;
using UnityEngine;
using UnityEngine.Video;

namespace Cutscenes
{
    public class CutscenePlayer : MonoBehaviour
    {
        private VideoPlayer _videoPlayer;

        private void Awake()
        {
            _videoPlayer = GetComponentInChildren<VideoPlayer>();
        }
        
        private void OnEnable()
        {
            CutsceneEvents.OnCutsceneStarted += StartCutscene;
            _videoPlayer.loopPointReached += FinishCutscene;
            CutsceneEvents.OnCutsceneSkipped += SkipCutscene;
        }
        
        private void OnDisable()
        {
            CutsceneEvents.OnCutsceneStarted -= StartCutscene;
            _videoPlayer.loopPointReached -= FinishCutscene;
            CutsceneEvents.OnCutsceneSkipped -= SkipCutscene;
        }
        
        private void StartCutscene(CutsceneSO cutsceneInfo)
        {
            UserInterfaceManager.Instance.ChangeState(UserInterfaceState.Cutscene);
            _videoPlayer.enabled = true;
            _videoPlayer.clip = cutsceneInfo.clip;
            _videoPlayer.Play();
        }
        
        private void FinishCutscene(VideoPlayer player)
        {
            _videoPlayer.enabled = false;
            CutsceneEvents.FinishCutscene();
            UserInterfaceManager.Instance.ChangeState(UserInterfaceState.Exploration);
        }
        
        private void SkipCutscene()
        {
            _videoPlayer.enabled = false;
            CutsceneEvents.FinishCutscene();
            UserInterfaceManager.Instance.ChangeState(UserInterfaceState.Exploration);
        }
    }
}

