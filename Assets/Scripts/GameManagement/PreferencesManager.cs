using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.UI;

public class PreferencesManager : MonoBehaviour
{
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider qualitySlider;
    [SerializeField] private Toggle fullScreenToggle;
    [SerializeField] private Dropdown resolutionDropdown;
    [SerializeField] private Toggle ambientOcclusionToggle;
    [SerializeField] private PostProcessingBehaviour postProcessing;

    [SerializeField] private AudioSource musicPlayer;

    void Awake()
    {
        if (soundSlider != null)
        {
            soundSlider.onValueChanged.AddListener(delegate { SetSoundLevel(); });
        }
        if (musicSlider != null)
        {
            musicSlider.onValueChanged.AddListener(delegate { SetMusicLevel(); });
        }
        if (qualitySlider != null)
        {
            qualitySlider.onValueChanged.AddListener(delegate { SetQualityLevel(); });
        }
        if (fullScreenToggle != null)
        {
            fullScreenToggle.onValueChanged.AddListener(delegate { SetScreenMode(); });
        }
        if (resolutionDropdown != null)
        {
            resolutionDropdown.onValueChanged.AddListener(delegate { SetResolution(); });
        }
        if (ambientOcclusionToggle != null)
        {
            ambientOcclusionToggle.onValueChanged.AddListener(delegate { SetAmbientOcclusion(); });
        }

        if (musicPlayer != null)
        {
            musicPlayer.ignoreListenerVolume = true;
        }

        LoadPreferences();
    }

    private void SetSoundLevel()
    {
        PlayerPrefs.SetFloat("SoundLevel", soundSlider.value);
        AudioListener.volume = soundSlider.value;
    }

    private void SetMusicLevel()
    {
        PlayerPrefs.SetFloat("MusicLevel", musicSlider.value);
        if (musicPlayer != null)
        {
            musicPlayer.volume = musicSlider.value;
        }
    }

    private void SetQualityLevel()
    {
        PlayerPrefs.SetString("QualityLevel", GetQualityNameForIndex((int)qualitySlider.value));
    }

    private void SetScreenMode()
    {
        int index = fullScreenToggle.isOn ? 1 : 0;
        PlayerPrefs.SetInt("ScreenMode", index);
        Screen.fullScreen = fullScreenToggle.isOn;
    }

    private void SetResolution()
    {
        int activeIndex = resolutionDropdown.value;
        Resolution resolution = GetResolutionFromString(resolutionDropdown.options[activeIndex].text);

        bool fullscreen = true;
        if (PlayerPrefs.HasKey("ScreenMode"))
        {
            fullscreen = PlayerPrefs.GetInt("ScreenMode") == 1 ? true : false;
        }

        PlayerPrefs.SetInt("ScreenWidth", resolution.width);
        PlayerPrefs.SetInt("ScreenHeight", resolution.height);
        PlayerPrefs.SetInt("ScreenRefreshRate", resolution.refreshRate);
        if (resolution.width != Screen.width
            || resolution.height != Screen.height
            || resolution.refreshRate != Screen.currentResolution.refreshRate)
        {
            Screen.SetResolution(resolution.width, resolution.height, fullscreen, resolution.refreshRate);
        }
    }

    private void SetAmbientOcclusion()
    {
        int index = ambientOcclusionToggle.isOn ? 1 : 0;
        PlayerPrefs.SetInt("AmbientOcclusion", index);
    }

    private void LoadPreferences()
    {
        // Sound level
        if (!PlayerPrefs.HasKey("SoundLevel"))
        {
            PlayerPrefs.SetFloat("SoundLevel", 80.0f);
        }
        AudioListener.volume = PlayerPrefs.GetFloat("SoundLevel");
        if (soundSlider != null)
        {
            soundSlider.value = PlayerPrefs.GetFloat("SoundLevel");
        }

        // Music level
        if (!PlayerPrefs.HasKey("MusicLevel"))
        {
            PlayerPrefs.SetFloat("MusicLevel", 30.0f);
        }
        if (musicPlayer != null)
        {
            musicPlayer.volume = PlayerPrefs.GetFloat("MusicLevel");
        }
        if (musicSlider != null)
        {
            musicSlider.value = PlayerPrefs.GetFloat("MusicLevel");
        }

        // Graphics quality level
        if (!PlayerPrefs.HasKey("QualityLevel"))
        {
            PlayerPrefs.SetString("QualityLevel", "Medium");
        }
        int qualityIndex = GetQualityIndexForName(PlayerPrefs.GetString("QualityLevel"));
        QualitySettings.SetQualityLevel(qualityIndex, true);
        if (qualitySlider != null)
        {
            qualitySlider.value = (float)qualityIndex;
        }

        // Screen mode
        if (!PlayerPrefs.HasKey("ScreenMode"))
        {
            PlayerPrefs.SetInt("ScreenMode", 1);
        }
        bool screenMode = PlayerPrefs.GetInt("ScreenMode") == 1 ? true : false;
        Screen.fullScreen = screenMode;
        if (fullScreenToggle != null)
        {
            fullScreenToggle.isOn = screenMode;
        }

        // Screen resolution
        if (!PlayerPrefs.HasKey("ScreenWidth"))
        {
            PlayerPrefs.SetInt("ScreenWidth", Screen.currentResolution.width);
        }
        if (!PlayerPrefs.HasKey("ScreenHeight"))
        {
            PlayerPrefs.SetInt("ScreenHeight", Screen.currentResolution.height);
        }
        if (!PlayerPrefs.HasKey("ScreenRefreshRate"))
        {
            PlayerPrefs.SetInt("ScreenRefreshRate", Screen.currentResolution.refreshRate);
        }
        Resolution newResolution = new Resolution();
        newResolution.width = PlayerPrefs.GetInt("ScreenWidth");
        newResolution.height = PlayerPrefs.GetInt("ScreenHeight");
        newResolution.refreshRate = PlayerPrefs.GetInt("ScreenRefreshRate");

        if (newResolution.width != Screen.width
            || newResolution.height != Screen.height
            || newResolution.refreshRate != Screen.currentResolution.refreshRate)
        {
            Screen.SetResolution(newResolution.width, newResolution.height, screenMode, newResolution.refreshRate);
        }
        
        Resolution[] resolutions = Screen.resolutions;
        List<string> resolutionOptions = new List<string>();
        int activeResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            resolutionOptions.Add(resolutions[i].ToString());
            if (newResolution.ToString() == resolutions[i].ToString())
            {
                activeResolutionIndex = i;
            }
        }
        if (resolutionDropdown != null)
        {
            resolutionDropdown.ClearOptions();
            resolutionDropdown.AddOptions(resolutionOptions);
            resolutionDropdown.value = activeResolutionIndex;
        }

        // Ambient occlusion
        if (!PlayerPrefs.HasKey("AmbientOcclusion"))
        {
            PlayerPrefs.SetInt("AmbientOcclusion", 0);
        }
        bool ambientOcclusionSetting = PlayerPrefs.GetInt("AmbientOcclusion") == 1 ? true : false;
        postProcessing.profile.ambientOcclusion.enabled = ambientOcclusionSetting;
        if (ambientOcclusionToggle != null)
        {
            ambientOcclusionToggle.isOn = ambientOcclusionSetting;
        }
    }

    private int GetQualityIndexForName(string name)
    {
        string[] qualitySettings = QualitySettings.names;

        for (int i = 0; i < qualitySettings.Length; i++)
        {
            if (qualitySettings[i].Equals(name))
            {
                return i;
            }
        }

        return 0;
    }

    private string GetQualityNameForIndex(int index)
    {
        if (index == 3)
        {
            return "Very High";
        }
        else if (index == 2)
        {
            return "High";
        }
        else if (index == 1)
        {
            return "Medium";
        }
        return "Low";
    }

    Resolution GetResolutionFromString(string resolutionString)
    {
        Resolution resolution = new Resolution();

        string[] separatingChars = {"x", "@"};
        string[] result = resolutionString.Split(separatingChars, System.StringSplitOptions.RemoveEmptyEntries);
        result[1] = result[1].Replace(" ", "");
        result[2] = result[2].Replace("Hz", "");
        result[2] = result[2].Replace(" ", "");

        resolution.width = System.Int32.Parse(result[0]);
        resolution.height = System.Int32.Parse(result[1]);
        resolution.refreshRate = System.Int32.Parse(result[2]);

        return resolution;
    }
}
