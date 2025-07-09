using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterFlashingBarUI : MonoBehaviour
{
    private const string IS_FLASHING = "IsFlashing";
    [SerializeField] private StoveCounter stoveCounter;
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
        animator.SetBool(IS_FLASHING, false);
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        float progresBurning = 0.5f;
        bool flashing = progresBurning <= e.progressNormalized && stoveCounter.IsFried();

        animator.SetBool(IS_FLASHING, flashing);

    }

}
