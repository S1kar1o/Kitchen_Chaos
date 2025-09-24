using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    private PlayerController playerController;
    private float footStepTime;
    private float footStepTimeMax = 0.1f;
    private void Awake()
    {
        playerController=GetComponent<PlayerController>();
    }
    private void Update()
    {
        footStepTime -= Time.deltaTime;
        if (footStepTime < 0) { 
            footStepTime = footStepTimeMax;
            if (playerController.IsWalking())
            {
                float volume = 1f;
                SoundManager.Instance.PlayFootStepSound(this.transform.position, volume);
            }
        }
    }
}
