using UnityEngine;
using UnityEngine.UI;

public class Cutscene : MonoBehaviour
{
    [SerializeField] private GameObject contentParent;
    [SerializeField] private Image cutsceneImage;
    
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
        cutsceneImage.sprite = cutsceneInfo.cutsceneImage;
        contentParent.SetActive(true);
        InterfaceEvents.OpenInterface(gameObject.name);
    }
    
    public void Update()
    {
        if (contentParent.activeSelf && Input.GetButtonDown("Fire1"))
        {
            contentParent.SetActive(false);
            InterfaceEvents.CloseInterface();
            CutsceneEvents.CloseCutscene();
        }
    }
}
