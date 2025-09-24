using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }
    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePauseAction;
    public event EventHandler OnGameUnPauseAction;
    public event EventHandler OnLocalPlayerReadyChanged;
    private Dictionary<ulong,bool> playersReadyDictionary;
    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver
    }
    private State state;
    private bool isLocalPlayerRedy;
    private float countdownForStartTimer = 3f;
    private float gamePlayingTimer;
    private float gamePlayingTimerMax = 120f;

    private bool isGamePause = false;
    private void Awake()
    {
        state = State.WaitingToStart;
        Instance = this;
        playersReadyDictionary = new Dictionary<ulong, bool>();

    }
    private void Start()
    {
        InputSystem.Instance.OnPauseAction += InputSystem_OnPauseAction;
        InputSystem.Instance.OnInteractAction += InputSystem_OnInteractAction;
        TutorialUI.Instance.OnTutorialWathced += TutorialUI_OnTutorialWathced;

    }
    [ServerRpc(RequireOwnership =false)]
    private void SetPlayerReadyServerRpc()
    {

    }
    private void TutorialUI_OnTutorialWathced(object sender, EventArgs e)
    {
        if (state == State.WaitingToStart)
        {
            state = State.CountdownToStart;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void InputSystem_OnInteractAction(object sender, EventArgs e)
    {
        if (state == State.WaitingToStart)
        {
            isLocalPlayerRedy = true;
            OnLocalPlayerReadyChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void InputSystem_OnPauseAction(object sender, EventArgs e)
    {

        TogglePauseGame();

    }

    private void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:

                break;
            case State.CountdownToStart:
                countdownForStartTimer -= Time.deltaTime;
                if (countdownForStartTimer <= 0f)
                {
                    state = State.GamePlaying;
                    gamePlayingTimer = gamePlayingTimerMax;
                    OnStateChanged.Invoke(this, EventArgs.Empty);

                }
                break;
            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer <= 0f)
                {
                    state = State.GameOver;
                    OnStateChanged.Invoke(this, EventArgs.Empty);

                }
                break;
            case State.GameOver:

                OnStateChanged.Invoke(this, EventArgs.Empty);
                break;
        }
    }
    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;
    }
    public bool IsCountdownToPlay()
    {
        return state == State.CountdownToStart;
    }
    public float GetCountDownTimerTOStart()
    {
        return countdownForStartTimer;
    }
    public bool IsGameOver()
    {
        return state == State.GameOver;
    }
    public float GetPlayingTimerNormalized()
    {
        return 1 - (gamePlayingTimer / gamePlayingTimerMax);
    }
    public void TogglePauseGame()
    {
        isGamePause = !isGamePause;
        if (isGamePause)
        {
            Time.timeScale = 0f;
            OnGamePauseAction?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1.0f;
            OnGameUnPauseAction?.Invoke(this, EventArgs.Empty);
        }
    }
    public bool IsLocalPlayerReady()
    {
        return isLocalPlayerRedy;
    }
}
