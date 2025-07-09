using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;
    private Animator animator;
    private int previousCountDownNumber;
    private const string NUMBER_POP_UP= "NumberPopUp";
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        Hide();
    }
    private void Update()
    {
        int countDownNimber = Mathf.CeilToInt(GameManager.Instance.GetCountDownTimerTOStart());
        countdownText.text= countDownNimber.ToString();
        if(previousCountDownNumber!=countDownNimber)
        {
            previousCountDownNumber = countDownNimber;
            animator.SetTrigger(NUMBER_POP_UP);
            SoundManager.Instance.PlayCountDownSound();
        }
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsCountdownToPlay())
        {
            Show();
        }
        else
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

    }
}
