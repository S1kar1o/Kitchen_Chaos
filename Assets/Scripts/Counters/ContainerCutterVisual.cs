using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCutterVisual : MonoBehaviour
{
    private const string CUT = "Cut";
    [SerializeField] private CuttingCounter cuttingCounter;
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        cuttingCounter.OnCut += CuttingContainer_OnCut;
    }

    private void CuttingContainer_OnCut(object sender, System.EventArgs e)
    {
        animator.SetTrigger(CUT);
    }
}
