using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private WaveManager waveManager = null;
    // private Canvas canvas = null;

    void Awake()
    {
        waveManager = GetComponent<WaveManager>();
        // canvas = ...
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // canvas.enabled = !canvas.enabled;
            TogglePause();
        }

        if (!waveManager.IsFirstWaveSpawned())
        {
            waveManager.SpawnNextWave();
        }

        if (waveManager.IsCurrentWaveDefeated())
        {
            if (waveManager.AreAllWavesDefeated())
            {
                // to do: show victory text
            }
            else
            {
                // to do: show wave defeated text
                waveManager.SpawnNextWave();
            }
        }
    }

    public void LoadLevel(string levelName)
    {
        TogglePause();
        SceneManager.LoadScene(levelName);
    }

    public void TogglePause()
    {
        if (Time.timeScale == 0.0f)
        {
            Time.timeScale = 1.0f;
        }
        else
        {
            Time.timeScale = 0.0f;
        }
    }

    public void Quit()
    {
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
