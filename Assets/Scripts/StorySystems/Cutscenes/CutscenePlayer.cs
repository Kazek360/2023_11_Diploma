using UI;
using UnityEngine;
using UnityEngine.Video;

namespace StorySystems.Cutscenes
{
    public class CutscenePlayer : MonoBehaviour
    {
        private VideoPlayer _videoPlayer;
        private Cutscene _currentCutscene;

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
        
        private void PrepareVideo(Cutscene cutsceneInfo)
        {
            _videoPlayer.targetTexture.Release();
            _currentCutscene = cutsceneInfo;
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
            CutsceneEvents.FinishCutscene(_currentCutscene);
            UserInterfaceManager.Instance.ChangeState(UserInterfaceState.Exploration);
        }
        
        private void SkipCutscene()
        {
            _videoPlayer.Stop();
            _videoPlayer.clip = null;
            CutsceneEvents.FinishCutscene(_currentCutscene);
            UserInterfaceManager.Instance.ChangeState(UserInterfaceState.Exploration);
        }
    }
}
