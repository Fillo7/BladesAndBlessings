using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject levelPanel;
    [SerializeField] private GameObject controlsPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject audioPanel;
    [SerializeField] private GameObject graphicsPanel;
    [SerializeField] private GameObject loadoutPanel;
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private GameObject saveResetPanel;

    void Awake()
    {
        if (loadoutPanel != null)
        {
            GoToLoadoutPanel();
        }
        else
        {
            GoToMenuPanel();
        }
    }

    public void GoToMenuPanel()
    {
        ResetPanels();
        menuPanel.SetActive(true);
    }

    public void GoToOptionsPanel()
    {
        ResetPanels();
        optionsPanel.SetActive(true);
    }

    public void GoToLevelPanel()
    {
        ResetPanels();
        levelPanel.SetActive(true);
    }

    public void GoToControlsPanel()
    {
        ResetPanels();
        controlsPanel.SetActive(true);
    }

    public void GoToGameOverPanel()
    {
        ResetPanels();
        gameOverPanel.SetActive(true);
    }

    public void GoToVictoryPanel()
    {
        ResetPanels();
        victoryPanel.SetActive(true);
    }

    public void GoToAudioPanel()
    {
        ResetPanels();
        audioPanel.SetActive(true);
    }

    public void GoToGraphicsPanel()
    {
        ResetPanels();
        graphicsPanel.SetActive(true);
    }

    public void GoToLoadoutPanel()
    {
        ResetPanels();
        loadoutPanel.SetActive(true);
    }

    public void GoToLoadingPanel()
    {
        ResetPanels();
        loadingPanel.SetActive(true);
    }

    public void GoToSaveResetPanel()
    {
        ResetPanels();
        saveResetPanel.SetActive(true);
    }

    private void ResetPanels()
    {
        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
        }
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false);
        }
        if (levelPanel != null)
        {
            levelPanel.SetActive(false);
        }
        if (controlsPanel != null)
        {
            controlsPanel.SetActive(false);
        }
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }
        if (audioPanel != null)
        {
            audioPanel.SetActive(false);
        }
        if (graphicsPanel != null)
        {
            graphicsPanel.SetActive(false);
        }
        if (loadoutPanel != null)
        {
            loadoutPanel.SetActive(false);
        }
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(false);
        }
        if (saveResetPanel != null)
        {
            saveResetPanel.SetActive(false);
        }
    }
}
