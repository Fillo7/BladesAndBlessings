using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private WaveManager waveManager;
    private Canvas canvas;
    private MenuController menuController;
    private PlayerHealth playerHealth;

    private bool gameOver = false;

    void Awake()
    {
        waveManager = GetComponent<WaveManager>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        canvas = GameObject.FindGameObjectWithTag("MenuCanvas").GetComponent<Canvas>();
        menuController = canvas.GetComponent<MenuController>();
        canvas.enabled = false;
        // TogglePause();
    }

    void Update()
    {
        if (gameOver)
        {
            return;
        }

        if (playerHealth.IsDead())
        {
            TriggerGameOver();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !gameOver)
        {
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
                waveManager.SpawnNextWave(2.0f);
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
        canvas.enabled = !canvas.enabled;
        menuController.GoToMenuPanel();

        if (Time.timeScale == 0.0f)
        {
            Time.timeScale = 1.0f;
        }
        else
        {
            Time.timeScale = 0.0f;
        }
    }

    public void TriggerGameOver()
    {
        TogglePause();
        menuController.GoToGameOverPanel();
        gameOver = true;
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
