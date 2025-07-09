using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] StoveCounter stoveCounter;
    private AudioSource audioSource;
    private float warningSoundTimer;
    private bool isWarningSoundPlay=false;
    private void Update()
    {
        if (isWarningSoundPlay)
        {
            warningSoundTimer -= Time.deltaTime;
            if (warningSoundTimer < 0)
            {
                float warningSoundTimerMax = 0.2f;
                warningSoundTimer = warningSoundTimerMax;
                SoundManager.Instance.PlayStoveBurningWarning(stoveCounter.transform.position);
            }
        }
    }
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        float progresBurning = 0.5f;
        isWarningSoundPlay = progresBurning <= e.progressNormalized && stoveCounter.IsFried();
       
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        bool playSond = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;
        if (playSond)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Pause();
        }

    }
}
