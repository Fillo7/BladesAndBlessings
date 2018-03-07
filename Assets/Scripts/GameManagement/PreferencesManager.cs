using UnityEngine;
using UnityEngine.UI;

public class PreferencesManager : MonoBehaviour
{
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Slider musicSlider;

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

        if (musicPlayer != null)
        {
            musicPlayer.ignoreListenerVolume = true;
        }

        LoadPreferences();
    }

    private void SetSoundLevel()
    {
        PlayerPrefs.SetFloat("SoundLevel", soundSlider.value);
    }

    private void SetMusicLevel()
    {
        PlayerPrefs.SetFloat("MusicLevel", musicSlider.value);
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
    }
}
