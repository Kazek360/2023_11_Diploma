[System.Serializable]

public class SettingsData
{
    private string _defaultWindowResolution = "1920x1080";
    private string _defaultWindowMode = "Pełny Ekran";
    private float _defaultMasterVolume = 1f;
    private float _defaultMusicVolume = 1f;
    private float _defaultSfxVolume = 1f;
    private bool _defaultMouseMovement = false;
    
    public string windowResolution;
    public string windowMode;
    public float masterVolume;
    public float musicVolume;
    public float sfxVolume;
    public bool mouseMovement;

    public string DefaultWindowResolution => _defaultWindowResolution;

    public string DefaultWindowMode => _defaultWindowMode;

    public float DefaultMasterVolume => _defaultMasterVolume;

    public float DefaultMusicVolume => _defaultMusicVolume;

    public float DefaultSfxVolume => _defaultSfxVolume;
    
    public bool DefaultMouseMovement => _defaultMouseMovement;
}
