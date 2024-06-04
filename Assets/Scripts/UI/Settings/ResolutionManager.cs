using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    private Resolution[] _resolutions;
    private List<Resolution> _filteredResolutions;

    private double _currentRefreshRate;
    private int _currentResolutionIndex;
    
    private void Start()
    {
        _resolutions = Screen.resolutions;
        _filteredResolutions = new List<Resolution>();
        resolutionDropdown.ClearOptions();
        _currentRefreshRate = Screen.currentResolution.refreshRateRatio.value;

        foreach (var resolution in _resolutions)
        {
            if (resolution.refreshRateRatio.value.Equals(_currentRefreshRate))
            {
                _filteredResolutions.Add(resolution);
            }
        }

        var options = new List<string>();

        for (var i = 0; i < _filteredResolutions.Count; i++)
        {
            options.Add($"{_filteredResolutions[i].width}x{_filteredResolutions[i].height}");
            if (_filteredResolutions[i].width == Screen.width && _filteredResolutions[i].height == Screen.height)
            {
                _currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = _currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        var resolution = _filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode);
    }
    
    public void SetResolutionMode(int modeIndex)
    {
        Screen.fullScreenMode = modeIndex switch
        {
            0 => FullScreenMode.FullScreenWindow,
            1 => FullScreenMode.ExclusiveFullScreen,
            2 => FullScreenMode.Windowed,
            _ => Screen.fullScreenMode
        };
    }
}
