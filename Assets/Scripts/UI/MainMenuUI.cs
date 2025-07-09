using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playGameButton; 
    [SerializeField] private Button exitGameButton;
    private void Awake()
    {
        playGameButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.GameScene);
        });
        exitGameButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
   
}
