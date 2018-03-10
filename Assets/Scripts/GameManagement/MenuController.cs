using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject levelPanel;
    [SerializeField] private GameObject controlsPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject weaponsPanel;
    [SerializeField] private GameObject audioPanel;
    [SerializeField] private GameObject graphicsPanel;

    void Awake()
    {
        GoToMenuPanel();
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

    public void GoToWeaponsPanel()
    {
        ResetPanels();
        weaponsPanel.SetActive(true);
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
        if (weaponsPanel != null)
        {
            weaponsPanel.SetActive(false);
        }
        if (audioPanel != null)
        {
            audioPanel.SetActive(false);
        }
        if (graphicsPanel != null)
        {
            graphicsPanel.SetActive(false);
        }
    }
}
