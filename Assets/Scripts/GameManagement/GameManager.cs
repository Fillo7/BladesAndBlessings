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

    private float waveSpawnDelay = 2.0f;
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

        // Comment following line to disable welcome message
        TogglePause();
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
            waveManager.SpawnNextWave(waveSpawnDelay);
        }

        if (waveManager.IsCurrentWaveDefeated())
        {
            if (waveManager.AreAllWavesDefeated())
            {
                TriggerVictory();
            }
            else
            {
                waveManager.SpawnNextWave(waveSpawnDelay);
            }
        }
    }
    
    public void RestartLevel()
    {
        LoadLevel(SceneManager.GetActiveScene().name);
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
        DespawnEnemies();
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

    private void DespawnEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                Destroy(enemies[i]);
            }
        }
    }
}
