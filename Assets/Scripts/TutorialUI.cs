using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    private const string USER_SOW_TUTORIAL = "UserSawTutorial";
    public event EventHandler OnTutorialWathced;
    public static TutorialUI Instance {  get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        GameManager.Instance.OnLocalPlayerReadyChanged += GameManager_OnLocalPlayerReadyChanged;
        StartCoroutine(Waiting());
    }

    private void GameManager_OnLocalPlayerReadyChanged(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsLocalPlayerReady())
        {
            Hide();
        }
    }

    private IEnumerator Waiting()
    {
        yield return new WaitForSeconds(0.1f);

        if (PlayerPrefs.GetInt(USER_SOW_TUTORIAL) == 0)
        {
            Show();
        }
        else
        {
            OnTutorialWathced?.Invoke(this, EventArgs.Empty);
            Hide();
        }
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsCountdownToPlay())
        {
            Hide();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
        PlayerPrefs.SetInt(USER_SOW_TUTORIAL, 1);
        PlayerPrefs.Save();
    }

}
