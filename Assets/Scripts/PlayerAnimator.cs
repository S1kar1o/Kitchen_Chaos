using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    private const string IS_WALKING = "IsWalking";
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetBool(IS_WALKING, controller.IsWalking());
    }
    void Update()
    {
        animator.SetBool(IS_WALKING, controller.IsWalking());
    }
}
