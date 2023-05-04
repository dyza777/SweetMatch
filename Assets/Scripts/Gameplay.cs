using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour
{
    [SerializeField] private FadeInOut _blackScreen;
    [SerializeField] private ObjectsSpawner _spawner;
    [SerializeField] private TimerHandler _timerHandler;
    [SerializeField] private GoalsHandler _goalsHandler;
    [SerializeField] private WinScreenHandler WinScreen;
    [SerializeField] private FailScreenHandler FailScreen;

    bool isGameReady = true;
    void Start()
    {
        var levelData = Resources.Load<LevelData>($"LevelsData/LevelData_{StaticFields.CurrentLevelNumber}");
        _spawner.SetUpLevel(levelData);
        _timerHandler.StartCountdown(levelData.levelTime);
        _goalsHandler.SetupGoals(levelData.levelGoalsInfo);
    }

    void Update()
    {
        TouchCheck();
    }

    void TouchCheck()
    {
        if (!isGameReady || !StaticFields.onboardingCompleted) return;
        Vector2 clickedPos = Vector2.zero;
        if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
        {
            clickedPos = Input.GetTouch(0).position;
        } else if (Input.GetMouseButtonDown(0)) {
            clickedPos = Input.mousePosition;
        } else
        {
            return;
        }

        Ray raycast = Camera.main.ScreenPointToRay(clickedPos);
        RaycastHit raycastHit;
        if (Physics.Raycast(raycast, out raycastHit))
        {
            if (raycastHit.collider.CompareTag("MatchObject"))
            {
                FindObjectOfType<MatchingManager>().OccupyNewCell(raycastHit.collider.GetComponent<MatchObject>());
            }
        }
    }

    public void HandleWin()
    {
        if (!isGameReady) return;
        _timerHandler.StopTimer();
        isGameReady = false;
        WinScreen.SetupAndShowWinScreen(_timerHandler.TimerValue);
        DataManager gameData = new DataManager();
        gameData.Load();
        StaticFields.levelsCompleted[StaticFields.CurrentLevelNumber - 1] = true;
        gameData.Data.levelsCompleted[StaticFields.CurrentLevelNumber - 1] = true;
        gameData.Save();
    }

    public void HandleFail()
    {
        if (!isGameReady) return;
        _timerHandler.StopTimer();
        isGameReady = false;
        FailScreen.SetupAndShowFailScreen();
    }
}
