using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject welcomePanel;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject controlPanel;

    void Awake()
    {
        // welcomePanel.SetActive(true);
        // menuPanel.SetActive(false);
        GoToMenuPanel();
    }

    public void GoToMenuPanel()
    {
        welcomePanel.SetActive(false);
        menuPanel.SetActive(true);
        controlPanel.SetActive(false);
    }

    public void GoToControlPanel()
    {
        menuPanel.SetActive(false);
        controlPanel.SetActive(true);
    }
}
