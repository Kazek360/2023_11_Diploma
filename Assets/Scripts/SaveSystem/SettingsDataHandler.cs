using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsDataHandler : MonoBehaviour
{
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public SoundMixerManager soundMixerManager;

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
