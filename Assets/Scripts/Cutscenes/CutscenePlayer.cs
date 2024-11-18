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
            CutsceneEvents.OnCutsceneStarted += PrepareVideo;
            _videoPlayer.loopPointReached += FinishCutscene;
            CutsceneEvents.OnCutsceneSkipped += SkipCutscene;
        }
        
        private void OnDisable()
        {
            CutsceneEvents.OnCutsceneStarted -= PrepareVideo;
            _videoPlayer.loopPointReached -= FinishCutscene;
            CutsceneEvents.OnCutsceneSkipped -= SkipCutscene;
        }
        
        private void PrepareVideo(CutsceneSO cutsceneInfo)
        {
            _videoPlayer.targetTexture.Release();
            _videoPlayer.clip = cutsceneInfo.clip;
            _videoPlayer.Prepare();
            _videoPlayer.prepareCompleted += StartCutscene;
        }
        
        private void StartCutscene(VideoPlayer source)
        {
            source.prepareCompleted -= StartCutscene;
            source.Play();
            source.Pause();  
            source.frame = 0;
            Invoke(nameof(StartPlayback), 0.1f);
        }
        
        void StartPlayback()
        {
            _videoPlayer.Play();
            UserInterfaceManager.Instance.ChangeState(UserInterfaceState.Cutscene);
        }
        
        
        private void FinishCutscene(VideoPlayer player)
        {
            _videoPlayer.clip = null;
            CutsceneEvents.FinishCutscene();
            UserInterfaceManager.Instance.ChangeState(UserInterfaceState.Exploration);
        }
        
        private void SkipCutscene()
        {
            _videoPlayer.Stop();
            _videoPlayer.clip = null;
            CutsceneEvents.FinishCutscene();
            UserInterfaceManager.Instance.ChangeState(UserInterfaceState.Exploration);
        }
    }
}

