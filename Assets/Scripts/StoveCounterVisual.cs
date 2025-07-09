using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] StoveCounter stoveCounter;
    [SerializeField] GameObject particlesGameObject;
    [SerializeField] GameObject stoveOnGameObject;

    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        bool showState = e.state == StoveCounter.State.Fried || e.state == StoveCounter.State.Frying;
        particlesGameObject.SetActive(showState);
        stoveOnGameObject.SetActive(showState);
    }
}
