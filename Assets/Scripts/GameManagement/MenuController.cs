using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject controlPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject weaponsPanel;

    void Awake()
    {
        GoToMenuPanel();
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
        menuPanel.SetActive(false);

        if (controlPanel != null)
        {
            controlPanel.SetActive(false);
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
    }
}
