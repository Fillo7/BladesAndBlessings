using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject welcomePanel;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject controlPanel;
    [SerializeField] private GameObject gameOverPanel;

    void Awake()
    {
        // welcomePanel.SetActive(true);
        // menuPanel.SetActive(false);
        // controlPanel.SetActive(false);
        GoToMenuPanel();
    }

    public void GoToMenuPanel()
    {
        welcomePanel.SetActive(false);
        menuPanel.SetActive(true);
        controlPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    public void GoToControlPanel()
    {
        menuPanel.SetActive(false);
        controlPanel.SetActive(true);
    }

    public void GoToGameOverPanel()
    {
        menuPanel.SetActive(false);
        controlPanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }
}
