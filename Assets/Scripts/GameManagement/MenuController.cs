using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject welcomePanel;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject controlPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject weaponsPanel;

    void Awake()
    {
        GoToWelcomePanel();
    }

    public void GoToWelcomePanel()
    {
        ResetPanels();
        welcomePanel.SetActive(true);
    }

    public void GoToMenuPanel()
    {
        ResetPanels();
        menuPanel.SetActive(true);
    }

    public void GoToControlPanel()
    {
        ResetPanels();
        controlPanel.SetActive(true);
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

    private void ResetPanels()
    {
        welcomePanel.SetActive(false);
        menuPanel.SetActive(false);
        controlPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        victoryPanel.SetActive(false);
        weaponsPanel.SetActive(false);
    }
}
