using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : Singleton<SceneTransitionManager> {

    [SerializeField]
    private string mainMenuSceneName;

    [SerializeField]
    private string gameSceneName;

    public void ToMainMenuScene() {
        SceneManager.LoadScene(mainMenuSceneName);
    }
    public void ToGameScene() {
        SceneManager.LoadScene(gameSceneName);
    }
}
