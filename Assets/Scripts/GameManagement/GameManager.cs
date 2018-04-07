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
    private CustomInputManager inputManager;
    bool gamePaused = true;
    bool inLoadout = false;

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
        inputManager = menuCanvas.GetComponentInChildren<CustomInputManager>();

        Pause();
        InitializeLoadout();
    }

    void Update()
    {
        if (inputManager.GetKeyDown("InputCancel") && levelManager != null && levelManager.IsLevelActive() && !inputManager.IsWaitingForKey() && !inLoadout)
        {
            TogglePauseWithDefaultMenu();
        }

        if (inputManager.GetKeyDown("InputScreenshot") && !inputManager.IsWaitingForKey())
        {
            ScreenCapture.CaptureScreenshot("Screenshot" + Random.Range(0, 100000) + ".png");
        }
    }
    
    public void StartGame(int firstWeaponIndex, int secondWeaponIndex)
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>().SetActiveWeapons(firstWeaponIndex, secondWeaponIndex);
        HUDCanvas.enabled = true;
        inLoadout = false;
        Resume();
    }

    public void RestartLevel()
    {
        LoadLevel(SceneManager.GetActiveScene().name);
    }

    public void LoadNextLevel()
    {
        string name = GetNextLevelName(SceneManager.GetActiveScene().name);
        LoadLevel(name);
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

    private void InitializeLoadout()
    {
        if (levelManager != null)
        {
            menuController.GoToLoadoutPanel();
            HUDCanvas.enabled = false;
            inLoadout = true;
        }
    }

    private string GetNextLevelName(string currentLevel)
    {
        switch (currentLevel)
        {
            case "00MainMenu":
                return "01CityOutskirts";
            case "01CityOutskirts":
                return "02ForestRuins";
            case "02ForestRuins":
                return "DarkForest";
            case "DarkForest":
                return "05TrollCave";
            case "05TrollCave":
                return "00MainMenu";
        }

        return "00MainMenu";
    }
}
