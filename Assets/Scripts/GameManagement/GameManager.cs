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

        // Enable following line for final build
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
            gameOver = true;
            Invoke("TriggerGameOver", 5.0f);
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !gameOver)
        {
            menuController.GoToMenuPanel();
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
                TriggerVictory();
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
        menuController.GoToGameOverPanel();
        TogglePause();
    }

    public void TriggerVictory()
    {
        HUDCanvas.GetComponent<Animator>().SetTrigger("Victory");
        menuController.GoToVictoryPanel();
        Invoke("TogglePause", 5.0f);
        victory = true;
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
