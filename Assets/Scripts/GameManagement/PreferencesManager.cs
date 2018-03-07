using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.UI;

public class PreferencesManager : MonoBehaviour
{
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider qualitySlider;
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

    private void SetAmbientOcclusion()
    {
        int index = ambientOcclusionToggle.isOn ? 1 : 0;
        PlayerPrefs.SetInt("AmbientOcclusion", index);
    }

    private void LoadPreferences()
    {
        if (!PlayerPrefs.HasKey("SoundLevel"))
        {
            PlayerPrefs.SetFloat("SoundLevel", 75.0f);
        }
        if (soundSlider != null)
        {
            soundSlider.value = PlayerPrefs.GetFloat("SoundLevel");
        }

        if (!PlayerPrefs.HasKey("MusicLevel"))
        {
            PlayerPrefs.SetFloat("MusicLevel", 75.0f);
        }
        if (musicSlider != null)
        {
            musicSlider.value = PlayerPrefs.GetFloat("MusicLevel");
        }

        AudioListener.volume = PlayerPrefs.GetFloat("SoundLevel");
        if (musicPlayer != null)
        {
            musicPlayer.volume = PlayerPrefs.GetFloat("MusicLevel");
        }

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

        if (!PlayerPrefs.HasKey("AmbientOcclusion"))
        {
            PlayerPrefs.SetInt("AmbientOcclusion", 0);
        }
        bool ambientOcclusionSetting = PlayerPrefs.GetInt("AmbientOcclusion") == 1 ? true : false;
        if (ambientOcclusionToggle != null)
        {
            ambientOcclusionToggle.isOn = ambientOcclusionSetting;
        }
        postProcessing.profile.ambientOcclusion.enabled = ambientOcclusionSetting;
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
}
