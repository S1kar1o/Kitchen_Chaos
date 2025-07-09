using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button optionsButton;
    private void Awake()
    {
        mainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenuScen);
        });
        optionsButton.onClick.AddListener(() =>
        {
            OptionsUI.Instance.Show(Show);
            Hide();
        });

        resumeButton.onClick.AddListener(() =>
        {
            GameManager.Instance.TogglePauseGame();
        });
        Time.timeScale = 1.0f;
    }
    private void Start()
    {
        GameManager.Instance.OnGamePauseAction += GameManager_OnPauseAction;
        GameManager.Instance.OnGameUnPauseAction += GameManager_OnGameUnPauseAction;
        Hide();
    }

    private void GameManager_OnGameUnPauseAction(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void GameManager_OnPauseAction(object sender, System.EventArgs e)
    {
        Show();
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
