using UnityEngine;

public class PlayMenuUI : MonoBehaviour
{
    [SerializeField] private PlayManager playManager;
    [SerializeField] private GameObject menuPanel;

    private void Start()
    {
        ShowMenu();
    }

    public void ShowMenu()
    {
        menuPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void HideMenu()
    {
        menuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnPassButtonPressed()
    {
        HideMenu();
        playManager.StartPassPlay();
    }

    public void OnRunButtonPressed()
    {
        HideMenu();
        playManager.StartRunPlay();
    }
}
