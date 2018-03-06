#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private Canvas HUDCanvas;
    private Canvas menuCanvas;
    private MenuController menuController;
    private LevelManager levelManager;
    bool gamePaused = true;

    void Awake()
    {
        GameObject HUD = GameObject.FindGameObjectWithTag("HUDCanvas");
        if (HUD != null)
        {
            HUDCanvas = HUD.GetComponent<Canvas>();
        }

        menuCanvas = GameObject.FindGameObjectWithTag("MenuCanvas").GetComponent<Canvas>();
        menuController = menuCanvas.GetComponent<MenuController>();
        levelManager = GetComponent<LevelManager>();

        Pause();
        if (levelManager != null)
        {
            Resume();
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel") && levelManager != null && levelManager.IsLevelActive())
        {
            TogglePauseWithDefaultMenu();
        }

        if (Input.GetButtonDown("Screenshot"))
        {
            ScreenCapture.CaptureScreenshot("Screenshot" + Random.Range(0, 100000) + ".png");
        }
    }
    
    public void RestartLevel()
    {
        Pause();
        LoadLevel(SceneManager.GetActiveScene().name);
    }

    public void LoadLevel(string levelName)
    {
        Pause();
        SceneManager.LoadScene(levelName);
    }

    public void Pause()
    {
        gamePaused = true;
        Time.timeScale = 0.0f;
        menuCanvas.enabled = true;
    }

    public void Resume()
    {
        gamePaused = false;
        Time.timeScale = 1.0f;
        menuCanvas.enabled = false;
    }

    public void TogglePause()
    {
        if (gamePaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void TogglePauseWithDefaultMenu()
    {
        menuController.GoToMenuPanel();
        TogglePause();
    }

    public void TriggerGameOver()
    {
        menuController.GoToGameOverPanel();
        Pause();
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
}
