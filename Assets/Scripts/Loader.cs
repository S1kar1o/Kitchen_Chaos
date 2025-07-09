using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public static int targeSceneIndex;
    public enum Scene {
        MainMenuScen,
        GameScene,
        LoadingScene
    }
    private static Scene targetScene;
    public static void Load(Scene tragetScene)
    {
        Loader.targetScene = tragetScene;

        SceneManager.LoadScene(Scene.LoadingScene.ToString());

    }
    public static void LoaderCallback()
    {
        SceneManager.LoadScene(targetScene.ToString());

    }

}
