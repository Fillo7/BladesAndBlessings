﻿#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int weaponUnlockIndex = 0;
    [SerializeField] private RawImage weaponImage;
    [SerializeField] private Texture weaponTexture;
    [SerializeField] private GameObject saveResetPanel;
    [SerializeField] private AudioSource musicClip;

    private Canvas HUDCanvas;
    private Canvas menuCanvas;
    private MenuController menuController;
    private LevelManager levelManager;
    private CustomInputManager inputManager;
    private SaveManager saveManager;
    bool gamePaused = true;
    bool inLoadout = false;
    bool inTutorial = false;

    void Awake()
    {
        GameObject HUD = GameObject.FindGameObjectWithTag("HUDCanvas");
        if (HUD != null)
        {
            HUDCanvas = HUD.GetComponent<Canvas>();
        }

        if (saveResetPanel != null)
        {
            saveResetPanel.SetActive(false);
        }

        menuCanvas = GameObject.FindGameObjectWithTag("MenuCanvas").GetComponent<Canvas>();
        menuController = menuCanvas.GetComponent<MenuController>();
        levelManager = GetComponent<LevelManager>();
        inputManager = menuCanvas.GetComponentInChildren<CustomInputManager>();
        saveManager = GetComponent<SaveManager>();

        Pause();
        InitializeLoadout();
    }

    void Update()
    {
        if (inputManager.GetKeyDown("InputCancel") && levelManager != null && levelManager.IsLevelActive() && !inputManager.IsWaitingForKey() && !inLoadout && !inTutorial)
        {
            TogglePauseWithDefaultMenu();
        }

        if (inputManager.GetKeyDown("InputScreenshot") && !inputManager.IsWaitingForKey())
        {
            ScreenCapture.CaptureScreenshot("BaB_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png");
        }
    }
    
    public void BeginNewGame(int unlockedLevelIndex)
    {
        if (unlockedLevelIndex > 1)
        {
            saveResetPanel.SetActive(true);
        }
        else
        {
            LoadLevelWithIndex(1);
        }
    }

    public void StartGame(int firstWeaponIndex, int secondWeaponIndex)
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>().SetActiveWeapons(firstWeaponIndex, secondWeaponIndex);
        HUDCanvas.enabled = true;
        inLoadout = false;
        Resume();

        if (musicClip != null)
        {
            musicClip.Play();
        }

        if (!saveManager.GetSaveData().GetTutorialSeen())
        {
            inTutorial = true;
            menuController.GoToTutorialPanel();
            Pause();
        }
    }

    public void FinishTutorial()
    {
        saveManager.UpdateTutorialSeen(true);
        Resume();
        inTutorial = false;
    }

    public void RestartLevel()
    {
        LoadLevel(SceneManager.GetActiveScene().name);
    }

    public void LoadLevelWithIndex(int index)
    {
        string name = GetLevelNameForIndex(index);
        LoadLevel(name);
    }

    public void LoadNextLevel()
    {
        string name = GetNextLevelName(SceneManager.GetActiveScene().name);
        LoadLevel(name);
    }

    public void LoadLevel(string levelName)
    {
        Pause();
        menuController.GoToLoadingPanel();
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
        string nextLevelName = GetNextLevelName(SceneManager.GetActiveScene().name);
        if (GetIndexForLevelName(nextLevelName) > saveManager.GetSaveData().GetUnlockedLevelCount())
        {
            saveManager.UpdateUnlockedLevelCount(GetIndexForLevelName(nextLevelName));
        }

        if (weaponUnlockIndex > saveManager.GetSaveData().GetUnlockedWeaponCount())
        {
            saveManager.UpdateUnlockedWeaponCount(weaponUnlockIndex);
            weaponImage.texture = weaponTexture;
            menuController.GoToWeaponUnlockPanel();
        }
        else
        {
            menuController.GoToVictoryPanel();
        }

        HUDCanvas.GetComponent<Animator>().SetTrigger("Victory");
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

    private int GetIndexForLevelName(string levelName)
    {
        switch (levelName)
        {
            case "00MainMenu":
                return 0;
            case "01CityOutskirts":
                return 1;
            case "02ForestRuins":
                return 2;
            case "03HauntedSwamp":
                return 3;
            case "04OrcEncampment":
                return 4;
            case "05TrollCave":
                return 5;
        }

        return 0;
    }

    private string GetLevelNameForIndex(int index)
    {
        switch (index)
        {
            case 0:
                return "00MainMenu";
            case 1:
                return "01CityOutskirts";
            case 2:
                return "02ForestRuins";
            case 3:
                return "03HauntedSwamp";
            case 4:
                return "04OrcEncampment";
            case 5:
                return "05TrollCave";
            default:
                return "05TrollCave";
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
                return "03HauntedSwamp";
            case "03HauntedSwamp":
                return "04OrcEncampment";
            case "04OrcEncampment":
                return "05TrollCave";
            case "05TrollCave":
                return "00MainMenu";
        }

        return "00MainMenu";
    }
}
