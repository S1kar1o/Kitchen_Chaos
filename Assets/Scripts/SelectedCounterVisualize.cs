using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisualize : MonoBehaviour
{
    [SerializeField] private BaseCounter baseCounter;
    [SerializeField] private GameObject[] visualizeGameObjectArray;
    private void Start()
    {
        PlayerController.Instance.OnSelectedCounterChanged += Instance_OnSelectedCounterChanged;
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
