using System.Collections.Generic;
using InputSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Cutscenes
{
    public class Cutscene : MonoBehaviour
    {
        [SerializeField] private GameObject contentParent;
        [SerializeField] private Image cutsceneImage;
        private List<Sprite> _cutsceneImages;
        private int _currentCutsceneIndex;
        
        private void OnEnable()
        {
            CutsceneEvents.OnCutsceneStarted += StartCutscene;
            InputEvents.OnCutsceneContinueInput += ContinueCutscene;
        }
        
        private void OnDisable()
        {
            CutsceneEvents.OnCutsceneStarted -= StartCutscene;
            InputEvents.OnCutsceneContinueInput -= ContinueCutscene;
        }
        
        private void Start()
        {
            contentParent.SetActive(false);
        }
        private void StartCutscene(CutsceneSO cutsceneInfo)
        {
            _cutsceneImages = cutsceneInfo.cutsceneImages;
            _currentCutsceneIndex = 0;
            cutsceneImage.sprite = _cutsceneImages[_currentCutsceneIndex];
            _currentCutsceneIndex++;
            contentParent.SetActive(true);
            InputEvents.OpenInterface();
        }

        private void ContinueCutscene()
        {
            if (_currentCutsceneIndex < _cutsceneImages.Count)
            {
                cutsceneImage.sprite = _cutsceneImages[_currentCutsceneIndex];
                _currentCutsceneIndex++;
            }
            else
            {
                contentParent.SetActive(false);
                CutsceneEvents.FinishCutscene();
            }
        }
    }
}

