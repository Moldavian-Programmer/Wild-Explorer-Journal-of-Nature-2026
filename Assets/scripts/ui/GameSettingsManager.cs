using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSettingsManager : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private Slider musicVolumeSlider;

    [Header("Video")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Toggle shadowsToggle;

    private readonly Vector2Int[] resolutions =
    {
        new Vector2Int(1280, 720),
        new Vector2Int(1600, 900),
        new Vector2Int(1920, 1080),
        new Vector2Int(2560, 1440)
    };

    private void Start()
    {
        LoadSettings();
        SetupResolutionDropdown();
    }

    public void SetMusicVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetResolution(int index)
    {
        if (index < 0 || index >= resolutions.Length)
        {
            return;
        }

        Vector2Int selectedResolution = resolutions[index];

        Screen.SetResolution(
            selectedResolution.x,
            selectedResolution.y,
            Screen.fullScreen
        );

        PlayerPrefs.SetInt("ResolutionIndex", index);
        PlayerPrefs.Save();
    }

    public void SetShadows(bool enabled)
    {
        QualitySettings.shadows = enabled ? ShadowQuality.All : ShadowQuality.Disable;

        PlayerPrefs.SetInt("Shadows", enabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void LoadSettings()
    {
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        bool savedFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        bool savedShadows = PlayerPrefs.GetInt("Shadows", 1) == 1;

        AudioListener.volume = savedVolume;
        Screen.fullScreen = savedFullscreen;
        QualitySettings.shadows = savedShadows ? ShadowQuality.All : ShadowQuality.Disable;

        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.value = savedVolume;
        }

        if (fullscreenToggle != null)
        {
            fullscreenToggle.isOn = savedFullscreen;
        }

        if (shadowsToggle != null)
        {
            shadowsToggle.isOn = savedShadows;
        }
    }

    private void SetupResolutionDropdown()
    {
        if (resolutionDropdown == null)
        {
            return;
        }

        resolutionDropdown.ClearOptions();

        var options = new System.Collections.Generic.List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            options.Add(resolutions[i].x + " x " + resolutions[i].y);
        }

        resolutionDropdown.AddOptions(options);

        int savedResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", 2);

        if (savedResolutionIndex < 0 || savedResolutionIndex >= resolutions.Length)
        {
            savedResolutionIndex = 2;
        }

        resolutionDropdown.value = savedResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        Vector2Int selectedResolution = resolutions[savedResolutionIndex];

        Screen.SetResolution(
            selectedResolution.x,
            selectedResolution.y,
            Screen.fullScreen
        );
    }
}