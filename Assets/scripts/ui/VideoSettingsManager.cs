using UnityEngine;

public class VideoSettingsManager : MonoBehaviour
{
    private void Start()
    {
        LoadVideoSettings();
    }

    public void SetShadowQualityOff()
    {
        QualitySettings.shadows = ShadowQuality.Disable;
        QualitySettings.shadowResolution = ShadowResolution.Low;
        QualitySettings.shadowDistance = 0f;

        PlayerPrefs.SetInt("video_shadow_quality", 0);
        PlayerPrefs.Save();

        Debug.Log("Shadow quality set to OFF");
    }

    public void SetShadowQualityLow()
    {
        QualitySettings.shadows = ShadowQuality.HardOnly;
        QualitySettings.shadowResolution = ShadowResolution.Low;
        QualitySettings.shadowDistance = 25f;

        PlayerPrefs.SetInt("video_shadow_quality", 1);
        PlayerPrefs.Save();

        Debug.Log("Shadow quality set to LOW");
    }

    public void SetShadowQualityHigh()
    {
        QualitySettings.shadows = ShadowQuality.All;
        QualitySettings.shadowResolution = ShadowResolution.High;
        QualitySettings.shadowDistance = 80f;

        PlayerPrefs.SetInt("video_shadow_quality", 2);
        PlayerPrefs.Save();

        Debug.Log("Shadow quality set to HIGH");
    }

    public void SetMotionBlurOff()
    {
        PlayerPrefs.SetInt("video_motion_blur", 0);
        PlayerPrefs.Save();

        Debug.Log("Motion blur set to OFF");
    }

    public void SetMotionBlurOn()
    {
        PlayerPrefs.SetInt("video_motion_blur", 1);
        PlayerPrefs.Save();

        Debug.Log("Motion blur set to ON");
    }

    public void SetTextureQualityLow()
    {
        QualitySettings.globalTextureMipmapLimit = 2;

        PlayerPrefs.SetInt("video_texture_quality", 0);
        PlayerPrefs.Save();

        Debug.Log("Texture quality set to LOW");
    }

    public void SetTextureQualityMedium()
    {
        QualitySettings.globalTextureMipmapLimit = 1;

        PlayerPrefs.SetInt("video_texture_quality", 1);
        PlayerPrefs.Save();

        Debug.Log("Texture quality set to MEDIUM");
    }

    public void SetTextureQualityHigh()
    {
        QualitySettings.globalTextureMipmapLimit = 0;

        PlayerPrefs.SetInt("video_texture_quality", 2);
        PlayerPrefs.Save();

        Debug.Log("Texture quality set to HIGH");
    }

    private void LoadVideoSettings()
    {
        int shadowQuality = PlayerPrefs.GetInt("video_shadow_quality", 1);

        if (shadowQuality == 0)
        {
            QualitySettings.shadows = ShadowQuality.Disable;
            QualitySettings.shadowResolution = ShadowResolution.Low;
            QualitySettings.shadowDistance = 0f;
        }
        else if (shadowQuality == 1)
        {
            QualitySettings.shadows = ShadowQuality.HardOnly;
            QualitySettings.shadowResolution = ShadowResolution.Low;
            QualitySettings.shadowDistance = 25f;
        }
        else
        {
            QualitySettings.shadows = ShadowQuality.All;
            QualitySettings.shadowResolution = ShadowResolution.High;
            QualitySettings.shadowDistance = 80f;
        }

        int textureQuality = PlayerPrefs.GetInt("video_texture_quality", 2);

        if (textureQuality == 0)
        {
            QualitySettings.globalTextureMipmapLimit = 2;
        }
        else if (textureQuality == 1)
        {
            QualitySettings.globalTextureMipmapLimit = 1;
        }
        else
        {
            QualitySettings.globalTextureMipmapLimit = 0;
        }

        PlayerPrefs.SetInt("video_motion_blur", 0);
        PlayerPrefs.Save();
    }
}