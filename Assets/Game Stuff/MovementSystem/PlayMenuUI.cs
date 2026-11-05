using UnityEngine;
using UnityEngine.EventSystems;

public class PlayMenuUI : MonoBehaviour
{
    [SerializeField] private PlayManager playManager;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject defaultSelectedButton;

    private void Start()
    {
        playManager.PlayEnded += ShowMenu;
        ShowMenu();
    }

    private void OnDestroy()
    {
        playManager.PlayEnded -= ShowMenu;
    }

    public void ShowMenu()
    {
        menuPanel.SetActive(true);
        Time.timeScale = 0f;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(defaultSelectedButton);
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
