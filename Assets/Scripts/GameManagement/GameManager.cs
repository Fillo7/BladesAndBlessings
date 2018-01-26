using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private WaveManager waveManager;
    private Canvas HUDCanvas;
    private Canvas menuCanvas;
    private MenuController menuController;
    private PlayerHealth playerHealth;

    private bool gameOver = false;
    private bool victory = false;

    void Awake()
    {
        waveManager = GetComponent<WaveManager>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        HUDCanvas = GameObject.FindGameObjectWithTag("HUDCanvas").GetComponent<Canvas>();
        menuCanvas = GameObject.FindGameObjectWithTag("MenuCanvas").GetComponent<Canvas>();
        menuController = menuCanvas.GetComponent<MenuController>();
        menuCanvas.enabled = false;
        // TogglePause();
    }

    void Update()
    {
        if (gameOver || victory)
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
            waveManager.SpawnNextWave(2.0f);
        }

        if (waveManager.IsCurrentWaveDefeated())
        {
            if (waveManager.AreAllWavesDefeated())
            {
                HUDCanvas.GetComponent<Animator>().SetTrigger("Victory");
                victory = true;
            }
            else
            {
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
        menuCanvas.enabled = !menuCanvas.enabled;
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
