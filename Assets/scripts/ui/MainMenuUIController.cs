using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIController : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] private string gameplaySceneName = "mapa";

    [Header("Main Menu Panels")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject playPanel;
    [SerializeField] private GameObject loadGamePanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject exitPanel;

    [Header("Pause Menu")]
    [SerializeField] private bool usedAsPauseMenu = false;

    private void Start()
    {
        ShowMainPanel();
    }

    public void ShowMainPanel()
    {
        SetPanel(mainPanel, true);
        SetPanel(playPanel, false);
        SetPanel(loadGamePanel, false);
        SetPanel(settingsPanel, false);
        SetPanel(exitPanel, false);
    }

    public void ShowPlayPanel()
    {
        SetPanel(mainPanel, true);
        SetPanel(playPanel, true);
        SetPanel(loadGamePanel, false);
        SetPanel(settingsPanel, false);
        SetPanel(exitPanel, false);
    }

    public void ShowLoadGamePanel()
    {
        SetPanel(mainPanel, true);
        SetPanel(playPanel, true);
        SetPanel(loadGamePanel, true);
        SetPanel(settingsPanel, false);
        SetPanel(exitPanel, false);
    }

    public void OpenSettings()
    {
        SetPanel(mainPanel, false);
        SetPanel(playPanel, false);
        SetPanel(loadGamePanel, false);
        SetPanel(settingsPanel, true);
        SetPanel(exitPanel, false);
    }

    public void ShowExitPanel()
    {
        SetPanel(mainPanel, true);
        SetPanel(playPanel, false);
        SetPanel(loadGamePanel, false);
        SetPanel(settingsPanel, false);
        SetPanel(exitPanel, true);
    }

    public void CancelExit()
    {
        ShowMainPanel();
    }

    public void NewGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void ContinueGame()
    {
        if (usedAsPauseMenu)
        {
            Time.timeScale = 1f;
            gameObject.SetActive(false);
            return;
        }

        Debug.Log("Continue pressed from main menu. Save/load logic will be added later.");
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void SetPanel(GameObject panel, bool active)
    {
        if (panel != null)
        {
            panel.SetActive(active);
        }
    }
}