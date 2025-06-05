using System.IO;
using InputSystem;
using Shared;
using UnityEngine;
using UnityEngine.UI;

public class SettingsDataHandler : Singleton<SettingsDataHandler>
{
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public SoundMixerManager soundMixerManager;

    private bool _mouseMovement;
    public bool MouseMovement
    {
        get => _mouseMovement;
        set
        {
            _mouseMovement = value;
            InputEvents.InvokeOnInputChanged(value);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        MouseMovement = false;
    }
    
    private void Start()
    {
        LoadSettings();
    }
    public void SaveSettings()
    {
        SettingsData settingsData = new SettingsData();
        settingsData.masterVolume = masterVolumeSlider.value;
        settingsData.musicVolume = musicVolumeSlider.value;
        settingsData.sfxVolume = sfxVolumeSlider.value;
        settingsData.mouseMovement = MouseMovement;

        string json = JsonUtility.ToJson(settingsData, true);
        File.WriteAllText(Application.dataPath + "/settings.json", json);
    }

    public void LoadSettings()
    {
        string json = File.ReadAllText(Application.dataPath + "/settings.json");
        SettingsData settingsData = JsonUtility.FromJson<SettingsData>(json);

        masterVolumeSlider.value = settingsData.masterVolume;
        musicVolumeSlider.value = settingsData.musicVolume;
        sfxVolumeSlider.value = settingsData.sfxVolume;
        MouseMovement = settingsData.mouseMovement;
        
        soundMixerManager.SetMasterVolume(settingsData.masterVolume);
        soundMixerManager.SetMusicVolume(settingsData.musicVolume);
        soundMixerManager.SetSoundFxVolume(settingsData.musicVolume);
        
    }

    public void DefaultSetting()
    {
        SettingsData settingsData = new SettingsData();
        masterVolumeSlider.value = settingsData.DefaultMasterVolume;
        musicVolumeSlider.value = settingsData.DefaultMusicVolume;
        sfxVolumeSlider.value = settingsData.DefaultSfxVolume;
    }
}
