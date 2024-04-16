using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cutscene : MonoBehaviour
{
    [SerializeField] private GameObject contentParent;
    [SerializeField] private Image cutsceneImage;
    private List<Sprite> _cutsceneImages;
    private int _currentCutsceneIndex;
    
    private void OnEnable()
    {
        CutsceneEvents.OnCutsceneOpen += OpenCutscene;
    }
    
    private void OnDisable()
    {
        CutsceneEvents.OnCutsceneOpen -= OpenCutscene;
    }
    
    private void Start()
    {
        contentParent.SetActive(false);
    }
    private void OpenCutscene(CutsceneSO cutsceneInfo)
    {
        _cutsceneImages = cutsceneInfo.cutsceneImages;
        _currentCutsceneIndex = 0;
        cutsceneImage.sprite = _cutsceneImages[_currentCutsceneIndex];
        _currentCutsceneIndex++;
        contentParent.SetActive(true);
        InterfaceEvents.OpenInterface(gameObject.name);
    }
    
    public void Update()
    {
        if (contentParent.activeSelf && Input.GetButtonDown("Fire1"))
        {
            if (_currentCutsceneIndex < _cutsceneImages.Count)
            {
                cutsceneImage.sprite = _cutsceneImages[_currentCutsceneIndex];
                _currentCutsceneIndex++;
            }
            else
            {
                contentParent.SetActive(false);
                InterfaceEvents.CloseInterface();
                CutsceneEvents.CloseCutscene();
            }
        }
    }
}
