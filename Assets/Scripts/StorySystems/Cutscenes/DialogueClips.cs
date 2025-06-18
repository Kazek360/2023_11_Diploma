using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace StorySystems.Cutscenes
{
    public class DialogueClips : MonoBehaviour
    {
        [SerializeField] private VideoPlayer firstPlayer;
        [SerializeField] private VideoPlayer loopPlayer;
        [SerializeField] private VideoClip firstClip;
        [SerializeField] private VideoClip loopClip;
        [SerializeField] private RawImage displayImage;
        [SerializeField] private CanvasGroup canvasGroup;
        
        
        private void OnEnable()
        {
            CutsceneEvents.OnDialogueClipStarted += StartClips;
            CutsceneEvents.OnDialogueClipFinished += StopClips;
        }

        private void OnDisable()
        {
            CutsceneEvents.OnDialogueClipStarted -= StartClips;
            CutsceneEvents.OnDialogueClipFinished -= StopClips;
        }

        private void StartClips()
        {
            // Setup both players
            canvasGroup.alpha = 1;
            firstPlayer.clip = firstClip;
            loopPlayer.clip = loopClip;
            firstPlayer.isLooping = false;
            loopPlayer.isLooping = true;

            // Prepare both
            firstPlayer.Prepare();
            loopPlayer.Prepare();

            firstPlayer.prepareCompleted += (vp) =>
            {
                firstPlayer.Play();
                displayImage.texture = firstPlayer.targetTexture;
                StartCoroutine(MonitorFirstClip());
            };
        }

        private IEnumerator MonitorFirstClip()
        {
            while (firstPlayer.isPlaying)
            {
                if (firstPlayer.time >= firstPlayer.clip.length - 0.1)
                {
                    // Prestart the loop player just before first ends
                    loopPlayer.Play();
                    displayImage.texture = loopPlayer.targetTexture;
                    firstPlayer.Stop(); // stop first one to free resources
                    yield break;
                }
                yield return null;
            }
        }

        private void StopClips()
        {
            canvasGroup.alpha = 0;
            firstPlayer.Stop();
            firstPlayer.clip = null;
            loopPlayer.Stop();
            loopPlayer.clip = null;
        }
    }
}