using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private FadeInOut BlackScreen;
    public void OnLevelPress(int levelNumber)
    {
        StaticFields.CurrentLevelNumber = levelNumber;
        BlackScreen.EnableAndFadeIn();
        StartCoroutine(LoadLevel());
    }

    public void OnMenuPress()
    {
        BlackScreen.EnableAndFadeIn();
        StartCoroutine(GoToMainMenu());
    }

    IEnumerator LoadLevel()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GameScene");
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (!BlackScreen.isFading)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    IEnumerator GoToMainMenu()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainMenu");
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (!BlackScreen.isFading)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
