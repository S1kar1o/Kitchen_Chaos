using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileControllerUI : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(Application.isMobilePlatform); 
    }

}
