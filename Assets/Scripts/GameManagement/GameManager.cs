#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private WaveManager waveManager;
    private Canvas HUDCanvas;
    private Canvas menuCanvas;
    private MenuController menuController;
    private PlayerHealth playerHealth;

    private float waveSpawnDelay = 3.0f;
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

        if (Input.GetButtonDown("Cancel") && !gameOver && !victory)
        {
            menuController.GoToMenuPanel();
            TogglePause();
        }

        if (Input.GetButtonDown("Screenshot"))
        {
            ScreenCapture.CaptureScreenshot("Screenshot" + Random.Range(0, 100000) + ".png");
        }

        if (!waveManager.IsFirstWaveSpawned())
        {
            waveManager.SpawnNextWave();
        }

        if (waveManager.IsCurrentWaveDefeated())
        {
            if (waveManager.AreAllWavesDefeated())
            {
                victory = true;
                DespawnEnemies();
                Invoke("TriggerVictory", 3.0f);
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

    public void TogglePauseAndIncreaseHealth()
    {
        playerHealth.SetBaseHealth(800);
        TogglePause();
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

    public void TogglePauseDefaultMenu()
    {
        menuController.GoToMenuPanel();
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
            if (enemies[i] != null && !enemies[i].GetComponent<EnemyHealth>().IsDead())
            {
                Destroy(enemies[i]);
            }
        }
    }
}
