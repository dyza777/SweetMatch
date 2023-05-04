using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class InitialLoader : MonoBehaviour
{
    [SerializeField] private GameObject[] LevelButtons;
    void Start()
    {
        if (!StaticFields.isInitialized) Init();

        if (!StaticFields.onboardingCompleted)
        {
            SceneManager.LoadScene("GameScene");
        }

        for (int i = 0; i < LevelButtons.Length; i++)
        {
            if (StaticFields.levelsCompleted[i])
            {
                LevelButtons[i].transform.GetChild(1).gameObject.SetActive(true);
            }
        }
        
    }

    void Init()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            SetScreenSize();
        }

        DataManager gameData = new DataManager();
        gameData.Load();
        StaticFields.onboardingCompleted = gameData.Data.onboardingCompleted;
        StaticFields.levelsCompleted = gameData.Data.levelsCompleted;
        StaticFields.isInitialized = true;
    }

    void SetScreenSize()
    {
        var res = Screen.currentResolution;
        int targetY = (int)(0.8f * res.height);
        int targetX = (int)(targetY * ((float)1125/2436));
        Screen.SetResolution(targetX, targetY, false);
    }
}
