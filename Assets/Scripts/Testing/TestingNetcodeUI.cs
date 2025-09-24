using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class TestingNetcodeUI : MonoBehaviour
{
    [SerializeField] Button clientButton;
    [SerializeField] Button hostButton;
    void Start()
    {
        clientButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
            Hide();
            Debug.Log(120);
        });
        hostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            Hide();

        });
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
   
}
