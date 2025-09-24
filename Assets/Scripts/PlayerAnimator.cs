using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class PlayerAnimator : NetworkBehaviour
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
        if (!IsOwner)
            return;
        animator.SetBool(IS_WALKING, controller.IsWalking());
    }
}
