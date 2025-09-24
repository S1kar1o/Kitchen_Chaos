using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisualize : MonoBehaviour
{
    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] visualizeGameObjectArray;
    private void Start()
    {
        if (PlayerController.LocalInstance != null)
        {
            PlayerController.LocalInstance.OnSelectedCounterChanged += Instance_OnSelectedCounterChanged;
        }
        else
        {
            PlayerController.OnAnyPlayerSpawned += PlayerController_OnAnyPlayerSpawned;
        }
    }

    private void PlayerController_OnAnyPlayerSpawned(object sender, System.EventArgs e)
    {
        if (PlayerController.LocalInstance != null)
        {
            PlayerController.LocalInstance.OnSelectedCounterChanged -= Instance_OnSelectedCounterChanged;
            PlayerController.LocalInstance.OnSelectedCounterChanged += Instance_OnSelectedCounterChanged;

        }
    }

    private void Instance_OnSelectedCounterChanged(object sender, PlayerController.OnSelectedCounterChangedEventArgs e)
    {
        if (e.selectedCounter == baseCounter) {
            Show();
        }
        else
        {
            Hide();
        }
    }
    private void Show()
    {
        foreach (GameObject visualGameObject in visualizeGameObjectArray) {
            visualGameObject.SetActive(true); 
        }
    }
    private void Hide()
    {
        foreach (GameObject visualGameObject in visualizeGameObjectArray)
        {
            visualGameObject?.SetActive(false);
        }
    }
}
